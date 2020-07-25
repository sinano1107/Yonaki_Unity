using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    NextController nextController;
    CenterRay centerRay;
    DevLog devLog;

    void Start() {
        nextController = GetComponent<NextController>();
        centerRay = GetComponent<CenterRay>();
        devLog = GetComponent<DevLog>();
    }

    // nextを送る際のトリガーをセッティング
    public void SetTrigger(string trigger) {
        switch (trigger) {
            // 中心に捉えた時
            case "Find":
                devLog.SendLog("オブジェクトを中心に捉えたときにnextを送ります");
                nextController.trigger = "Find";
                // 拾うボタンを非表示
                centerRay.showPickUpButton = false;
                break;

            // 拾った時
            case "PickUp":
                devLog.SendLog("拾ったときにnextを送ります");
                nextController.trigger = "PickUp";
                // 拾うボタンを表示
                centerRay.showPickUpButton = true;
                break;

            default:
                devLog.SendLog($"未知のトリガーを設定しようとしました。登録されているか確認してください\ntrigger: {trigger}");
                break;
        }
    }
}
