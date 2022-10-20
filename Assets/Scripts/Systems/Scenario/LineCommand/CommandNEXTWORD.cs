using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommandNEXTWORD : ILineCommand
{

    public const string LINECOMMAND = "[$&";
    private int lineIndex;
    public override void SetData( ref int lineindex) {
        lineIndex = lineindex;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI, List<string>lineText) {
        string nextLine = lineText[lineIndex].Replace(advUI.frameText.text + LINECOMMAND, "");
        string str = nextLine.Substring(0, 1);
        // 先頭に改行文字が入っている場合は削除
        if (Regex.IsMatch(str, "\r") || Regex.IsMatch(str, "\n") || Regex.IsMatch(str, "\r\n"))
        {
            nextLine = nextLine.Remove(0, 1);
        }
        lineText.Insert(lineIndex + 1, nextLine);
        lineText[lineIndex] = advUI.frameText.text;
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
