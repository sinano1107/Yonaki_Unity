using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Networking;

public class ObjectController : MonoBehaviour
{
    public float planeY; // 床の高さ

    DevLog devLog;
    FadeController fadeController;
    NextController nextController;

    AssetBundle assetBundle;
    // アセットバンドルの記録用
    Dictionary<string, Object> assets = new Dictionary<string, Object>();

    void Start() {
        devLog = GetComponent<DevLog>();
        fadeController = GetComponent<FadeController>();
        nextController = GetComponent<NextController>();
    }

    // オブジェクトの設置
    public void CreateObject(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
        int number = int.Parse(data["number"]); // 個数
        float space = float.Parse(data["space"]); // カメラを中心としたオブジェクトを設置しない範囲

        fadeController.action = () => {
            if (assets.ContainsKey(data["name"])) {
                // すでにAssetを読み込んでいたら
                InstantiateObject(assets[data["name"]], cameraPos, number, space);
            } else {
                // Assetを読み込んでいない場合
                // ここでコルーチンスタート
                StartCoroutine(LoadAsset(
                    data["uri"],
                    data["name"],
                    cameraPos, 
                    uint.Parse(data["crc"]),
                    number,
                    space
                ));
            }
        };

        fadeController.isFadeOut = true;
    }

    // Assetを取得・設置
    IEnumerator LoadAsset(string uri, string name, Vector3 cameraPos, uint crc, int number, float space) {
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
                InstantiateObject(prefab.asset, cameraPos, number, space);
            }
        }
    }

    void InstantiateObject(Object prefab, Vector3 cameraPos, int number, float space) {
        Vector3[] positiones = RandomPositiones(cameraPos, number, space);
        for (int i=0; i<number; i++) // 個数分繰り返す
        {
            // positionの演算
            Vector3 position = positiones[i];

            // 角度の算出
            float yRotation = Random.Range(-180f, 180f);

            // AR空間に生成
            var newObject = (GameObject)Instantiate(prefab, position, Quaternion.Euler(0, yRotation, 0));
            // 親にタグを登録
            newObject.tag = "Object";
            // 子にタグを登録
            List<GameObject> children = GetAllChildren.GetAll(newObject);
            foreach (GameObject obj in children)
            {
                obj.tag = "CH_Object";
            }
        }

        UnityMessageManager.Instance.SendMessageToFlutter("next");
        fadeController.isFadeIn = true;
    }

    // ドーナツ状の座標を取得
    Vector3[] RandomPositiones(Vector3 cameraPos, int number, float space) {
        float max = space + 1;
        float min = space;

        Vector3[] answer = new Vector3[number];

        float x;
        float z;

        float xAbs;
        float zAbs;

        float maxR = Mathf.Pow(max, 2);
        float minR = Mathf.Pow(min, 2);

        for (int i = 0; i < number; i++)
        {
            while (answer[i].x == 0)
            {
                x = Random.Range(-max, max);
                z = Random.Range(-max, max);

                xAbs = Mathf.Abs(Mathf.Pow(x, 2));
                zAbs = Mathf.Abs(Mathf.Pow(z, 2));

                // 特定の範囲内か確認
                if (maxR > xAbs + zAbs && xAbs + zAbs > minR)
                    answer[i] = new Vector3(cameraPos.x+x, planeY, cameraPos.z+z);
            }
        }

        return answer;
    }

    // オブジェクトを削除
    public void DestroyObject() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }
    }

    // AssetBundleのアンロード
    public void Unload() {
        if (assetBundle != null)
            assetBundle.Unload(true);
    }
}
