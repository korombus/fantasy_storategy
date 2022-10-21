using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioOption : MonoBehaviour
{
    public OptionWindow parent; //!< 親クラス
    public Button highSpeedButton;      //!< 高速文字送りボタン
    public Button normalSpeedButton;    //!< 通常文字送りボタン
    public Button lowSpeedButton;       //!< 低速文字送り

    public float highSpeedValue = 0.6f;     //!< 最速送り値
    public float normalSpeedValue = 1.2f;   //!< 通常送り値
    public float lowSpeedValue = 2.0f;      //!< 低速送り値

    public enum TEXT_SPEED_TYPE{
        HIGH,
        NORMAL,
        LOW
    }

    public void SetData(float speed){
        // 最速設定の場合は最速ボタンを選択状態にする
        if(speed <= highSpeedValue){
            ChangeButtonNormalColor(highSpeedButton, Color.red);
        } else 
        if(speed == normalSpeedValue){
            ChangeButtonNormalColor(normalSpeedButton, Color.red);
        } else 
        if(speed >= lowSpeedValue){
            ChangeButtonNormalColor(lowSpeedButton, Color.red);
        }
    }

    public void OnClickSpeedButton(string type){
        // 押されたボタンを選択状態へ
        ChangeSelectButton(CommonUtil.ParseEnum<TEXT_SPEED_TYPE>(type.ToUpper()));
    }

    public void ChangeSelectButton(TEXT_SPEED_TYPE type){
        switch(type){
            case TEXT_SPEED_TYPE.HIGH:
                ChangeButtonNormalColor(highSpeedButton, Color.red);
                ChangeButtonNormalColor(normalSpeedButton, Color.white);
                ChangeButtonNormalColor(lowSpeedButton, Color.white);

                // 押されたボタンの設定を反映
                parent.optionOrigin.option.SetTextSpeed(highSpeedValue);
            break;
            case TEXT_SPEED_TYPE.NORMAL:
                ChangeButtonNormalColor(highSpeedButton, Color.white);
                ChangeButtonNormalColor(normalSpeedButton, Color.red);
                ChangeButtonNormalColor(lowSpeedButton, Color.white);

                // 押されたボタンの設定を反映
               parent.optionOrigin.option.SetTextSpeed(normalSpeedValue);
            break;
            case TEXT_SPEED_TYPE.LOW:
                ChangeButtonNormalColor(highSpeedButton, Color.white);
                ChangeButtonNormalColor(normalSpeedButton, Color.white);
                ChangeButtonNormalColor(lowSpeedButton, Color.red);

                // 押されたボタンの設定を反映
                parent.optionOrigin.option.SetTextSpeed(lowSpeedValue);
            break;
        }
    }

    public void ChangeButtonNormalColor(Button button, Color color){
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
