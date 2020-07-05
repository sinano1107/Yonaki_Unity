using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class CreateObject : MonoBehaviour
{
    [SerializeField]
    GameObject objectPrefab;

    ARRaycastManager raycastManager;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update() {
        // タッチ時
        if (Input.GetMouseButtonDown(0)) {
            // 衝突時
            if (raycastManager.Raycast(Input.GetTouch(0).position, hitResults, TrackableType.PlaneWithinPolygon)) {
                // オブジェクトの生成
                Instantiate(objectPrefab, hitResults[0].pose.position, Quaternion.identity);
            }
        }
    }
}
