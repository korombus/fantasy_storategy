using UnityEngine;

public class MapWindow : CommonSys
{
    public bool CLEAR = false;  //!< マップクリアフラグ

    public override void Awake()
    {
        base.Awake();
    }

    void Start(){
        base.BeforStart();

        StartCoroutine(base.AfterStart());
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
