using Newtonsoft.Json;
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
        int num = int.Parse(data["num"]);
        devLog.SendLog($"アニメを {data["num"]} に設定します");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Object");
        foreach(GameObject target in targets)
        {
            target.GetComponent<Animator>().SetInteger("Animation", num);
        }
    }
}
