using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class CommonSys : MonoBehaviour
{
    private static CommonSys commonSys;  //!< コモンシステム
    private static void SetSystem(CommonSys sys) { commonSys = sys; }
    public static CommonSys GetSystem() { return commonSys; }
    public static T GetSystem<T>() where T : CommonSys { return (T)commonSys; }

    public static bool PAUSE = false;   //!< 一時停止状態
    public OPTION option = new OPTION();    //!< オプション全般
    
    public GameObject FadeObject;   //!< 画面暗転オブジェクト
    public bool FADE_COMPLETE = false; //!< フェード完了確認用フラグ
    public bool reControllAfterFade = false; //!< フェード後に操作系統を元に戻すかどうか
    public InputSystemUIInputModule inputSystem;    //!< 入力イベント統括システム

    public CommonSound bgm = null;  //!< BGM再生用コンポーネント
    public CommonSound se = null;   //!< SE再生用コンポーネント

    public static bool opening = false; //!< オープニングフラグ

    public virtual void Awake(){
        // フェード用オブジェクトをセット
        if(FadeObject == null){
            GameObject[] fadeObjects = GameObject.FindGameObjectsWithTag("Fade");
            if(fadeObjects.Length == 1){
                FadeObject = fadeObjects[0];
            } else {
                // 複数ある場合は、破壊不可な方を取得するようにする
                if(fadeObjects[0].scene.name == "Title"){
                    DestroyImmediate(fadeObjects[0]);
                    FadeObject = fadeObjects[1];
                } else {
                    DestroyImmediate(fadeObjects[1]);
                    FadeObject = fadeObjects[0];
                }
            }
        }

        // 入力操作を禁止する
        inputSystem.enabled = false;
        // 一時停止状態にする
        PAUSE = true;
    }

    public virtual IEnumerator AfterStart() {
        yield return null;
    }

    public virtual void BeforStart() {
        // システムを登録
        SetSystem(this); 
        
        // オプション読み込み
        option.LoadOption();
        
        // BGM,SEの音量を設定
        option.SetVolume(option.GetVolume(OptionBase.Sound.BGM), OptionBase.Sound.BGM, bgm);
        option.SetVolume(option.GetVolume(OptionBase.Sound.SE), OptionBase.Sound.SE, se);

        // フェードイン開始フラグ設定
        FadeObject.GetComponent<FadeController>().SetData(FadeController.FADE_STATE.IN, this, true);
    }

    // シーン遷移時のフェードアウト制御
    public IEnumerator SceneChangeFadeOut(SCENE_TYPE scene){
        FadeObject.GetComponent<FadeController>().SetData(FadeController.FADE_STATE.OUT, this, false);
        // 入力操作禁止
        inputSystem.enabled = false;
        // 一時停止状態
        PAUSE = true;
        // フェード完了フラグを念のため折っておく
        FADE_COMPLETE = false;

        // フェード完了まで待機
        while(!FADE_COMPLETE){
            yield return null;
        }

        // シーン遷移
        SceneManager.LoadSceneAsync(((int)scene), LoadSceneMode.Single);
    }

    // フェードインとフェードアウトを一度に行う
    public IEnumerator SceneHereFadeInAndOut(Func<bool> betweenFunc, Func<bool> retFunc){
        //フェードアウト
        FadeObject.GetComponent<FadeController>().SetData(FadeController.FADE_STATE.OUT, this, false);
        // 入力操作禁止
        inputSystem.enabled = false;
        // 一時停止状態
        PAUSE = true;
        // フェード完了フラグを念のため折っておく
        FADE_COMPLETE = false;

        // フェード完了まで待機
        while(!FADE_COMPLETE){
            yield return null;
        }

        // フェードアウト後に起動する関数があれば起動
        if(betweenFunc != null){
            betweenFunc();
        }

        FADE_COMPLETE = false;
        // フェードイン
        FadeObject.GetComponent<FadeController>().SetData(FadeController.FADE_STATE.IN, this, true);

        // フェード完了まで待機
        while(!FADE_COMPLETE){
            yield return null;
        }

        // 戻りの関数が指定されている場合は戻り関数を起動
        if(retFunc != null){
            retFunc();
        }
    }

    // 専用シーンを追加
    public IEnumerator SceneAdditive<T>(SCENE_TYPE type, Func<bool> retFunc) where T : IWindow{
        // すでに読み込まれているかを確認
        string sceneName = SceneManager.GetSceneByBuildIndex((int)type).name;
        Scene scene = new Scene();

        if(sceneName == null){
            // 読み込まれていない場合は、シーンを読み込む
            scene = SceneManager.LoadScene(((int)type), new LoadSceneParameters(LoadSceneMode.Additive));
        } else {
            // 読み込まれている場合はsceneを取得
            scene = SceneManager.GetSceneByName(sceneName);
        }

        yield return new WaitUntil(() => scene.isLoaded);

        // 読み込んだシーンに戻り関数と現在のクラスを設置
        scene.GetRootGameObjects()[0].GetComponent<T>().SetData(retFunc, this);
        // 読み込んだシーンをフォーカスする
        SceneManager.SetActiveScene(scene);
    }

    void LateUpdate() {
        // フェードが完了したら操作権限を元に戻す
        if(FADE_COMPLETE && reControllAfterFade){
            inputSystem.enabled = true;
            PAUSE = false;
            FADE_COMPLETE = false;
        }
    }
}

public class OPTION : OptionBase
{
    /// <summary>
    /// 音量調整
    /// </summary>
    /// <param name="volume">音量</param>
    /// <param name="type">音源タイプ</param>
    /// <param name="source">AudioSource</param>
    public void SetVolume(float volume, Sound type, CommonSound source = null){
        switch(type){
            case Sound.BGM:
                OptionBase.BGMVolume = volume;
            break;
            case Sound.SE:
                OptionBase.SEVolume = volume;
            break;
        }

        if(source != null){
            source.ChangeVolume(volume);
        }
    }

    /// <summary>
    /// 音量を取得
    /// </summary>
    /// <param name="type">音源タイプ</param>
    /// <returns>音量</returns>
    public float GetVolume(Sound type){
        switch(type){
            case Sound.BGM:
                return OptionBase.BGMVolume;

            case Sound.SE:
                return OptionBase.SEVolume;
        }
        return 0.5f;
    }

    /// <summary>
    /// 文字表示速度変更
    /// </summary>
    /// <param name="speed">表示速度</param>
    public void SetTextSpeed(float speed){
        OptionBase.TextSpeed = speed;
    }

    /// <summary>
    /// 文字表示速度を取得
    /// </summary>
    /// <returns></returns>
    public float GetTextSpeed(){
        return OptionBase.TextSpeed;
    }

    /// <summary>
    /// オプション読み込み
    /// </summary>
    public void LoadOption(){
        OptionBase.LoadOptionData();
    }

    /// <summary>
    /// オプション保存
    /// </summary>
    public void SaveOption(){
        OptionBase.SaveOptionData();
    }
}
