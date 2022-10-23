using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenkinWindow : CommonSys
{

    public GameObject startButton;  //!< 最初にフォーカスしておくボタン
    public GameObject confirmPanelPrefab; //!< 確認パネル
    public GameObject confirmPanel; //!< 確認パネル
    public GameObject poolFocusGameObject = null;   //!< 確認パネル表示時の現在フォーカスされているオブジェクト一時保持

    public override void Awake()
    {
        base.Awake();

        // 確認パネルを読み込み、非表示にしておく
        confirmPanel = CommonUtil.PrefabInstance(confirmPanelPrefab, this.transform);
        confirmPanel.GetComponent<ConfirmPanel>().SetData("メインホールに戻りますか？", OnClickConFirmButton, OnClickConFirmButton);
        confirmPanel.SetActive(false);
    }

    void Start(){
        base.BeforStart();

        StartCoroutine(base.AfterStart());

        // 最初のボタンにフォーカスする
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnClickConFirmButton(string type){
        // 閉じる場合は、オブジェクト非表示にしておく
        if(type.ToUpper() == "YES"){
            // HOMEへ移動
            StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.HOME));
        } 
        // 閉じない場合は、確認パネルを消してフォーカスを戻す
        else {
            confirmPanel.SetActive(false);
            // 前に選択していたオブジェクトにフォーカスを戻す
            EventSystem.current.SetSelectedGameObject(poolFocusGameObject);
        }
    }

    void Update(){
        // エスケープか、Xボタンが押されたら
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2)){
            // 現在のフォーカスしているオブジェクトを保存
            poolFocusGameObject = EventSystem.current.currentSelectedGameObject;
            // RenkinからHomeへ戻る確認パネルを表示
            confirmPanel.SetActive(true);
            // フォーカスをYesボタンにする
            EventSystem.current.SetSelectedGameObject(CommonUtil.SearchObjectChild("YesButton", confirmPanel.transform));
        }
    }
}
