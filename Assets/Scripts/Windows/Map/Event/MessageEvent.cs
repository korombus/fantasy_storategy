using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageEvent : MonoBehaviour
{
    [TextArea(3, 10)]
    public string eventMessageOpening = "";

    [TextArea(3, 10)]
    public string eventMessage;

    /// <summary>
    /// イベントメッセージ取得
    /// </summary>
    /// <returns>コマンドを含めたイベントメッセージ</returns>
    public string GetEventMessage(){
        string message = "#dispclear" + System.Environment.NewLine;
        message += eventMessageOpening + System.Environment.NewLine;
        message += eventMessage + System.Environment.NewLine;
        return message;
    }
}
