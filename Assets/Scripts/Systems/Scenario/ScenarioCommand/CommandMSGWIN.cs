using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMSGWIN : ICommand {

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList)
    {
        throw new System.NotImplementedException();
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI)
    {
        advUI.textPanel.gameObject.SetActive(command[1].ToUpper() == "ON");
    }

    public override object END()
    {
        throw new System.NotImplementedException();
    }
}
