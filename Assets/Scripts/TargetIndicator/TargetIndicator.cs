using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TargetIndicator: MonoBehaviour
{
    public Transform target = default;

    [SerializeField]
    private Image arrow = default;

    private Camera mainCamera;
    private RectTransform rectTransform;

    private void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        float canvasScale = transform.root.localScale.z;
        var center = 0.5f * new Vector3(Screen.width, Screen.height);
        
        // （画面中心を原点(0,0)とした）ターゲットのスクリーン座標を求める
        var pos = mainCamera.WorldToScreenPoint(target.position) - center;
        // カメラ後方にあるターゲットのスクリーン座標は、画面中心に対する点対称の座標にする
        if (pos.z < 0f)
        {
            pos.x = -pos.x;
            pos.y = -pos.y;

            // カメラと水平なターゲットのスクリーン座標を補正する
            if (Mathf.Approximately(pos.y, 0f))
            {
                pos.y = -center.y;
            }
        }

        var halfSize = 0.5f * canvasScale * rectTransform.sizeDelta;
        float d = Mathf.Max(
            Mathf.Abs(pos.x / (center.x - halfSize.x)),
            Mathf.Abs(pos.y / (center.y - halfSize.y))
        );

        // ターゲットのスクリーン座標が画面外なら、画面端になるように調整する
        bool isOffscreen = (pos.z < 0f || d > 1f);

        // ターゲットのスクリーン座標が画面外なら、ターゲットの方向を指す矢印を表示する
        arrow.enabled = isOffscreen;
        if (isOffscreen)
        {
            pos.x /= d;
            pos.y /= d;

            arrow.rectTransform.eulerAngles = new Vector3(
                0f, 0f,
                Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg
            );
        }
        rectTransform.anchoredPosition = pos / canvasScale;
    }
}