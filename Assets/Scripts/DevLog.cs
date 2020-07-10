using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DevLog : MonoBehaviour
{
    public void SendLog(string log) {
        Debug.Log(log);

        // Flutterに送信
        Dictionary<string, string> message = new Dictionary<string, string>()
        {
            {"process", "log"},
            {"log", log},
        };
        UnityMessageManager.Instance.SendMessageToFlutter(
            JsonConvert.SerializeObject(message)
        );
    }
}
