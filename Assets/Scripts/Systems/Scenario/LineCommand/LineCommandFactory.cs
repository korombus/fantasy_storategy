using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCommandFactory {

    // コマンドのクラスは使い回す
    private static Dictionary<string, ILineCommand> lineCommandTable = new Dictionary<string, ILineCommand>();

    /// <summary>
    /// 対象となるコマンドを取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
	public static ILineCommand GetCommand(string lineCom) {
        switch (lineCom) {
            case CommandSTOP.LINECOMMAND:
                if (!lineCommandTable.ContainsKey(CommandSTOP.LINECOMMAND)) {
                    lineCommandTable.Add(CommandSTOP.LINECOMMAND, new CommandSTOP());
                }
                return lineCommandTable[CommandSTOP.LINECOMMAND];

            case CommandNEW.LINECOMMAND:
                if (!lineCommandTable.ContainsKey(CommandNEW.LINECOMMAND)) {
                    lineCommandTable.Add(CommandNEW.LINECOMMAND, new CommandNEW());
                }
                return lineCommandTable[CommandNEW.LINECOMMAND];

            case CommandCOMNEW.LINECOMMAND:
                if (!lineCommandTable.ContainsKey(CommandCOMNEW.LINECOMMAND)) {
                    lineCommandTable.Add(CommandCOMNEW.LINECOMMAND, new CommandCOMNEW());
                }
                return lineCommandTable[CommandCOMNEW.LINECOMMAND];

            case CommandCOLOR.LINECOMMAND:
                if (!lineCommandTable.ContainsKey(CommandCOLOR.LINECOMMAND)) {
                    lineCommandTable.Add(CommandCOLOR.LINECOMMAND, new CommandCOLOR());
                }
                return lineCommandTable[CommandCOLOR.LINECOMMAND];

            case CommandNEXTWORD.LINECOMMAND:
                if (!lineCommandTable.ContainsKey(CommandNEXTWORD.LINECOMMAND)) {
                    lineCommandTable.Add(CommandNEXTWORD.LINECOMMAND, new CommandNEXTWORD());
                }
                return lineCommandTable[CommandNEXTWORD.LINECOMMAND];
        }
        return null;
    }
}
