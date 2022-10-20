using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSHAKE : ICommand {
    private bool shakeModal;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> shakePosList, ref List<Color> fadeColorList, List<Vector3> defaultshakePosList) {
        throw new System.NotImplementedException();
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public bool RUN(string[] command, ReadScenario.ADVUI advUI, ref float[] waitTime, ref float shakeSpan, ref float shakeLength, ref bool verticalEn)
    {
        if (command.Length <= 2) return false;                  //パラメータ有り無し

        shakeSpan = command.Length > 2 ? float.Parse(command[2]) : 1.0f;//振動間隔
        shakeLength = command.Length > 3 ? float.Parse(command[3]) : 10.0f;//振動間隔
        string shakeTime = command.Length > 4 ? command[4] : "1";//移動時間

        switch (((command[1].ToUpper().StartsWith("M")) ? command[1].Remove(0, 1).Substring(0, 1).ToUpper() : command[1]).Substring(0, 1).ToUpper())
        {
            case "X":   //上移動（相対座標）
                verticalEn = false;
                break;

            default:
                verticalEn = true;
                break;

        }

        if (true == command[1].ToUpper().StartsWith("M"))
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

        return shakeModal;
    }

    public override object END() {
        return shakeModal ? StateReadScenario.WAIT : StateReadScenario.READ;
    }
}
