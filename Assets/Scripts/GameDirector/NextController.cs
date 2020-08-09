using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextController : MonoBehaviour
{
    DevLog devLog;

    public string trigger;

    void Start() {
        devLog = GetComponent<DevLog>();
    }

    public void CheckNext(string triggerName) {
        if (triggerName == trigger)
            devLog.SendLog("Flutterにnextを送ります");
    }
}
