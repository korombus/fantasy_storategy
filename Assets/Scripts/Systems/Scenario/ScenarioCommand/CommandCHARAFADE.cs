using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCHARAFADE : ICommand {

    private List<Color> fadeColorList;
    private Dictionary<string, Image> charaObjList;
    private Dictionary<string, string> charaNameList;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
        this.fadeColorList = fadeColorList;
        this.charaObjList = charaObjList;
        this.charaNameList = charaNameList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }

    public void RUN(string[] command, ReadScenario.ADVUI advUI, ref float[] waitTime) {
        //秒数は初期値１秒
        if (!float.TryParse(command[3], out waitTime[0])) {
            waitTime[0] = 1;
        }

        // コマンドに立ち絵オブジェクトが含まれていれば座標設定
        string charaFadeSet = command[2];

        var fadeNameList = charaNameList.OrderBy(x => x.Value);

        //キャラクター名リストからオブジェクト名キーを取得して、対象オブジェクトに処理を実行させる。
        foreach (KeyValuePair<string, string> item in fadeNameList) {
            if (item.Value == charaFadeSet) advUI.moveObj = charaObjList[item.Key];
        }

        // 色の順序が重要なので個別に追加
        fadeColorList.Add((command[1].ToUpper() == "IN") ? advUI.moveObj.color : Color.clear);
        fadeColorList.Add((command[1].ToUpper() == "IN") ? Color.clear : advUI.moveObj.color);
    }

    public override object END() {
        return StateReadScenario.WAIT;
    }
}
