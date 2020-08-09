using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopController : MonoBehaviour
{
    DevLog devLog;

    void Start()
    {
        devLog = GetComponent<DevLog>();
    }

    public void Pop() {
        devLog.SendLog("Flutterさんpopしてください");
    }
}
