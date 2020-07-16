using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] GameObject Sphere;
    [SerializeField] GameObject Cube;
    [SerializeField] GameObject Cylinder;
    [SerializeField] GameObject Sasuke;
    [SerializeField] GameObject Eyeball;
    [SerializeField] GameObject Menasi;

    ObjectController objectController;
    NextController nextController;
    DevLog devLog;

    void Start() {
        objectController = GetComponent<ObjectController>();
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
            
            case "Cylinder":
                newObject = Cylinder;
                break;
            
            case "Sasuke":
                newObject = Sasuke;
                break;

            case "Eyeball":
                newObject = Eyeball;
                break;

            case "Menasi":
                newObject = Menasi;
                break;
            
            default:
                devLog.SendLog($"未知の名前です。登録されているか確認してください\nname: {name}");
                break;
        }

        objectController.objectPrefab = newObject;
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    // nextを送る際のトリガーをセッティング
    public void SetTrigger(string trigger) {
        switch (trigger) {

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

            default:
                devLog.SendLog($"未知のトリガーを設定しようとしました。登録されているか確認してください\ntrigger: {trigger}");
                break;
        }
    }
}
