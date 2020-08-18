using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    DevLog devLog;
    ObjectController objectController;

    GameObject[] chasers = new GameObject[0];
    float speed = 0;
    new float collider = 0; // 捕まったと感知する距離

    void Start() {
        devLog = GetComponent<DevLog>();
        objectController = GetComponent<ObjectController>();
    }

    void Update() {
        if (chasers.Length != 0) {
            float step = Time.deltaTime * speed;
            Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
            Vector3 target = new Vector3(cameraPos.x, objectController.planeY, cameraPos.z);

            for (int i=0; i < chasers.Length; i++)
            {
                GameObject chaser = chasers[i];
                Transform chaserTransform = chaser.GetComponent<Transform>();

                // ユーザーの足元をむく
                chaserTransform.forward = target - chaserTransform.position;
                // ユーザーの足元を追いかける
                chaserTransform.position = Vector3.MoveTowards(chaserTransform.position, target, step);

                // 捕まえたか評価
                if (Vector3.Distance(chaserTransform.position, target) <= collider)
                {
                    devLog.SendLog("プレイヤーと衝突しました");
                    chasers = new GameObject[0];
                    speed = 0;
                    collider = 0;
                };
            }
        }
    }

    public void StartChase(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        devLog.SendLog($"{data["newSpeed"]}% の速さで追いかけてきます。 当たり判定は {data["newCollider"]} です");
        speed = int.Parse(data["newSpeed"]) / 100f;
        chasers = GameObject.FindGameObjectsWithTag("Object");
        collider = float.Parse(data["newCollider"]);
    }
}
