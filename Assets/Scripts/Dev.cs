using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dev : MonoBehaviour
{
    GameObject devText;

    public void EditDevText(string newText) {
        this.devText.GetComponent<Text>().text = newText;
    }

    void Start() {
        devText = GameObject.Find("DevText");
        EditDevText("devが起動しています");
    }
}
