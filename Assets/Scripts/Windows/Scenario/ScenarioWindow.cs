using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioWindow : MonoBehaviour
{
    public Text scenarioText;
    public Text charaNameText;
    public Text logText;
    public Text MonologueScenarioText;
    public Image charaObjR;
    public Image charaObjC;
    public Image charaObjL;
    public Image fadePanel;
    public Image MonologuePanel;
    public Image scenarioTextPanel;
    public List<RawImage> bgImg = new List<RawImage>();
    public GameObject choicePanel;

    public GameObject logObject;
    /// <summary> メニュー表示 </summary>
    public bool isMenuDisp = false;
    /// <summary> セーブ画面表示 </summary>
    public bool isSaveScene = false;
    public ReadScenario readScenario;
    public bool isHoldLog = false;
    private bool isLog = false;

    public Func<bool> scenarioFinalyReturnFunc = null;

    public void SetData(string senarioTxtName, CommonSound bgm, CommonSound se, Func<bool> retFunc, TextAsset senarioTxtBody = null)
    {
        // テキスト呼び出し
        TextAsset textData = senarioTxtBody;
        if(textData == null){
            textData = Resources.Load<TextAsset>("Scenario/" + senarioTxtName);
        }
        readScenario = new ReadScenario(textData, bgm, se);
        readScenario.SetADVUI(
            scenarioText, 
            charaNameText, 
            logText, 
            charaObjR, 
            charaObjC, 
            charaObjL, 
            bgImg, 
            fadePanel,
            scenarioTextPanel,
            MonologuePanel,
            MonologueScenarioText,
            choicePanel
        );

        // シナリオ終了時に呼び出す関数を設定
        scenarioFinalyReturnFunc = retFunc;

        // ゲーム全体を一時停止
        CommonSys.PAUSE = true;

        // シナリオオブジェクト復帰
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        //セーブ画面を表示してない場合はシナリオを読ませる
        if(!isSaveScene)
        {
            // メニューを表示してない場合はシナリオを読ませる
            if (!isMenuDisp)
            {
                // ログを表示してない場合はシナリオを読ませる
                if (!isLog)
                {
                    // 右クリックでログを表示
                    if (Input.GetMouseButtonDown(1) && !isHoldLog)
                    {
                        isLog = true;
                        logObject.SetActive(isLog);
                    }

                    // マウスを押している間、読む速度上昇
                    if (Input.GetKey(KeyCode.C) || Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.JoystickButton0))
                    {
                        readScenario.OnClickDisplay();
                    }
                    // シナリオ読み出し
                    bool scenarioEndFlag = readScenario.ReadScenarioText();

                    // シナリオが読み終わったらシナリオを停止
                    if (scenarioEndFlag)
                    {
                        this.gameObject.SetActive(false);
                        scenarioFinalyReturnFunc();
                    }
                }
                else
                {
                    // ログを非表示にする
                    if (Input.GetMouseButtonDown(1))
                    {
                        isLog = false;
                        logObject.SetActive(isLog);
                    }
                }
            }
        }
    }
}
