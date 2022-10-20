using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMOVE : ICommand {

    private Dictionary<string, Image> charaObjList;
    private Dictionary<string, string> charaNameList;
    private List<Vector3> movePosList;

    private bool moveModal;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        this.charaObjList = charaObjList;
        this.charaNameList = charaNameList;
        this.movePosList = movePosList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public bool RUN(string[] command, ReadScenario.ADVUI advUI, ref float[] waitTime) {
        movePosList.Clear();                                    //移動座標初期化
        if (command.Length <= 3) return false;                  //パラメータ有り無し

        // コマンドに立ち絵オブジェクトが含まれていれば座標設定
        string charaSet = command[1];
        var moveNameList = charaNameList.OrderBy(x => x.Value);

        //キャラクター名リストからオブジェクト名キーを取得して、対象オブジェクトに処理を実行させる。
        foreach (KeyValuePair<string, string> item in moveNameList) {
            if (item.Value == charaSet) {
                advUI.moveObj = charaObjList[item.Key];
            }
        }
        //キャラクターがいなければ処理を抜ける。
        if (advUI.moveObj == null) return false;

        movePosList.Add(advUI.moveObj.transform.localPosition);       //移動前

        string moveTime = command.Length > 4 ? command[4] : "1";//移動時間
        int firstMovePrameter = int.Parse(command[3]);          //移動量（F指定の場合X座標）の変数

        switch (((command[2].ToUpper().StartsWith("M")) ? command[2].Remove(0, 1).Substring(0, 1).ToUpper() : command[2]).Substring(0, 1).ToUpper()) {
            case "U":   //上移動（相対座標）
                movePosList.Add(new Vector3(movePosList[0].x, movePosList[0].y + firstMovePrameter, movePosList[0].z));  //移動後
                break;

            case "D":   //下移動（相対座標）
                movePosList.Add(new Vector3(movePosList[0].x, movePosList[0].y - firstMovePrameter, movePosList[0].z));  //移動後
                break;

            case "R":   //右移動（相対座標）
                movePosList.Add(new Vector3(movePosList[0].x + firstMovePrameter, movePosList[0].y, movePosList[0].z));  //移動後
                break;

            case "L":   //左移動（相対座標）
                movePosList.Add(new Vector3(movePosList[0].x - firstMovePrameter, movePosList[0].y, movePosList[0].z));  //移動後
                break;

            case "F":   //自由移動（絶対座標）
                movePosList.Add(new Vector3(firstMovePrameter, float.Parse(command[4]), movePosList[0].z));  //移動後
                if (command.Length > 5) moveTime = command[5];  //移動時間取得
                break;

            default:
                movePosList.Add(movePosList[0]);
                break;
        }

        if (true == command[2].ToUpper().StartsWith("M")) {
            //移動時間設定（デフォルト1秒）
            waitTime[0] = float.Parse(moveTime);
            //モーダル/モードレス判定、コマンド実行
            moveModal = true;
        } else {
            //移動時間設定（デフォルト1秒）
            waitTime[1] = float.Parse(moveTime);
            //モーダル/モードレス判定、コマンド実行
            moveModal = false;
        }

        return moveModal;
    }

    public override object END() {
        return moveModal ? StateReadScenario.WAIT : StateReadScenario.READ;
    }
}
