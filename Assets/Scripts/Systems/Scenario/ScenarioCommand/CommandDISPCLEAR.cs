using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDISPCLEAR : ICommand {
    private Dictionary<string, Image> charaObjList;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        this.charaObjList = charaObjList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        if (command.Length >= 2) {
            advUI.charaNameText.text = command[1];
        } else {
            advUI.charaNameText.text = "";
        }
        charaObjList["L"].gameObject.SetActive(false);
        charaObjList["C"].gameObject.SetActive(false);
        charaObjList["R"].gameObject.SetActive(false);
        charaObjList["R"].color = charaObjList["C"].color = charaObjList["L"].color = new Color(1f, 1f, 1f, 1f);
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
