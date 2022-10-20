using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleWindow : CommonSys
{
    public GameObject startButton;    //!< スタートボタン
    public GameObject loadButton;     //!< ロードボタン
    public GameObject optionButton;   //!< オプションボタン

    public override void Awake()
    {
        base.Awake();
        // フェードオブジェクトはすべてのシーンで使い回す
        if(base.FadeObject != null && base.FadeObject.scene.name == "Title"){
            DontDestroyOnLoad(base.FadeObject);
        }
    }

    void Start(){
        // 事前処理
        base.BeforStart();

        // 処理負荷が必要な事前処理
        StartCoroutine(base.AfterStart());

        // 始めるボタンにフォーカスしておく
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    public void OnClickStartButton(){
        // フェードアウトして、拠点画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.HOME));
    }

    public void OnClickLoadButton(){

    }

    public void OnClickOptionButton(){

    }

    public void OnClickEndButton(){
        Application.Quit();
    }
}
