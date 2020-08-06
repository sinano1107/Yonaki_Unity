using Newtonsoft.Json;
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

    public void SetAnim(string strData) {
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);
        devLog.SendLog($"アニメを {data["num"]} に設定します");
        GameObject target = GameObject.FindGameObjectWithTag("Object");
        target.GetComponent<Animator>().SetInteger("Animation", int.Parse(data["num"]));
    }
}
