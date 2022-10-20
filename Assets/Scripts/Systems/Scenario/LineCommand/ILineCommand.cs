using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ILineCommand
{
    /// <summary>
    /// 必要なデータをセット
    /// </summary>
    /// <param name="lineIndex"></param>
    public abstract void SetData(
        ref int lineIndex
        );
    /// <summary>
    /// コマンド起動
    /// </summary>
    /// <param name="command"></param>
    /// <param name="advUI"></param>
    /// <param name="lineText"></param>
    public abstract void RUN(string[] command, ReadScenario.ADVUI advUI, List<string> lineText);
    /// <summary>
    /// 最終的に値を返す場合はこれを使う
    /// </summary>
    /// <returns></returns>
    public abstract object END();
}
