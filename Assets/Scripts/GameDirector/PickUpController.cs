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
        // CH_のついたチルドレンオブジェクトの場合親を削除
        if (targetTag.Substring(0, 3) == "CH_") targetTag = targetTag.Substring(3);

        Destroy(GameObject.FindGameObjectWithTag(targetTag));
        Destroy(GameObject.FindGameObjectWithTag("TargetIndicator"));

        // nextCheck
        nextController.CheckNext("PickUp");
    }
}
