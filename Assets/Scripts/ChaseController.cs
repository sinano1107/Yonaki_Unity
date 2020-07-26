using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    DevLog devLog;

    GameObject chaser;
    float speed = 0;
    float collider = 0; // 捕まったと感知する距離

    void Start() {
        devLog = GetComponent<DevLog>();
    }

    void Update() {
        if (chaser != null) {
            float step = Time.deltaTime * speed;
            Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
            Transform chaserTransform = chaser.GetComponent<Transform>();
            Vector3 target = new Vector3(cameraPos.x, chaserTransform.position.y, cameraPos.z);

            // ユーザーの足元をむく
            chaserTransform.forward = target - chaserTransform.position;
            // ユーザーの足元を追いかける
            chaserTransform.position = Vector3.MoveTowards(chaserTransform.position, target, step);

            // 捕まえたか評価
            if (Vector3.Distance(chaserTransform.position, target) <= collider) {
                devLog.SendLog("プレイヤーと衝突しました");
                chaser = null;
                speed = 0;
                collider = 0;
                UnityMessageManager.Instance.SendMessageToFlutter("next");
            };
        }
    }

    public void StartChase(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        devLog.SendLog($"{data["tag"]} が {data["newSpeed"]}% の速さで追いかけてきます。 当たり判定は {data["newCollider"]} です");
        speed = int.Parse(data["newSpeed"]) / 100f;
        chaser = GameObject.FindGameObjectWithTag(data["tag"]);
        collider = float.Parse(data["newCollider"]);
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }
}
