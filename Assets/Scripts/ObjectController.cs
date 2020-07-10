using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectController : MonoBehaviour
{
    public GameObject objectPrefab; // 設置するオブジェクト
    public float planeY; // 床の高さ

    FadeController fadeController;
    NextController nextController;

    void Start() {
        fadeController = GetComponent<FadeController>();
        nextController = GetComponent<NextController>();
    }

    // オブジェクトの設置
    public void CreateObject(string spaceStr) {
        Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        float space = float.Parse(spaceStr); // カメラを中心としたオブジェクトを設置しない範囲

        fadeController.action = () => {
            Instantiate(objectPrefab, new Vector3(space+cameraPos.x+x, planeY , space+cameraPos.z+z), Quaternion.identity);

            UnityMessageManager.Instance.SendMessageToFlutter("next");
        };

        fadeController.isFadeOut = true;
    }
}
