using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] GameObject Cube;

    CreateObject createObject;
    NextController nextController;
    DevLog devLog;

    void Start() {
        createObject = GameObject.Find("AR Session Origin").GetComponent<CreateObject>();
        nextController = GetComponent<NextController>();
        devLog = GetComponent<DevLog>();
    }

    // オブジェクトのセッティング
    public void SetObjectPrefab(string name) {
        GameObject newObject = Sphere;
        devLog.SendLog($"SetObjectPrefabが起動しました name:{name}");

        switch (name) {
            case "Sphere":
                newObject = Sphere;
                break;

            case "Cube":
                newObject = Cube;
                break;
            
            default:
                devLog.SendLog($"未知の名前です。登録されているか確認してください\nname: {name}");
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
                devLog.SendLog("オブジェクトを生成した時にnextを送ります");
                nextController.trigger = "Drop";
                break;
            
            // 中心に捉えた時
            case "Find":
                devLog.SendLog("オブジェクトを中心に捉えたときにnextを送ります");
                nextController.trigger = "Find";
                break;

            // 拾った時
            case "PickUp":
                devLog.SendLog("拾ったときにnextを送ります");
                nextController.trigger = "PickUp";
                break;
            
            // 捕まった時
            case "Caught":
                Debug.Log("捕まった時にnextを送ります");
                nextController.trigger = "Caught";
                break;

            default:
                devLog.SendLog($"未知のトリガーを設定しようとしました。登録されているか確認してください\ntrigger: {trigger}");
                break;
        }
    }
}
