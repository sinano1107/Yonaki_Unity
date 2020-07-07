using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] GameObject Cube;

    CreateObject createObject;

    void Start() {
        createObject = GameObject.Find("AR Session Origin").GetComponent<CreateObject>();
    }

    public void SetObjectPrefab(string name) {
        GameObject newObject = Sphere;

        switch (name) {
            case "Sphere":
                newObject = Sphere;
                break;

            case "Cube":
                newObject = Cube;
                break;
            
            default:
                Debug.Log($"未知の名前です。登録されているか確認してください\nname: {name}");
                break;
        }

        createObject.objectPrefab = newObject;
    }
}
