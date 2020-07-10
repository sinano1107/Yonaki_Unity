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
    public void CreateObject() {
        Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        fadeController.action = () => {
            Instantiate(objectPrefab, new Vector3(cameraPos.x+x, planeY ,cameraPos.z+z), Quaternion.identity);

            // nextCheck
            nextController.CheckNext("Create");
        };

        fadeController.isFadeOut = true;
    }
}
