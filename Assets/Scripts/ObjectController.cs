using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Firebase;
using Firebase.Storage;

public class ObjectController : MonoBehaviour
{
    public float planeY; // 床の高さ

    DevLog devLog;
    FadeController fadeController;
    NextController nextController;

    void Start() {
        // Firebase初期化
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                Debug.Log("Firebase初期化成功");
            } else {
                Debug.Log("Firebase初期化失敗");
            }
        });

        devLog = GetComponent<DevLog>();
        fadeController = GetComponent<FadeController>();
        nextController = GetComponent<NextController>();
    }

    // オブジェクトの設置
    public async void CreateObject(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        fadeController.action = () => {
            // ここでコルーチンスタート
            //Instantiate(objectPrefab, new Vector3(cameraPos.x+x, planeY ,cameraPos.z+z), Quaternion.identity);
            LoadUri(
                data["name"],
                new Vector3(cameraPos.x, planeY, cameraPos.z),
                uint.Parse(data["crc"]));

            // nextCheck
            nextController.CheckNext("Create");
        };

        fadeController.isFadeOut = true;
    }

    // URIを取得
    async void LoadUri(string name, Vector3 position, uint crc) {
        // ストレージアクセスインスタンスの取得
        var _storage = FirebaseStorage.DefaultInstance;
        // 作成したストレージURIを指定
        var storage_ref = _storage.GetReferenceFromUrl("gs://yonaki.appspot.com");
        // ダウンロードしたいAssetBundleのストレージ内におけるパスを指定
        var prefab_ref = storage_ref.Child($"prefabs/{name}");
        // AssetBundleのURIを取得
        await prefab_ref.GetDownloadUrlAsync().ContinueWith((Task<Uri> fetchTask) => {
            if (!fetchTask.IsFaulted && !fetchTask.IsCanceled) {
                Debug.Log("URI取得成功");
                StartCoroutine(LoadAsset(fetchTask.Result.AbsoluteUri, name, position, crc));
            } else {
                Debug.Log("URI取得失敗");
            }
        });
    }

    // Assetを取得・設置
    IEnumerator LoadAsset(string uri, string name, Vector3 position, uint crc) {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0, crc)) {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError) {
                Debug.Log($"AssetBundleのダウンロードに失敗しました: {uwr.error}");
            } else {
                // ダウンロード成功
                Debug.Log("AssetBundleのダウンロードに成功");
                var bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                var prefab = bundle.LoadAssetAsync(name);
                // AR空間に生成
                var newObject = (GameObject)Instantiate(prefab.asset, position, Quaternion.identity);
                fadeController.isFadeIn = true;
            }
        }
    }

    public void DestroyObject(string tag) {
        Destroy(GameObject.FindGameObjectWithTag(tag));
    }
}
