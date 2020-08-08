using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepDirector : MonoBehaviour
{
    public void Restart() {
        GetComponent<DevLog>().SendLog("リスタートします");
        SceneManager.LoadScene("Scenes/GameScene");
    }
}
