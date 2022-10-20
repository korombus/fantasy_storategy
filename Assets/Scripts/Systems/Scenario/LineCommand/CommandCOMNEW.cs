using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCOMNEW : ILineCommand
{

    public const string LINECOMMAND = "[cr";
    private int lineIndex;
    public override void SetData( ref int lineindex) {
        lineIndex = lineindex;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI, List<string>lineText) {
        // #コマンド行を削除
        lineText.RemoveAt(lineIndex + 1);
        // コマンドを改行に入れ替え
        lineText[lineIndex] = lineText[lineIndex].Replace("[cr]", Environment.NewLine);
        // 次々行を連結
        lineText[lineIndex] += lineText[lineIndex + 1];
        // 次々行も削除
        lineText.RemoveAt(lineIndex + 1);
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
