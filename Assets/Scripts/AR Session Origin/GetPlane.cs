using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class GetPlane : MonoBehaviour
{
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    ARPlaneManager planeManager;
    ARPointCloudManager pointCloudManager;

    ObjectController objectController;

    bool isSaved = false; // すでに床の高さを保存したか

    void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
    }

    void Start() {
        GameObject gameDirector = GameObject.Find("GameDirector");
        objectController = gameDirector.GetComponent<ObjectController>();
    }

    void Update() {
        // タッチ時
        if (Input.GetMouseButtonDown(0) && !isSaved) {
            // 衝突時
            if (raycastManager.Raycast(Input.GetTouch(0).position, hitResults, TrackableType.PlaneWithinPolygon)) {
                // 床の高さを保存
                objectController.planeY = hitResults[0].pose.position.y;

                // 平面検知を停止
                planeManager.detectionMode = PlaneDetectionMode.None;

                // planeManagerを非アクティブ化
                planeManager.SetTrackablesActive(false);

                // 平面のプレハブを非アクティブ化
                planeManager.planePrefab.SetActive(false);

                // 新規点群検知を停止
                pointCloudManager.enabled = false;

                // メッセージを削除
                GameObject.Find("Message").SetActive(false);

                // 全ての点群を削除
                foreach (var Point in pointCloudManager.trackables) {
                    Point.gameObject.SetActive(false);
                }

                isSaved = true;
            }
        }
    }
}
