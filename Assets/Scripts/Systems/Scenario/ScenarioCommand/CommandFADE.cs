using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandFADE : ICommand {

    private List<Color> fadeColorList;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        this.fadeColorList = fadeColorList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public void RUN(string[] command, ref float[] waitTime) {
        if (float.TryParse(command[2], out waitTime[0])) {

            // コマンドに色が含まれていればフェード色設定
            Color color = Color.black;
            if (command.Length > 3) {
                ColorUtility.TryParseHtmlString(command[3], out color);
            }

            // 色の順序が重要なので個別に追加
            fadeColorList.Add((command[1].ToUpper() == "IN") ? color : Color.clear);
            fadeColorList.Add((command[1].ToUpper() == "IN") ? Color.clear : color);
        }
    }

    public override object END() {
        return StateReadScenario.WAIT;
    }
}
