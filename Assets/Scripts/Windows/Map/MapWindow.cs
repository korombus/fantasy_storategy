using UnityEngine;
using TMPro;

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

        StartCoroutine(base.AfterStart());
    }

    void Update(){
        if(isEventMessage){
            if(messageTimer > 1 && Input.GetKeyDown(KeyCode.JoystickButton0)){
                CloseEventMessaeg();
            }
            // メッセージ表示後1秒待機を挟む
            messageTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// イベントメッセージ表示
    /// </summary>
    /// <param name="message">表示したい文字列</param>
    public void DispEventMessage(string message){
        if(eventMessageWindow != null){
            eventMessageWindow.SetActive(true);
            eventMessageWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        }
        PAUSE = true;
        isEventMessage = true;
        messageTimer = 0;
    }

    /// <summary>
    /// イベントメッセージを閉じる
    /// </summary>
    public void CloseEventMessaeg(){
        if(eventMessageWindow != null){
            eventMessageWindow.SetActive(false);
        }
        isEventMessage = false;
        PAUSE = false;
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
