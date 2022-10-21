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
        if(base.FadeObject != null && base.FadeObject.scene.name == SceneManager.GetSceneByBuildIndex((int)SCENE_TYPE.TITLE).name){
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
        // オープニング再生フラグを立てる
        CommonSys.opening = true;
        // フェードアウトして、拠点画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.HOME));
    }

    public void OnClickLoadButton(){
        // オープニング再生フラグを折る
        CommonSys.opening = false;
        // フェードアウトして、拠点画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.HOME));
    }

    public void OnClickOptionButton(){
        // オプションシーンを追加
        StartCoroutine(SceneAdditive<OptionWindow>(SCENE_TYPE.OPTION, FinishOptionSetting));
    }

    public bool FinishOptionSetting(){
        // フォーカスをオプションボタンに戻す
        EventSystem.current.SetSelectedGameObject(optionButton);
        // 戻ってきたらオプションをセーブ
        option.SaveOption();
        // Title画面を復活
        this.gameObject.SetActive(true);
        return true;
    }

    public void OnClickEndButton(){
        Application.Quit();
    }
}
