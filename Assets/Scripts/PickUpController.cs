using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    GameObject pickUpButton;
    GameObject pickUpButtonText;
    NextController nextController;

    string targetTag; // 拾う対象のタグ

    void Start() {
        pickUpButton = GameObject.Find("PickUpButton");
        pickUpButton.SetActive(false);
        pickUpButtonText = pickUpButton.transform.Find("PickUpButtonText").gameObject;

        nextController = GetComponent<NextController>();
    }

    public void TogglePickUpButton(
        bool active,
        string newText,
        string newTargetTag) {
            pickUpButton.SetActive(active);
            if (active) {
                // ボタンのテキストを変更
                pickUpButtonText.GetComponent<Text>().text = newText;
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
