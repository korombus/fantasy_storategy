using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HomeWindow : CommonSys
{
    public GameObject GoBoukenButton;
    public GameObject Canvas;

    public override void Awake()
    {
        base.Awake();

        // オープニングの場合は、シナリオを読み出す
        if(CommonSys.opening){
            // シナリオシーンの呼び出し
            StartCoroutine(LoadOpening());
            // Canvasを非表示にしておく
            Canvas.SetActive(false);
        }
    }

    IEnumerator LoadOpening(){
        // シナリオシーン読み出し
        yield return SceneManager.LoadSceneAsync(((int)SCENE_TYPE.SCENARIO), LoadSceneMode.Additive);
        // オープニングシナリオをセット
        SceneManager.GetSceneByName("adv").GetRootGameObjects()[0].GetComponent<ScenarioWindow>().SetData("opening", bgm, se, OpeningEnd);
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

    }

    /// <summary>
    /// 鍛冶処へ
    /// </summary>
    public void OnCliCkGoKajiButton(){

    }

    public void OnClickGoTitleButton(){
        // フェードアウトして、タイトル画面へ遷移
        StartCoroutine(base.SceneChangeFadeOut(SCENE_TYPE.TITLE));
    }
}
