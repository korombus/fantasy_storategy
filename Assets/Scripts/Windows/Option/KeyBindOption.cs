using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindOption : MonoBehaviour
{
    public OptionWindow parent; //!< 親クラス
    public List<KeyConfig> KeyBindSetterList = new List<KeyConfig>();   //!< キー配置設定用のコンポーネント群

    public void SetData(Dictionary<string, KeyCode> keyBindSettings){
        // キー配置できる数だけ設定用のコンポーネントを持ったオブジェクトを起こす
        int wakeUpObjectNum = keyBindSettings.Count;
        
        foreach( var keySetter in KeyBindSetterList.Select((value, index) => new {Value = value, Index = index})){
            // 設定可能であればオブジェクトを起こす
            if(keySetter.Index < keyBindSettings.Count){
                keySetter.Value.SetData(keyBindSettings.ElementAt(keySetter.Index).Key, keyBindSettings.ElementAt(keySetter.Index).Value.ToString());
                keySetter.Value.gameObject.SetActive(true);
            } 
            // 設定不可であればオブジェクトを非表示
            else {
                keySetter.Value.gameObject.SetActive(false);
            }
        }
    }
}
