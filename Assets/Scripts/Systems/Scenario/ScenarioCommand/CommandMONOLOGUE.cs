using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMONOLOGUE : ICommand {

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        throw new System.NotImplementedException();
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        // 色の順序が重要なので個別に追加
        switch (command[1].ToUpper())
        {
            case "ON":
                advUI.monologuePanel.gameObject.SetActive(true);
                advUI.textPanel.gameObject.SetActive(false);
                break;

            case "OFF":
                advUI.monologuePanel.gameObject.SetActive(false);
                advUI.textPanel.gameObject.SetActive(true);
                break;
        }
        
    }

    public override object END() {
        return true;
    }
}
