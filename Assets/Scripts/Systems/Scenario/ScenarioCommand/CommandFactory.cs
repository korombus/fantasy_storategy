using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFactory {

    // クラス使い回す
    private static Dictionary<string, ICommand> commandSharpTable = new Dictionary<string, ICommand>();

    /// <summary>
    /// 対象となるコマンドを取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
	public static ICommand GetCommand(ScenarioSharpCommand type) {
        switch (type) {
            case ScenarioSharpCommand.DISP:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.DISP.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.DISP.ToString(), new CommandDISP());
                }
                return commandSharpTable[ScenarioSharpCommand.DISP.ToString()];

            case ScenarioSharpCommand.DISPCLEAR:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.DISPCLEAR.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.DISPCLEAR.ToString(), new CommandDISPCLEAR());
                }
                return commandSharpTable[ScenarioSharpCommand.DISPCLEAR.ToString()];

            case ScenarioSharpCommand.BGIMG:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.BGIMG.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.BGIMG.ToString(), new CommandBGIMG());
                }
                return commandSharpTable[ScenarioSharpCommand.BGIMG.ToString()];

            case ScenarioSharpCommand.FADE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.FADE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.FADE.ToString(), new CommandFADE());
                }
                return commandSharpTable[ScenarioSharpCommand.FADE.ToString()];

            case ScenarioSharpCommand.CHARAFADE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.CHARAFADE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.CHARAFADE.ToString(), new CommandCHARAFADE());
                }
                return commandSharpTable[ScenarioSharpCommand.CHARAFADE.ToString()];

            case ScenarioSharpCommand.SHAKE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.SHAKE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.SHAKE.ToString(), new CommandSHAKE());
                }
                return commandSharpTable[ScenarioSharpCommand.SHAKE.ToString()];

            case ScenarioSharpCommand.CHARASHAKE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.CHARASHAKE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.CHARASHAKE.ToString(), new CommandCHARASHAKE());
                }
                return commandSharpTable[ScenarioSharpCommand.CHARASHAKE.ToString()];

            case ScenarioSharpCommand.MONOLOGUE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.MONOLOGUE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.MONOLOGUE.ToString(), new CommandMONOLOGUE());
                }
                return commandSharpTable[ScenarioSharpCommand.MONOLOGUE.ToString()];

            case ScenarioSharpCommand.MOVE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.MOVE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.MOVE.ToString(), new CommandMOVE());
                }
                return commandSharpTable[ScenarioSharpCommand.MOVE.ToString()];

            case ScenarioSharpCommand.NEXT:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.NEXT.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.NEXT.ToString(), new CommandNEXT());
                }
                return commandSharpTable[ScenarioSharpCommand.NEXT.ToString()];

            case ScenarioSharpCommand.WAIT:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.WAIT.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.WAIT.ToString(), new CommandWAIT());
                }
                return commandSharpTable[ScenarioSharpCommand.WAIT.ToString()];

            case ScenarioSharpCommand.BGM:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.BGM.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.BGM.ToString(), new CommandBGM());
                }
                return commandSharpTable[ScenarioSharpCommand.BGM.ToString()];

            case ScenarioSharpCommand.SE:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.SE.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.SE.ToString(), new CommandSE());
                }
                return commandSharpTable[ScenarioSharpCommand.SE.ToString()];

            case ScenarioSharpCommand.FRONTBGCOLOR:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.FRONTBGCOLOR.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.FRONTBGCOLOR.ToString(), new CommandFRONTBGCOLOR());
                }
                return commandSharpTable[ScenarioSharpCommand.FRONTBGCOLOR.ToString()];

            case ScenarioSharpCommand.BGZOOM:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.BGZOOM.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.BGZOOM.ToString(), new CommandBGZOOM());
                }
                return commandSharpTable[ScenarioSharpCommand.BGZOOM.ToString()];

            case ScenarioSharpCommand.MSG:
                if (!commandSharpTable.ContainsKey(ScenarioSharpCommand.MSG.ToString())) {
                    commandSharpTable.Add(ScenarioSharpCommand.MSG.ToString(), new CommandMSGWIN());
                }
                return commandSharpTable[ScenarioSharpCommand.MSG.ToString()];
        }

        return null;
    }
}
