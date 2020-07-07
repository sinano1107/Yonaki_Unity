using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class CreateObject : MonoBehaviour
{
    public GameObject objectPrefab; // 設置するオブジェクト

    ARRaycastManager raycastManager;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    ARPlaneManager planeManager;

    FadeController fadeController;

    bool isDropped = false; // すでにオトシモノを設置したか

    void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Start() {
        fadeController = GameObject.Find("GameDirector").GetComponent<FadeController>();
    }

    void Update() {
        // タッチ時
        if (Input.GetMouseButtonDown(0) && !isDropped) {
            // 衝突時
            if (raycastManager.Raycast(Input.GetTouch(0).position, hitResults, TrackableType.PlaneWithinPolygon)) {
                DropObject(hitResults[0].pose.position.y);
            }
        }
    }

    // オブジェクトの設置
    void DropObject(float y) {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        fadeController.action = () => {
            Instantiate(objectPrefab, new Vector3(x, y ,z), Quaternion.identity);

            // 平面検知を停止
            planeManager.detectionMode = PlaneDetectionMode.None;

            // planeManagerを非アクティブ化
            planeManager.SetTrackablesActive(false);

            // 平面のプレハブを非アクティブ化
            planeManager.planePrefab.SetActive(false);

            isDropped = true;
        };

        // フェードアウト
        fadeController.isFadeOut = true;
    }
}
