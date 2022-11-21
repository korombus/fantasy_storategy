using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapWindow : CommonSys
{
    public bool isEventMessage = false; //!< イベントメッセージ表示中フラグ
    public bool CLEAR = false;  //!< マップクリアフラグ
    public GameObject eventMessageWindow;

    public float messageTimer = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start(){
        base.BeforStart();
        // イベントメッセージ用にadvシーンを呼んでおく
        SceneManager.LoadSceneAsync(((int)SCENE_TYPE.SCENARIO), LoadSceneMode.Additive);

        StartCoroutine(base.AfterStart());
    }

    /// <summary>
    /// イベントメッセージ表示
    /// </summary>
    /// <param name="message">表示したい文字列</param>
    public void DispEventMessage(string message){
        // advシーンにテキストを流し込んで表示
        SceneManager.GetSceneByBuildIndex((int)SCENE_TYPE.SCENARIO).GetRootGameObjects()[0].GetComponent<ScenarioWindow>().SetData("", bgm, se, CloseEventMessaeg, new TextAsset(message));
        PAUSE = true;
        isEventMessage = true;
    }

    /// <summary>
    /// イベントメッセージを閉じる
    /// </summary>
    public bool CloseEventMessaeg(){
        isEventMessage = false;
        PAUSE = false;
        return true;
    }

    /// <summary>
    /// マップ攻略フラグをON
    /// </summary>
    public bool MapClear(){
        CLEAR = true;
        this.ShowResult();
        return true;
    }

    /// <summary>
    /// 結果表示
    /// </summary>
    public void ShowResult(){
        this.ReturnHome();
    }

    /// <summary>
    /// 拠点へ戻る
    /// </summary>
    public void ReturnHome(){
        // フェードアウトして拠点画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.HOME));
    }
}
