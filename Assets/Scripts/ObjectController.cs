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

public class ObjectController : MonoBehaviour
{
    public float planeY; // 床の高さ

    DevLog devLog;
    FadeController fadeController;
    NextController nextController;

    AssetBundle assetBundle;
    // アセットバンドルの記録用
    Dictionary<string, UnityEngine.Object> assets = new Dictionary<string, UnityEngine.Object>();

    void Start() {
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
        float space = float.Parse(data["space"]); // カメラを中心としたオブジェクトを設置しない範囲
        x = (x > 0) ? x+space : x-space;
        z = (z > 0) ? z+space : z-space;
        Vector3 position = new Vector3(cameraPos.x+x, planeY, cameraPos.z+z);

        fadeController.action = () => {
            if (assets.ContainsKey(data["name"])) {
                // すでにAssetを読み込んでいたら
                InstantiateObject(assets[data["name"]], position);
            } else {
                // Assetを読み込んでいない場合
                // ここでコルーチンスタート
                StartCoroutine(LoadAsset(
                    data["uri"],
                    data["name"],
                    position,
                    uint.Parse(data["crc"])));
            }
        };

        fadeController.isFadeOut = true;
    }

    // Assetを取得・設置
    IEnumerator LoadAsset(string uri, string name, Vector3 position, uint crc) {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0, crc)) {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError) {
                devLog.SendLog($"AssetBundleのダウンロードに失敗しました: {uwr.error}");
            } else {
                // ダウンロード成功
                devLog.SendLog("AssetBundleのダウンロードに成功");
                assetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
                var prefab = assetBundle.LoadAssetAsync(name);
                assets[name] = prefab.asset;
                InstantiateObject(prefab.asset, position);
            }
        }
    }

    void InstantiateObject(UnityEngine.Object prefab, Vector3 position) {
        // AR空間に生成
        var newObject = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        // 親にタグを登録
        newObject.tag = "Object";
        // 子にタグを登録
        List<GameObject> children = GetAllChildren.GetAll(newObject);
        foreach (GameObject obj in children) {
            obj.tag = "Object";
        }

        fadeController.isFadeIn = true;
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    // オブジェクトを削除
    public void DestroyObject() {
        Destroy(GameObject.FindGameObjectWithTag("Object"));
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    // AssetBundleのアンロード
    public void Unload() {
        assetBundle.Unload(true);
    }
}
