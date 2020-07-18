using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterRay : MonoBehaviour
{
    DevLog devLog;
    PickUpController pickUpController;
    NextController nextController;

    public bool showPickUpButton = true;

    GameObject findGauge; // 円ゲージ
    float gauge = 0; // ゲージの値
    bool showGauge = true;

    string state = "";
    Vector3 center = new Vector3(Screen.width/2, Screen.height/2);
    RaycastHit hit;
    int distance = 10;

    void Start() {
        findGauge = GameObject.Find("FindGauge");
        devLog = GetComponent<DevLog>();
        pickUpController = GetComponent<PickUpController>();
        nextController = GetComponent<NextController>();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(center);

        if (Physics.Raycast(ray,out hit,distance)) {
            // オブジェクトと当たった場合
            string tag = hit.collider.tag;

            // 状態が変わったとき
            if (showGauge && gauge <= 1 && state != tag && tag != "Untagged") {
                IncreaseFind(tag);
            }
        } else {
            // オブジェクトと当たらなかったとき
            if (state != "") {
                state = "";
                devLog.SendLog("オブジェクトを見失いました");
                pickUpController.TogglePickUpButton(
                    active: false,
                    newText: "",
                    newTargetTag: "");
            }

            // ゲージをゼロに
            if (gauge != 0) {
                gauge = 0;
                findGauge.GetComponent<Image>().fillAmount = 0;
            }
        }
    }

    public void ResetGauge() {
        state = "";
        gauge = 0;
        findGauge.GetComponent<Image>().fillAmount = 0;
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }

    void IncreaseFind(string tag) {
        float inclease = Time.deltaTime * 0.5f;
        gauge += inclease;
        if (gauge <= 1) {
            findGauge.GetComponent<Image>().fillAmount += inclease;
        } else {
            findGauge.GetComponent<Image>().fillAmount = 1;

            state = tag;
            devLog.SendLog($"{tag}を見つけました");

            if (showPickUpButton)
                pickUpController.TogglePickUpButton(
                    active: true,
                    newText: $"{tag}を拾う",
                    newTargetTag: tag);
            
            // nextCheck
            nextController.CheckNext("Find");
        }
    }

    public void ToggleShowGauge() {
        showGauge = !showGauge;
        findGauge.SetActive(showGauge);
        UnityMessageManager.Instance.SendMessageToFlutter("next");
    }
}
