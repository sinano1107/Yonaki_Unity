using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameDirector : MonoBehaviour
{
    ARPlaneManager planeManager;

    void Start() {
        planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        // 平面検知を開始
        planeManager.detectionMode = PlaneDetectionMode.Horizontal;

        // planeManagerをアクティブ化
        planeManager.SetTrackablesActive(true);

        // 平面のプレハブをアクティブ化
        planeManager.planePrefab.SetActive(true);
    }

    public void Sleep() {
        // AssetBundleのアンロード
        GetComponent<DevLog>().SendLog("スリープします");
        GetComponent<ObjectController>().Unload();
        SceneManager.LoadScene("Scenes/SleepScene");
    }
}
