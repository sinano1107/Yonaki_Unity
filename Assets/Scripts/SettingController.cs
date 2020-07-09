using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] GameObject Cube;

    CreateObject createObject;
    NextController nextController;

    void Start() {
        createObject = GameObject.Find("AR Session Origin").GetComponent<CreateObject>();
        nextController = GetComponent<NextController>();
    }

    // オブジェクトのセッティング
    public void SetObjectPrefab(string name) {
        GameObject newObject = Sphere;
        GetComponent<Dev>().EditDevText($"SetObjectPrefabが起動しました name:{name}");

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

        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    // nextを送る際のトリガーをセッティング
    public void SetTrigger(string trigger) {
        switch (trigger) {

            // オブジェクトを生成した時
            case "Drop":
                Debug.Log("オブジェクトを生成した時にnextを送ります");
                nextController.trigger = "Drop";
                break;
            
            // 中心に捉えた時
            case "Find":
                Debug.Log("オブジェクトを中心に捉えたときにnextを送ります");
                nextController.trigger = "Find";
                break;

            // 拾った時
            case "PickUp":
                Debug.Log("拾ったときにnextを送ります");
                nextController.trigger = "PickUp";
                break;

            default:
                Debug.Log($"未知のトリガーを設定しようとしました。登録されているか確認してください\ntrigger: {trigger}");
                break;
        }
    }
}
