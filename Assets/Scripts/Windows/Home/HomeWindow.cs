using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HomeWindow : CommonSys
{
    public GameObject GoBoukenButton;

    public override void Awake()
    {
        base.Awake();
    }

    void Start(){
        base.BeforStart();

        StartCoroutine(base.AfterStart());

        // 冒険ボタンにフォーカスしておく
        EventSystem.current.SetSelectedGameObject(GoBoukenButton);
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
