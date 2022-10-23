using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBase
{
    public enum Sound
    {
        BGM
       , SE
    }

    private static float _BGMVolume = 0.1f;    //!< BGM音量
    private static float _SEVolume  = 0.1f;    //!< SE音量
    private static float _TextSpeed = 0.1f;   //!< 文字表示速度
    private static Dictionary<string, KeyCode> _KeyBind = KeySettings.KEY_MAP;  //!< キー配置

    public static float BGMVolume { 
        protected set{ 
            if(value <= 1 && value >= 0){
                _BGMVolume = value;
            }
        } 
        get { return _BGMVolume; } 
    }
    public static float SEVolume { 
        protected set{
            if(value <= 1 && value >= 0){
                _SEVolume = value;
            }
        }
        get { return _SEVolume; }
    }
    public static float TextSpeed { 
        protected set{
            if(value <= 5 && value > 0){
                _TextSpeed = value;
            }
        }
        get { return _TextSpeed; }
    }
    public static Dictionary<string, KeyCode> KeyBind {
        protected set{}
        get { return _KeyBind; }
    }

    const string SAVE_KEY_OPTION_BGM_VOLUME = "save_key_option_bgm_volume"; //!< bgm音量のキー
    const string SAVE_KEY_OPTION_SE_VOLUME = "save_key_option_se_volume";   //!< se音量のキー
    const string SAVE_KEY_OPTION_TEXT_SPEED = "save_key_option_text_speed"; //!< 文字表示速度のキー
    const string SAVE_KEY_OPTION_KEY_BIND = "save_key_option_key_bind";     //!< キーバインドのキー

    /// <summary>
    /// オプション情報をロード
    /// </summary>
    protected static void LoadOptionData() {
        // 音量
        if (PlayerPrefs.HasKey(SAVE_KEY_OPTION_BGM_VOLUME)) {
            _BGMVolume = PlayerPrefs.GetFloat(SAVE_KEY_OPTION_BGM_VOLUME);
            _SEVolume = PlayerPrefs.GetFloat(SAVE_KEY_OPTION_SE_VOLUME);
        }

        // テキストスピード
        if (PlayerPrefs.HasKey(SAVE_KEY_OPTION_TEXT_SPEED)){
            _TextSpeed = PlayerPrefs.GetFloat(SAVE_KEY_OPTION_TEXT_SPEED);
        }

        // キー設定
        if(PlayerPrefs.HasKey(SAVE_KEY_OPTION_KEY_BIND)){
            // 一旦中身を初期化
            _KeyBind = new Dictionary<string, KeyCode>();
            // キー設定の一行文字列を取得
            string allKeyBindStr = PlayerPrefs.GetString(SAVE_KEY_OPTION_KEY_BIND);
            // コマンド設定ごとに分解
            string[] keyBindStrArr = allKeyBindStr.Split(";");
            // コマンド設定をオプションに反映
            foreach(string keyBindStr in keyBindStrArr){
                string[] commandNameAndKeyCode = keyBindStr.Split(",");
                _KeyBind.Add(commandNameAndKeyCode[0], CommonUtil.ParseEnum<KeyCode>(commandNameAndKeyCode[1]));
            }
        }
    }

    /// <summary>
    /// オプション情報をセーブ
    /// </summary>
    protected static void SaveOptionData() {
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_BGM_VOLUME, BGMVolume);
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_SE_VOLUME, SEVolume);
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_TEXT_SPEED, TextSpeed);

        // キー設定の保存
        string allkeyBindStr = "";
        foreach(string keyBindKey in _KeyBind.Keys){
            allkeyBindStr += keyBindKey + "," + _KeyBind[keyBindKey].ToString() + ";";
        }
        // 末尾のセミコロンは削除
        allkeyBindStr = allkeyBindStr.Remove(allkeyBindStr.Length - 1);
        PlayerPrefs.SetString(SAVE_KEY_OPTION_KEY_BIND, allkeyBindStr);
    }
}
