using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCOLOR : ILineCommand
{

    public const string LINECOMMAND = "<color>";
    private int lineIndex;
    public override void SetData( ref int lineindex) {
        lineIndex = lineindex;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI, List<string>lineText) {
        string body = null;
        for (int i = 0; i < command[2].Length; i++)
        {
            // 正式なコマンドを生成して文字に割り当てる
            body += "<color=" + command[1] + ">" + command[2].Substring(i, 1) + "</color>";
        }
        // 本文にあるコマンド部分を消す
        lineText[lineIndex] = lineText[lineIndex].Replace("<@" + command[1] + ">", "");
        lineText[lineIndex] = lineText[lineIndex].Remove(lineText[lineIndex].IndexOf("</>"), 3);
        // 本文とコマンドを置換する
        lineText[lineIndex] = lineText[lineIndex].Replace(command[2], body);
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
