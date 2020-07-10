using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    NextController nextController;

    GameObject chaser;
    float speed = 0;

    void Start() {
        nextController = GetComponent<NextController>();
    }

    void Update() {
        if (chaser != null && speed != 0) {
            Debug.Log("追いかけます");
            float step = Time.deltaTime * speed;
            Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
            Transform chaserTransform = chaser.GetComponent<Transform>();
            Vector3 target = new Vector3(cameraPos.x, chaserTransform.position.y, cameraPos.z);

            // ユーザーの足元をむく
            chaserTransform.forward = chaserTransform.position - target;
            // ユーザーの足元を追いかける
            chaserTransform.position = Vector3.MoveTowards(chaserTransform.position, target, step);

            // 捕まえたか評価
            if (chaserTransform.position == target) {
                Debug.Log("プレイヤーと衝突しました");
                chaser = null;
                speed = 0;
                nextController.CheckNext("Caught");
            }
        }
    }

    public void SetSpeed(string newSpeed) {
        Debug.Log($"スピードを {newSpeed}% に設定します");
        speed = int.Parse(newSpeed) / 100f;
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    public void SetChaser(string tag) {
        Debug.Log($"追いかけてくるオブジェクトを設定します tag: {tag}");
        chaser = GameObject.FindGameObjectWithTag(tag);
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }
}
