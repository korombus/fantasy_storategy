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

    private static float _BGMVolume = 0.5f;    //!< BGM音量
    private static float _SEVolume  = 0.5f;    //!< SE音量
    private static float _TextSpeed = 0.07f;   //!< 文字表示速度

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
                SEVolume = value;
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

    const string SAVE_KEY_OPTION_BGM_VOLUME = "save_key_option_bgm_volume"; //!< bgm音量のキー
    const string SAVE_KEY_OPTION_SE_VOLUME = "save_key_option_se_volume";   //!< se音量のキー
    const string SAVE_KEY_OPTION_TEXT_SPEED = "save_key_option_text_speed"; //!< 文字表示速度

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
    }

    /// <summary>
    /// オプション情報をセーブ
    /// </summary>
    protected static void SaveOptionData() {
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_BGM_VOLUME, BGMVolume);
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_SE_VOLUME, SEVolume);
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_TEXT_SPEED, TextSpeed);
    }
}
