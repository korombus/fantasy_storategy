using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSTOP : ILineCommand
{
    public const string LINECOMMAND = "[l]";

    private int lineIndex;
    public override void SetData( ref int lineindex) {
        lineIndex = lineindex;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI, List<string>lineText) {
        // 一時停止コマンドを文字から削除
        lineText[lineIndex] = lineText[lineIndex].Replace("[l]", "");
    }

    public override object END() {
        return StateReadScenario.STOP;
    }
}
