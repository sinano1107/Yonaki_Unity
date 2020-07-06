using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRay : MonoBehaviour
{
    string state = "";
    Vector3 center = new Vector3(Screen.width/2, Screen.height/2);
    RaycastHit hit;
    int distance = 3;

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(center);

        if (Physics.Raycast(ray,out hit,distance)) {
            // オブジェクトと当たった場合
            string tag = hit.collider.tag;

            // 状態が変わったとき
            if (state != tag && tag != "Untagged") {
                state = tag;
                Debug.Log($"{tag}を見つけました");
            }
        } else {
            // オブジェクトと当たらなかったとき
            if (state != "") {
                state = "";
                Debug.Log("オブジェクトを見失いました");
            }
        }
    }
}
