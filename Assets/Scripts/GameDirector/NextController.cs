﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextController : MonoBehaviour
{
    public string trigger;

    public void CheckNext(string triggerName) {
        if (triggerName == trigger) {
            trigger = null;
            UnityMessageManager.Instance.SendMessageToFlutter("next");
        }
    }
}
