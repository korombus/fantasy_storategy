using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ICommand {
    // 必要なデータをセット
    public abstract void SetData(
        ref Dictionary<string, Image> charaObjList,
        ref Dictionary<string, string> charaNameList,
        ref List<Vector3> movePosList,
        ref List<Color> fadeColorList,
        List<Vector3> defaultMovePosList
    );
    // コマンド起動
    public abstract void RUN(string[] command, ReadScenario.ADVUI advUI);
    // 最終的に値を返す場合はこれを使う
    public abstract object END();
}
