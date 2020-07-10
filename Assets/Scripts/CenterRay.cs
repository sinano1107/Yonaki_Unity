using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRay : MonoBehaviour
{
    DevLog devLog;
    PickUpController pickUpController;
    NextController nextController;

    string state = "";
    Vector3 center = new Vector3(Screen.width/2, Screen.height/2);
    RaycastHit hit;
    int distance = 10;

    void Start() {
        devLog = GetComponent<DevLog>();
        pickUpController = GetComponent<PickUpController>();
        nextController = GetComponent<NextController>();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(center);

        if (Physics.Raycast(ray,out hit,distance)) {
            // オブジェクトと当たった場合
            string tag = hit.collider.tag;

            // 状態が変わったとき
            if (state != tag && tag != "Untagged") {
                state = tag;
                devLog.SendLog($"{tag}を見つけました");
                pickUpController.TogglePickUpButton(
                    active: true,
                    newText: $"{tag}を拾う",
                    newTargetTag: tag);
                
                // nextCheck
                nextController.CheckNext("Find");
            }
        } else {
            // オブジェクトと当たらなかったとき
            if (state != "") {
                state = "";
                devLog.SendLog("オブジェクトを見失いました");
                pickUpController.TogglePickUpButton(
                    active: false,
                    newText: "",
                    newTargetTag: "");
            }
        }
    }
}
