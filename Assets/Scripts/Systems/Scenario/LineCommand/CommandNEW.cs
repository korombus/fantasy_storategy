using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommandNEW : ILineCommand
{

    public const string LINECOMMAND = "[r]";
    private int lineIndex;
    public override void SetData( ref int lineindex) {
        lineIndex = lineindex;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI, List<string>lineText) {
        // 一時停止コマンドを文字から削除
        Regex re = new Regex("\\[r\\]");
        if (lineText[lineIndex].EndsWith(command[0]))
        {
            lineText[lineIndex] = re.Replace(lineText[lineIndex], Environment.NewLine, 1) + lineText[lineIndex + 1];
            // 次の行を削除
            lineText.RemoveAt(lineIndex + 1);
        }
        else
        {
            lineText[lineIndex] = re.Replace(lineText[lineIndex], Environment.NewLine, 1);
        }
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
