using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    GameObject pickUpButton;
    NextController nextController;

    string targetTag; // 拾う対象のタグ

    void Start() {
        pickUpButton = GameObject.Find("PickUpButton");
        pickUpButton.SetActive(false);

        nextController = GetComponent<NextController>();
    }

    public void TogglePickUpButton(
        bool active,
        string newTargetTag) {
            pickUpButton.SetActive(active);
            if (active) {
                // ターゲットを変更
                targetTag = newTargetTag;
            }
        }
    
    public void PickUp() {
        Destroy(GameObject.FindGameObjectWithTag(targetTag));

        // nextCheck
        nextController.CheckNext("PickUp");
    }
}
