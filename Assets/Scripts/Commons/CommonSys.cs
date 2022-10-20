using System.Collections;
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
    public InputSystemUIInputModule inputSystem;

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
    public void SetVolume(float volume, Sound type, AudioSource source = null){
        switch(type){
            case Sound.BGM:
                OptionBase.BGMVolume = volume;
            break;
            case Sound.SE:
                OptionBase.SEVolume = volume;
            break;
        }

        if(source != null){
            source.volume = volume;
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
