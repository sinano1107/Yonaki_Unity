using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRay : MonoBehaviour
{
    PickUpController pickUpController;
    NextController nextController;

    string state = "";
    Vector3 center = new Vector3(Screen.width/2, Screen.height/2);
    RaycastHit hit;
    int distance = 3;

    void Start() {
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
                Debug.Log($"{tag}を見つけました");
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
                Debug.Log("オブジェクトを見失いました");
                pickUpController.TogglePickUpButton(
                    active: false,
                    newText: "",
                    newTargetTag: "");
            }
        }
    }
}
