using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public string targetName;

    DevLog devLog;

    void Start() {
        devLog = GetComponent<DevLog>();
    }

    public void SetAnimTarget(string name) {
        devLog.SendLog($"アニメのターゲットを {name} に設定します");
        targetName = name;
    }

    public void SetAnim(int num) {
        devLog.SendLog($"{targetName} のアニメを {num}に設定します");
        GameObject.Find($"{targetName}(Clone)").GetComponent<Animator>().SetInteger("Animation", num);
    }
}
