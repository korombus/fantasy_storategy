using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandWAIT : ICommand {

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        throw new System.NotImplementedException();
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public void RUN(string[] command, ref float[] waitTime) {
        if (!float.TryParse(command[1], out waitTime[0])) {
            waitTime[0] = 0f;
        }
    }

    public override object END() {
        return StateReadScenario.WAIT;
    }
}
