using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmPanel : MonoBehaviour
{
    public TextMeshProUGUI confirmText;
    public Button YesButton;
    public Button NoButton;

    public void SetData(string message, Action<string> yesButtonEvent, Action<string> noButtonEvent){
        confirmText.text = message;
        YesButton.onClick.AddListener(delegate { yesButtonEvent("YES"); });
        NoButton.onClick.AddListener(delegate { noButtonEvent("NO"); });

        // 位置ずれが起こるので補正
        this.GetComponent<RectTransform>().localPosition = Vector3.zero;
        this.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }
}
