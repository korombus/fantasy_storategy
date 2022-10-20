using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSE : ICommand {
    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
    }

    public override object END() {
        return null;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) { }

    public void RUN(string[] command, CommonSound seObject) {
        // コマンドで強制フェードされている場合は以降bgmは流さない
        seObject.Play(command[1], true);
    }
}
