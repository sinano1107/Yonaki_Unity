using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    NextController nextController;
    DevLog devLog;

    GameObject chaser;
    float speed = 0;
    float collider = 0; // 捕まったと感知する距離

    void Start() {
        nextController = GetComponent<NextController>();
        devLog = GetComponent<DevLog>();
    }

    void Update() {
        if (chaser != null && speed != 0) {
            float step = Time.deltaTime * speed;
            Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
            Transform chaserTransform = chaser.GetComponent<Transform>();
            Vector3 target = new Vector3(cameraPos.x, chaserTransform.position.y, cameraPos.z);

            // ユーザーの足元をむく
            chaserTransform.forward = chaserTransform.position - target;
            // ユーザーの足元を追いかける
            chaserTransform.position = Vector3.MoveTowards(chaserTransform.position, target, step);

            // 捕まえたか評価
            if (Vector3.Distance(chaserTransform.position, target) <= collider) {
                devLog.SendLog("プレイヤーと衝突しました");
                chaser = null;
                speed = 0;
                collider = 0;
                nextController.CheckNext("Caught");
            }
        }
    }

    public void SetSpeed(string newSpeed) {
        devLog.SendLog($"スピードを {newSpeed}% に設定します");
        speed = int.Parse(newSpeed) / 100f;
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    public void SetChaser(string tag) {
        devLog.SendLog($"追いかけてくるオブジェクトを設定します tag: {tag}");
        chaser = GameObject.FindGameObjectWithTag(tag);
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    public void SetCollider(string newCollider) {
        devLog.SendLog($"chaserとの当たり判定の距離を {newCollider} に設定します");
        collider = float.Parse(newCollider);
    }
}
