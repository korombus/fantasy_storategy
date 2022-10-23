using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyConfig : MonoBehaviour
{
    public KeyBindOption parent;    //!< 親クラス
    public TextMeshProUGUI commandText;  //!< コマンド表示用ボタン
    public TextMeshProUGUI configButton; //!< 設定表示用ボタン

    public void SetData(string commandName, string bindKeyName){
        commandText.text = commandName;
        configButton.text = bindKeyName;

        // Clickイベントを設置
        CommonUtil.SearchObjectChild("Button", this.transform).GetComponent<Button>().onClick.AddListener(OnClickKeySetting);
    }

    public void OnClickKeySetting(){
        if(parent.parent.isKeyBinding){
            configButton.text = parent.parent.setterKey;
            parent.parent.isKeyBinding = false;
        } else {
            parent.parent.isKeyBinding = true;
            configButton.text = "キー入力待機中";
        }
    }
}
