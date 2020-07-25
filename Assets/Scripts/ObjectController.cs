using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectController : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] GameObject Cube;
    [SerializeField] GameObject Cylinder;
    [SerializeField] GameObject Sasuke;
    [SerializeField] GameObject Eyeball;
    [SerializeField] GameObject Menasi;

    public float planeY; // 床の高さ

    DevLog devLog;
    FadeController fadeController;
    NextController nextController;

    void Start() {
        devLog = GetComponent<DevLog>();
        fadeController = GetComponent<FadeController>();
        nextController = GetComponent<NextController>();
    }

    // オブジェクトの設置
    // [TODO] Objectを指定してcreateできるようにしたい
    public void CreateObject(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        GameObject objectPrefab = getObjectPrefab(data["name"]);
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

    // objectPrefabを取得する
    GameObject getObjectPrefab(string name) {
        switch (name) {
            case "Sphere":
                return Sphere;
                
            
            case "Cube":
                return Cube;
                

            case "Cylinder":
                return Cylinder;
                

            case "Sasuke":
                return Sasuke;
                

            case "Eyeball":
                return Eyeball;
                

            case "Menasi":
                return Menasi;
                

            default:
                devLog.SendLog($"未知の名前です。登録されているか確認してください\nname: {name}");
                return Sasuke;
        }
    }

    public void DestroyObject(string tag) {
        Destroy(GameObject.FindGameObjectWithTag(tag));
    }
}
