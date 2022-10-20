using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDISP : ICommand
{
    private Dictionary<string, Image> charaObjList;
    private Dictionary<string, string> charaNameList;
    private List<Vector3> defaultMovePosList;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        this.charaObjList = charaObjList;
        this.charaNameList = charaNameList;
        this.defaultMovePosList = defaultMovePosList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {

        // キャラ名を空白にする
        if(command[1].ToUpper() == "EMPTY") {
            command[1] = "";
        }

        // キャラ名
        advUI.charaNameText.text = command[1];

        // logに名前を追加
        advUI.logText.text += Environment.NewLine + command[1] + Environment.NewLine;

        // コマンドの長さによって処理を変更
        if (command.Length >= 3) {
            // 画像を取得
            Sprite charaImg = Resources.Load<Sprite>(ReadScenario.CHARACTER_TEXTURE_PATH + command[2]);

            // 画像表示
            if (charaImg != null) {
                // 画像の配置位置は、RかLかを必ず指定してもらう。
                if (command.Length >= 4) {
                    string posStr = command[3].Substring(0, 1).ToUpper();
                    charaNameList[posStr] = command[1];
                    charaObjList[posStr].gameObject.SetActive(true);
                    charaObjList[posStr].sprite = charaImg;
                    charaObjList[posStr].transform.localPosition = defaultMovePosList[posStr == "L" ? 2 : posStr == "C" ? 1 : 0];

                    // 大きさを画像の大きさの半分に変更
                    //Debug.Log(charaImg.textureRect);
                    //charaObjList[posStr].rectTransform.sizeDelta = new Vector2(charaImg.textureRect.width/2, charaImg.textureRect.height/2);
                }
            }
        }
        //キャラクター名のみの場合
        else if (command.Length == 2) {
            var dispNameList = charaNameList.OrderBy(x => x.Value);
            //キャラクター名リストからオブジェクト名キーを取得して、対象オブジェクトに処理を実行させる。
            foreach (KeyValuePair<string, string> item in dispNameList) {
                float alphaValue = charaObjList[item.Key].color.a;
                charaObjList[item.Key].color = (item.Value == command[1]) ? new Color(1f, 1f, 1f, alphaValue) : new Color(0.5f, 0.5f, 0.5f, alphaValue);
            }
        }
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
