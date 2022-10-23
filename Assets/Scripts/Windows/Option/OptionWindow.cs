using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OptionWindow : MonoBehaviour, IWindow
{
    public GameObject musicTab; //!< 音量設定タブ
    public GameObject comfirmPanel; //!< 確認パネル
    public List<GameObject> options = new List<GameObject>(); //!< オプション一覧

    public GameObject poolFocusGameObject = null;   //!< 確認パネル表示時の現在フォーカスされているオブジェクト一時保持
    public Func<bool> returnFunc = null;
    public CommonSys optionOrigin = null;

    public bool isKeyBinding = false;   //!< キーコンフィグ中フラグ
    public string setterKey = "";   //!< 入力されたキー保持用

    void OnEnable(){
        // 最初のオプションをフォーカスしておく
        EventSystem.current.SetSelectedGameObject(musicTab);
        // 確認パネルは非表示にしておく
        comfirmPanel.SetActive(false);
    }

    /// <summary>
    /// オプション画面を開く際に戻り関数を設定
    /// </summary>
    /// <param name="retFunc">delegate</param>
    public void SetData(Func<bool> retFunc, CommonSys sys){
        returnFunc = retFunc;
        optionOrigin = sys;

        // 自身を起こす
        this.gameObject.SetActive(true);

        // 音源設定タブを選択状態にする
        OnClickOptionTab("0");
    }

    public void OnClickOptionTab(string index){
        // クリックされたタブ以外のオプション表示を消す
        options.ForEach((optionItem) => {
            string[] optionParam = optionItem.name.Split('_');
            // 対象の表示であれば表示
            if(optionParam[1] == index){
                optionItem.SetActive(true);
                switch(optionParam[2].ToUpper()){
                    case "MUSIC":
                        // 現在の設定を反映
                        optionItem.GetComponent<MusicOption>().SetData(optionOrigin.option.GetVolume(OptionBase.Sound.BGM), optionOrigin.option.GetVolume(OptionBase.Sound.SE));
                    break;
                    case "KEYBIND":
                        // 現在の設定を反映
                        optionItem.GetComponent<KeyBindOption>().SetData(optionOrigin.option.GetKeyBind());
                    break;
                    case "DISPLAY":
                    break;
                    case "SCENARIO":
                        // 現在の設定を反映
                        optionItem.GetComponent<ScenarioOption>().SetData(optionOrigin.option.GetTextSpeed());
                    break;
                }
            } 
            // 対象でなければ非表示
            else {
                optionItem.SetActive(false);
            }
        });
    }

    public void OnClickComFirmButton(string type){
        // 閉じる場合は、オブジェクト非表示にしておく
        if(type.ToUpper() == "YES"){
            this.gameObject.SetActive(false);

            // オプション後に呼び出す関数が設定されていたら起動
            if(returnFunc != null){
                returnFunc();
            }
        } 
        // 閉じない場合は、確認パネルを消してフォーカスを戻す
        else {
            comfirmPanel.SetActive(false);
            // 前に選択していたオブジェクトにフォーカスを戻す
            EventSystem.current.SetSelectedGameObject(poolFocusGameObject);
        }
    }

    void LateUpdate() {
        // キー設定中は専用の機能を動かす
        if(!isKeyBinding){
            // エスケープか、Xボタンが押されたら
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2)){
                // 現在のフォーカスしているオブジェクトを保存
                poolFocusGameObject = EventSystem.current.currentSelectedGameObject;
                // OptionからTitleへ戻る確認パネルを表示
                comfirmPanel.SetActive(true);
                // フォーカスをYesボタンにする
                EventSystem.current.SetSelectedGameObject(CommonUtil.SearchObjectChild("YesButton", comfirmPanel.transform));
            }
        } else {
            // 押されたキーを取得しておく
            if(Input.anyKey){
                setterKey = JoysticControl.GetInputJoysticButtonType().ToString();
            }
        }
    }
}
