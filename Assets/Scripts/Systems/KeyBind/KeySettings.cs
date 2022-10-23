using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySettings
{
    public static readonly Dictionary<string, KeyCode> KEY_MAP = new Dictionary<string, KeyCode>(){
        { "OK", KeyCode.JoystickButton0 },
        { "Cancel", KeyCode.JoystickButton1 },
        { "Menu", KeyCode.JoystickButton2 }
    };
}
