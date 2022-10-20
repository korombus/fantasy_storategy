using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCHARASHAKE : ICommand
{

    private Dictionary<string, Image> charaObjList;
    private Dictionary<string, string> charaNameList;

    private bool shakeModal;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> shakePosList, ref List<Color> fadeColorList, List<Vector3> defaultshakePosList) {
        this.charaNameList = charaNameList;
        this.charaObjList = charaObjList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public bool RUN(string[] command, ReadScenario.ADVUI advUI, ref float[] waitTime, ref float shakeSpan, ref float shakeLength, ref bool verticalEn, ref List<Vector3> shakePosList) {
        shakePosList.Clear();                                       //移動座標初期化
        if (command.Length <= 1) return false;                      //パラメータ有り無し

        // コマンドに立ち絵オブジェクトが含まれていれば座標設定
        string charaSet = command[1];
        var shakeNameList = charaNameList.OrderBy(x => x.Value);

        //キャラクター名リストからオブジェクト名キーを取得して、対象オブジェクトに処理を実行させる。
        foreach (KeyValuePair<string, string> item in shakeNameList) {
            if (item.Value == charaSet) {
                advUI.shakeObj = charaObjList[item.Key];
            }
        }
        shakePosList.Add(advUI.shakeObj.transform.localPosition);           //移動前位置
        shakeSpan = command.Length > 3 ? float.Parse(command[3]) : 1.0f;    //振動間隔
        shakeLength = command.Length > 4 ? float.Parse(command[4]) : 10.0f; //振動距離
        string shakeTime = command.Length > 5 ? command[5] : "1";           //移動時間
        if (command.Length > 2)
        {
            switch (((command[2].ToUpper().StartsWith("M")) ? command[2].Remove(0, 1).Substring(0, 1).ToUpper() : command[2]).Substring(0, 1).ToUpper())
            {
                case "X":   //横振動
                    verticalEn = false;
                    break;

                default:    //縦振動
                    verticalEn = true;
                    break;
            }
            if (command[2].ToUpper().StartsWith("M"))
            {
                //移動時間設定（デフォルト1秒）
                waitTime[0] = float.Parse(shakeTime);
                //モーダル/モードレス判定、コマンド実行
                shakeModal = true;
            }
            else
            {
                //移動時間設定（デフォルト1秒）
                waitTime[1] = float.Parse(shakeTime);
                //モーダル/モードレス判定、コマンド実行
                shakeModal = false;
            }
        }
        else
        {
            verticalEn = true;
            //移動時間設定（デフォルト1秒）
            waitTime[1] = float.Parse(shakeTime);
            //モーダル/モードレス判定、コマンド実行
            shakeModal = false;
        }

        
        return shakeModal;
    }

    public override object END() {
        return shakeModal ? StateReadScenario.WAIT : StateReadScenario.READ;
    }
}
