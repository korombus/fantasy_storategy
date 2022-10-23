using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HomeWindow : CommonSys
{
    public GameObject GoBoukenButton;
    public GameObject Canvas;
    public AudioClip homeBgm;

    public override void Awake()
    {
        base.Awake();

        // ホームBGMが無い場合はここで読み込んでおく
        if(homeBgm == null){
            homeBgm = Resources.Load<AudioClip>(CommonSound.BGMPath + "Home.mp3");
        }

        // オープニングの場合は、シナリオを読み出す
        if(CommonSys.opening){
            // シナリオシーンの呼び出し
            StartCoroutine(LoadOpening());
            // Canvasを非表示にしておく
            Canvas.SetActive(false);
        } else {
            // オープニングが再生されない場合は、音楽を流す
            bgm.Play(homeBgm);
        }
    }

    IEnumerator LoadOpening(){
        // シナリオシーン読み出し
        yield return SceneManager.LoadSceneAsync(((int)SCENE_TYPE.SCENARIO), LoadSceneMode.Additive);
        // オープニングシナリオをセット
        SceneManager.GetSceneByBuildIndex((int)SCENE_TYPE.SCENARIO).GetRootGameObjects()[0].GetComponent<ScenarioWindow>().SetData("opening", bgm, se, OpeningEnd);
    }

    void Start(){
        base.BeforStart();

        StartCoroutine(base.AfterStart());

        // 冒険ボタンにフォーカスしておく
        EventSystem.current.SetSelectedGameObject(GoBoukenButton);
    }

    // オープニング終了時の戻り関数
    public bool OpeningEnd(){
        // フェードアウトとフェードインを行う
        StartCoroutine(SceneHereFadeInAndOut(OpeningEndBetween, null));
        // オープニングフラグを折る
        CommonSys.opening = false;
        return true;
    }

    // オープニング終了後のフェードアウトした後に起動する関数
    public bool OpeningEndBetween(){
        // シナリオを読み終わったら、表示を戻す
        Canvas.SetActive(true);
        // 音楽を流し始める
        bgm.Play(homeBgm);
        return true;
    }

    /// <summary>
    /// 冒険へ
    /// </summary>
    public void OnClickGoBoukenButton(){
        // フェードアウトして、冒険画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.MAP));
    }

    /// <summary>
    /// 牧場へ
    /// </summary>
    public void OnClickGoBokujouButton(){

    }

    /// <summary>
    /// 錬金室へ
    /// </summary>
    public void OnClickGoRenkinButton(){
        // フェードアウトして、錬金画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.RENKIN));
    }

    /// <summary>
    /// 鍛冶処へ
    /// </summary>
    public void OnClickGoKajiButton(){

    }

    public void OnClickGoTitleButton(){
        // フェードアウトして、タイトル画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.TITLE));
    }
}
