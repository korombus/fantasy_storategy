using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandFRONTBGCOLOR : ICommand {


    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        // コマンドに色が含まれていればフェード色設定
        Color color = new Color(1f, 1f, 1f, 0.5f);
        if (command.Length > 2)
        {
            ColorUtility.TryParseHtmlString(command[2], out color);
        }

        // 色の順序が重要なので個別に追加
        switch(command[1].ToUpper())
        {
            case "ON":
                advUI.fadePanel.color = color;
                break;

            case "OFF":
                advUI.fadePanel.color = Color.clear;
                break;
        }
    }

    public void RUN(string[] command, ref float[] waitTime) {
        throw new System.NotImplementedException();
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
