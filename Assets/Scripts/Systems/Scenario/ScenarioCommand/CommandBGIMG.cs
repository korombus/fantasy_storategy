using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBGIMG : ICommand {

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> movePosList, ref List<Color> fadeColorList, List<Vector3> defaultMovePosList) {
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        if (command.Length == 1) {
            advUI.bgImage.ForEach(n => n.gameObject.SetActive(false));
            advUI.bgImage.ForEach(n => n.color = Color.black);
        } else if(command.Length == 2){
            int tryInt = 0;
            if (int.TryParse(command[1], out tryInt)) {
                advUI.bgImage[tryInt].gameObject.SetActive(false);
                advUI.bgImage[tryInt].color = Color.black;
                ;
            } else {
                advUI.bgImage[0].gameObject.SetActive(true);
                advUI.bgImage[0].color = Color.white;
                advUI.bgImage[0].texture = Resources.Load<Texture>(ReadScenario.BACKGROUND_TEXTURE_PATH + command[1]);
            }
        } else if(command.Length == 3){
            int tryInt = 0;
            if (int.TryParse(command[2], out tryInt)) {
                advUI.bgImage[tryInt].gameObject.SetActive(true);
                advUI.bgImage[tryInt].color = Color.white;
                advUI.bgImage[tryInt].texture = Resources.Load<Texture>(ReadScenario.BACKGROUND_TEXTURE_PATH + command[1]);
            }
        }
    }

    public override object END() {
        throw new System.NotImplementedException();
    }
}
