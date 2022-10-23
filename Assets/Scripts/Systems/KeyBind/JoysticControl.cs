using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysticControl
{
    /// <summary>
    /// 入力されたジョイスティックのボタンタイプを取得
    /// </summary>
    /// <returns></returns>
    public static KeyCode GetInputJoysticButtonType(){
        if(Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick2Button0) || Input.GetKey(KeyCode.JoystickButton0)){
            return KeyCode.JoystickButton0;
        }

        if(Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick2Button1) || Input.GetKey(KeyCode.JoystickButton1)){
            return KeyCode.JoystickButton1;
        }

        if(Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick2Button2) || Input.GetKey(KeyCode.JoystickButton2)){
            return KeyCode.JoystickButton2;
        }

        if(Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Joystick2Button3) || Input.GetKey(KeyCode.JoystickButton3)){
            return KeyCode.JoystickButton3;
        }

        if(Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick2Button4) || Input.GetKey(KeyCode.JoystickButton4)){
            return KeyCode.JoystickButton4;
        }

        if(Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.Joystick2Button5) || Input.GetKey(KeyCode.JoystickButton5)){
            return KeyCode.JoystickButton5;
        }

        if(Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick2Button6) || Input.GetKey(KeyCode.JoystickButton6)){
            return KeyCode.JoystickButton6;
        }

        if(Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick2Button7) || Input.GetKey(KeyCode.JoystickButton7)){
            return KeyCode.JoystickButton7;
        }

        if(Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.Joystick2Button8) || Input.GetKey(KeyCode.JoystickButton8)){
            return KeyCode.JoystickButton8;
        }

        if(Input.GetKey(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Joystick2Button9) || Input.GetKey(KeyCode.JoystickButton9)){
            return KeyCode.JoystickButton9;
        }

        if(Input.GetKey(KeyCode.Joystick1Button10) || Input.GetKey(KeyCode.Joystick2Button10) || Input.GetKey(KeyCode.JoystickButton10)){
            return KeyCode.JoystickButton10;
        }

        return KeyCode.JoystickButton0;
    }
}
