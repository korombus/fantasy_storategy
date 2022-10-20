using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ReadScenario
{

    // 音楽のフェードをコントロールするクラス
    internal class BGMFadeController
    {
        public int id;             //!< フェードID(0:イン1, 1:アウト1, 2:イン2, 3:アウト2)
        public bool fadeInFlag;    //!< フェードインしたかのフラグ
        public float speed;        //!< フェード速度

        internal BGMFadeController(int i_id, bool i_flag, float i_speed)
        {
            id = i_id;
            fadeInFlag = i_flag;
            speed = i_speed;
        }
    }

    public class ADVUI
    {
        public Text frameText;             //!< テキスト表示用フレーム
        public Text charaNameText;         //!< キャラ名表示テキスト
        public Text logText;               //!< ログ表示用テキスト
        public Text MonologueText;          //!< モノローグテキスト表示用フレーム
        public Image textPanel;            //!< テキスト用オブジェクト
        public Image monologuePanel;       //!< モノローグ用オブジェクト
        public Image fadePanel;            //!< フェードイン/アウト用オブジェクト
        public Image moveObj;              //!< 立ち絵移動用オブジェクト
        public Image shakeObj;             //!< 振動用オブジェクト
        public List<RawImage> bgImage;     //!< 背景オブジェクト
        public GameObject panelChoiceObj;  //!< 選択肢パネルオブジェクト

        public int lineLimitWordNum = 0;                       //!< 一行に表示される最大文字数
        public int lineLimitNewLineNum = 0;                    //!< 最大改行数
        public int monologueLineLimitWordNum = 0;              //!< 一行に表示される最大文字数
        public int monologueLineLimitNewLineNum = 0;           //!< 最大改行数

        public ADVUI(
            Text i_frameText,
            Text i_name,
            Text i_log,
            List<RawImage> i_bgImg,
            Image i_fade,
            Image TextPanel,
            Image i_Monologue,
            Text i_MonologueText,
            GameObject i_choicePanel
            )
        {
            // テキスト表示用フレームセット
            frameText = i_frameText;
            frameText.text = "";
            textPanel = TextPanel;

            //モノローグテキスト表示用フレームセット
            MonologueText = i_MonologueText;
            MonologueText.text = "";

            // 一行の表示文字数を取得
            lineLimitWordNum = (int)frameText.rectTransform.sizeDelta.x / frameText.fontSize;
            lineLimitNewLineNum = (int)frameText.rectTransform.sizeDelta.y / frameText.fontSize;

            monologueLineLimitWordNum = (int)MonologueText.rectTransform.sizeDelta.x / MonologueText.fontSize;
            monologueLineLimitNewLineNum = (int)MonologueText.rectTransform.sizeDelta.y / MonologueText.fontSize;


            // キャラ名表示用テキスト
            charaNameText = i_name;
            charaNameText.text = "";

            // log表示用テキスト
            logText = i_log;
            logText.text = "";

            // 背景画像表示用イメージ
            bgImage = i_bgImg;
            bgImage.ForEach(n => n.gameObject.SetActive(false));

            //モノローグ用オブジェクト
            monologuePanel = i_Monologue;
            monologuePanel.gameObject.SetActive(false);

            // フェードイン/アウト用オブジェクト
            fadePanel = i_fade;

            // 選択肢パネルオブジェクト
            panelChoiceObj = i_choicePanel;
        }
    }


    // 定数
    public const string CHARACTER_TEXTURE_PATH = "Textures/Characters/";   //!< キャラクター画像パス
    public const string BACKGROUND_TEXTURE_PATH = "Textures/background/";  //!< 背景画像パス
    private const string AUTO_NEXT_LINE_COMMAND_WORD = "[$&";

    // 内部変数
    private float timer = 0f;                               //!< 読み出しタイマー
    private float speed = 0.6f;                             //!< 読み出し速度
    private float[] waitTime = new float[] { 0f, 0f };      //!< 読み出し一時停止時間
    private float[] waitTimer = new float[] { 0f, 0f };      //!< 読み出し一時停止タイマー
    private float shakeSpan = 1f;                           //!< 画面振動間隔
    private float shakeLength = 10.0f;                      //!< 画面振動距離
    private float charaShakeSpan = 1f;                      //!< 立ち絵振動間隔
    private float charaShakeLength = 10.0f;                 //!< 立ち絵振動距離

    private bool endState = true;       //!< 読み終わり状態
    private bool fadeStart = false;     //!< 画面フェード開始フラグ
    private bool charaFadeStart = false;//!< キャラフェード開始フラグ
    private bool moveStart = false;     //!< 立ち絵移動開始フラグ
    private bool moveModal = false;     //!< 立ち絵のモーダル移動
    private bool shakeStart = false;    //!< 画面振動開始フラグ
    private bool shakeModal = false;    //!< 画面のモーダル振動
    private bool verticalShake = false; //!< 画面の縦移動（true）/横移動（false）
    private bool charaShakeStart = false;//!< 立ち絵振動開始フラグ
    private bool charaShakeModal = false;//!< 立ち絵のモーダル振動
    private bool verticalCharaShake = false;//!< 立ち絵の縦移動（true）/横移動（false）
    private bool MonologueEnable = false;//!< モノローグ
    private bool zoomStart = false;     //!< 背景ズーム＆移動
    private bool zoomModal = false;     //!< 背景ズームのモーダルフラグ

    private List<Vector3> zoomPosScaleList = new List<Vector3>();   //!< ズーム用位置、スケール格納変数[0]:StartPos [1]:StartScale [2]:TargetPos [3]:TargetScale
    private List<Color> fadeColorList = new List<Color>();          //!< フェード
    private List<Vector3> movePosList = new List<Vector3>();        //!< 立ち絵移動用パラメータ変数
    private List<Vector3> charaShakePosList = new List<Vector3>();  //!< 立ち絵振動用パラメータ変数
    private List<Vector3> defaultMovePosList = new List<Vector3>(); //!< 立ち絵移動前位置[0]:R [1]:C [2]:L

    private ADVUI advUI;

    private CommonSound bgmObject;      //!< bgm再生用オブジェクト
    private CommonSound seObject;       //!< se再生用オブジェクト

    private TextAsset scenarioText;     //!< 読み出すシナリオテキスト
    private StateReadScenario state;    //!< シナリオの読み出し状態

    Dictionary<string, Image> charaObjList = new Dictionary<string, Image>(); //!< キャラクターオブジェクト
    Dictionary<string, string> charaNameList = new Dictionary<string, string>(); //!< キャラクター名

    private List<string> lineText = new List<string>();     //!< 一行テキスト
    private int lineIndex = 0;                              //!< 一行テキストのインデックス

    public string nextChoiceOne = "";   //!< 選択肢１の遷移先
    public string nextChoiceTwo = "";   //!< 選択肢２の遷移先

    public IEnumerator LoadSaveData(string saveData)
    {
        string[] loadData = saveData.Split(',');
        for (int i = 0; i < loadData.Length; i++) {
            string[] data = loadData[i].Split(':');
            switch (data[0]) {
                case "lineIndex":
                    lineIndex = int.Parse(data[1]) - 1;
                    break;
                case "charaObjR":
                    if (!string.IsNullOrEmpty(data[1])) {
                        charaObjList["R"].sprite = Resources.Load<Sprite>(CHARACTER_TEXTURE_PATH + data[1]);
                        charaObjList["R"].gameObject.SetActive(true);
                        charaObjList["R"].transform.localPosition = defaultMovePosList[0];
                    }
                    break;
                case "charaObjRColor":
                    if (!string.IsNullOrEmpty(data[1])) {
                        Color color;
                        if (ColorUtility.TryParseHtmlString("#" + data[1], out color)){
                            charaObjList["R"].color = color;
                        }
                    }
                    break;
                case "charaObjC":
                    if (!string.IsNullOrEmpty(data[1])) {
                        charaObjList["C"].sprite = Resources.Load<Sprite>(CHARACTER_TEXTURE_PATH + data[1]);
                        charaObjList["C"].gameObject.SetActive(true);
                        charaObjList["C"].transform.localPosition = defaultMovePosList[1];
                    }
                    break;
                case "charaObjCColor":
                    if (!string.IsNullOrEmpty(data[1])) {
                        Color color;
                        if (ColorUtility.TryParseHtmlString("#" + data[1], out color)) {
                            charaObjList["C"].color = color;
                        }
                    }
                    break;
                case "charaObjL":
                    if (!string.IsNullOrEmpty(data[1])) {
                        charaObjList["L"].sprite = Resources.Load<Sprite>(CHARACTER_TEXTURE_PATH + data[1]);
                        charaObjList["L"].gameObject.SetActive(true);
                        charaObjList["L"].transform.localPosition = defaultMovePosList[2];
                    }
                    break;
                case "charaObjLColor":
                    if (!string.IsNullOrEmpty(data[1])) {
                        Color color;
                        if (ColorUtility.TryParseHtmlString("#" + data[1], out color)) {
                            charaObjList["L"].color = color;
                        }
                    }
                    break;
                case "charaNameR":
                    charaNameList["R"] = data[1];
                    break;
                case "charaNameC":
                    charaNameList["C"] = data[1];
                    break;
                case "charaNameL":
                    charaNameList["L"] = data[1];
                    break;
                case "frameText":
                    advUI.frameText.text = data[1];
                    break;
                case "charaNameText":
                    advUI.charaNameText.text = data[1];
                    break;
                case "logText":
                    advUI.logText.text = data[1];
                    break;
                case "MonologueText":
                    advUI.MonologueText.text = data[1];
                    break;
                case "bgImg0":
                    if (!string.IsNullOrEmpty(data[1])) {
                        advUI.bgImage[0].texture = Resources.Load<Texture>(BACKGROUND_TEXTURE_PATH + data[1]);
                        advUI.bgImage[0].gameObject.SetActive(true);
                        advUI.bgImage[0].color = Color.white;
                    }
                    break;
                case "bgImg1":
                    if (!string.IsNullOrEmpty(data[1])) {
                        advUI.bgImage[1].texture = Resources.Load<Texture>(BACKGROUND_TEXTURE_PATH + data[1]);
                        advUI.bgImage[1].gameObject.SetActive(true);
                        advUI.bgImage[1].color = Color.white;
                    }
                    break;
            }
        }
        yield return null;
    }

    public string GetSaveData()
    {
        string saveData = "lineIndex:" + lineIndex.ToString() + ",";               //!< 現在の行数

        if (charaObjList["R"].gameObject.activeSelf && charaObjList["R"].sprite != null) {
            saveData += "charaObjR:" + charaObjList["R"].sprite.name + ",";             //!< 右キャラクター画像名
            saveData += "charaObjRColor:" + ColorUtility.ToHtmlStringRGBA(charaObjList["R"].color) + ",";   //!< 右キャラクター画像色

        }

        if (charaObjList["C"].gameObject.activeSelf && charaObjList["C"].sprite != null) {
            saveData += "charaObjC:" + charaObjList["C"].sprite.name + ",";
            saveData += "charaObjCColor:" + ColorUtility.ToHtmlStringRGBA(charaObjList["C"].color) + ",";
        }

        if (charaObjList["L"].gameObject.activeSelf && charaObjList["L"].sprite != null) {
            saveData += "charaObjL:" + charaObjList["L"].sprite.name + ",";
            saveData += "charaObjLColor:" + ColorUtility.ToHtmlStringRGBA(charaObjList["L"].color) + ",";
        }

        Debug.Log(saveData);

        saveData += "charaNameR:" + charaNameList["R"] + "," +  //!< 右キャラクター名
        "charaNameC:" + charaNameList["C"] + "," +              //!< 中央キャラクター名
        "charaNameL:" + charaNameList["L"] + "," +              //!< 左キャラクター名

        "frameText:" + advUI.frameText.text + "," +             //!< 現在表示されているテキスト
        "charaNameText:" + advUI.charaNameText.text + "," +     //!< 現在表示されているキャラ名
        "logText:" + advUI.logText.text + "," +                 //!< 現在表示されているログ
        "MonologueText:" + advUI.MonologueText.text + ",";      //!< 現在表示されているモノローグテキスト
        if(advUI.bgImage[0].texture != null) {
            saveData += "bgImg0:" + advUI.bgImage[0].texture.name + ",";  //!< 現在表示されている背景画像名０
        }

        if(advUI.bgImage[1].texture != null) {
            saveData += "bgImg1:" + advUI.bgImage[1].texture.name;  //!< 現在表示されている背景画像名１
        }

        return saveData;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="txt">テキストデータ</param>
    public ReadScenario(TextAsset txt, CommonSound bgm, CommonSound se, string debugtxt = null)
    {
        // シナリオのテキストをセット
        scenarioText = txt;
        state = StateReadScenario.BEGINE;
        lineText.Clear();
        // シナリオを一行ずつに切り分け
        if (scenarioText == null) { CreateLineText(debugtxt); }
        else { CreateLineText(scenarioText); }

        endState = false;
        timer = 0f;

        // bgm用コンポーネント
        bgmObject = bgm;
        bgmObject.ChangeVolume(OPTION.BGMVolume);

        // se用コンポーネント
        seObject = se;
        seObject.ChangeVolume(OPTION.SEVolume);
    }

    public void SetADVUI(
        Text i_frameText, 
        Text i_name, 
        Text i_log, 
        Image i_charaR, 
        Image i_charaC, 
        Image i_charaL, 
        List<RawImage> i_bgImg, 
        Image i_fade,
        Image TextPanel,
        Image i_Monologue,
        Text i_MonologueText,
        GameObject i_choicePanel
        )
    {
        // UIをセット
        advUI = new ADVUI(i_frameText, i_name, i_log, i_bgImg, i_fade, TextPanel, i_Monologue, i_MonologueText, i_choicePanel);

        //キャラクター表示用オブジェクト
        charaObjList.Add("R", i_charaR);
        charaObjList.Add("C", i_charaC);
        charaObjList.Add("L", i_charaL);

        //キャラクター名表示用リスト
        charaNameList.Add("R", string.Empty);
        charaNameList.Add("C", string.Empty);
        charaNameList.Add("L", string.Empty);

        // キャラクター表示用オブジェクト初期位置
        defaultMovePosList.Add(i_charaR.transform.localPosition);
        defaultMovePosList.Add(i_charaC.transform.localPosition);
        defaultMovePosList.Add(i_charaL.transform.localPosition);

        ////ズーム用リスト
        defaultMovePosList.Add(i_bgImg[0].transform.position);
    }

    /// <summary>
    /// シナリオを読み出す
    /// </summary>
    /// <returns></returns>
    private int wordIndex = 0;      // 現在の文字位置
    private int colorCodeLen = 0;   // 色の指定の文字の長さ
    public bool ReadScenarioText()
    {
        switch (state)
        {
            case StateReadScenario.BEGINE:
                // 読み出し開始
                state = StateReadScenario.READ;
                break;

            case StateReadScenario.STOP:
                // 行末に記号を表示
                if(timer >= speed) {
                    if(advUI.frameText.text.Substring(advUI.frameText.text.Length-1, 1) == "▼") {
                        advUI.frameText.text = advUI.frameText.text.Substring(0, advUI.frameText.text.Length - 1);
                    } else {
                        advUI.frameText.text += "▼";
                    }

                    timer = 0;
                }
                timer += Time.deltaTime;
                break;

            case StateReadScenario.READ:
                // ここを通る場合は全て待機させる
                if (lineText[lineIndex].Length <= wordIndex)
                {
                    // まだ文があれば一時停止する
                    if (lineText.Count > lineIndex + 1)
                    {
                        lineIndex++;
                        wordIndex = 0;
                        timer = 0;
                        state = StateReadScenario.STOP;
                    }
                    else
                    {
                        // 全部読んだら終わり
                        state = StateReadScenario.END;
                    }
                }
                else
                {
                    // コマンド読み込み
                    if (lineText[lineIndex].Substring(0, 1) == "#")
                    {
                        Command(lineText[lineIndex].Split(new char[] { ' ' }), ScenarioCommandType.SHARP);
                        lineIndex++;
                        // 最後がコマンドで終わったらそのまま開始する
                        if (lineIndex >= lineText.Count)
                        {
                            state = StateReadScenario.NONE;
                            endState = true;
                        }
                    }

                    // 文字の色変え事前処理
                    else if (lineText[lineIndex].Substring(wordIndex, 1) == "<" && lineText[lineIndex].Substring(wordIndex, 2) == "<@")
                    {
                        // 最初の'>'までが色指定なのでそこを保持
                        int len = 2;
                        List<string> com = new List<string>() { "<color>" };
                        // カラーコード抜き出し
                        while (true)
                        {
                            if (lineText[lineIndex].Substring(wordIndex + len + 2, 1) == ">")
                            {
                                colorCodeLen = len;
                                com.Add(lineText[lineIndex].Substring(wordIndex + 2, len));
                                break;
                            }
                            len++;
                        }

                        int bodyLen = 1;
                        // カラーコードで囲われた本文の抜き出し
                        while (true)
                        {
                            if (lineText[lineIndex].Substring(wordIndex + colorCodeLen + 3 + bodyLen, 1) == "<")
                            {
                                com.Add(lineText[lineIndex].Substring(wordIndex + colorCodeLen + 3, bodyLen));
                                break;
                            }
                            bodyLen++;
                        }
                        // コマンド実行
                        Command(com.ToArray(), ScenarioCommandType.LINECOM);
                    }

                    // 文字の色変え
                    else if (lineText[lineIndex].Substring(wordIndex, 1) == "<" && lineText[lineIndex].Substring(wordIndex, 2) == "<c")
                    {
                        if (timer >= speed)
                        {
                            // カラーコードを考慮して現時点から色文字全文を先にログに表示
                            advUI.logText.text += lineText[lineIndex].Substring(wordIndex, colorCodeLen + 17);

                            //「<color=>W</color>」とカラーコード分文字を読み込む進める
                            wordIndex += colorCodeLen + 17;
                            if (true == MonologueEnable)
                            {
                                advUI.MonologueText.text = lineText[lineIndex].Substring(0, wordIndex);
                                // 現在の行数を取得する為、テキストジェネレータを生成
                                TextGenerator tg = new TextGenerator();
                                tg.Populate(advUI.MonologueText.text, advUI.MonologueText.GetGenerationSettings(advUI.MonologueText.rectTransform.sizeDelta));

                                // 表示範囲を超えている場合は、はみ出ている文字を次の行とするコマンドを追加 (文字が最大数を超えている || 改行数が最大範囲を超える))
                                if (tg.lineCount >= advUI.monologueLineLimitNewLineNum
                                && (advUI.MonologueText.text.Length >= advUI.monologueLineLimitWordNum * advUI.monologueLineLimitNewLineNum
                                || IsNewLine(lineText[lineIndex].Substring(wordIndex, 1))
                                || wordIndex - tg.lines[2].startCharIdx >= advUI.lineLimitWordNum))
                                {
                                    lineText[lineIndex] = advUI.MonologueText.text + AUTO_NEXT_LINE_COMMAND_WORD + lineText[lineIndex].Substring(wordIndex, lineText[lineIndex].Length - wordIndex);
                                }
                            }
                            else
                            {
                                advUI.frameText.text = lineText[lineIndex].Substring(0, wordIndex);
                                // 現在の行数を取得する為、テキストジェネレータを生成
                                TextGenerator tg = new TextGenerator();
                                tg.Populate(advUI.frameText.text, advUI.frameText.GetGenerationSettings(advUI.frameText.rectTransform.sizeDelta));

                                // 表示範囲を超えている場合は、はみ出ている文字がある、もしくは再度改行しようとしている場合に次の行とするコマンドを追加
                                if (tg.lineCount >= advUI.lineLimitNewLineNum
                                && (advUI.frameText.text.Length >= advUI.lineLimitWordNum * advUI.lineLimitNewLineNum
                                || IsNewLine(lineText[lineIndex].Substring(wordIndex, 1))
                                || wordIndex - tg.lines[2].startCharIdx >= advUI.lineLimitWordNum))
                                {
                                    lineText[lineIndex] = advUI.frameText.text + AUTO_NEXT_LINE_COMMAND_WORD + lineText[lineIndex].Substring(wordIndex, lineText[lineIndex].Length - wordIndex);
                                }
                            }
                        }
                        timer += Time.deltaTime;
                    }

                    // 台詞読み込み
                    else if (lineText[lineIndex].Substring(0, 1) != "@" || (lineText[lineIndex].Length < 2 && lineText[lineIndex].Substring(0, 2) != "//"))
                    {
                        // 一行内のコマンドに当たったらコマンド実行
                        if (lineText[lineIndex].Substring(wordIndex, 1) == "[")
                        {
                            Command(new string[] { lineText[lineIndex].Substring(wordIndex, 3) }, ScenarioCommandType.LINECOM);
                        }
                        // それ以外は普通に表示
                        else
                        {
                            if (timer >= speed)
                            {
                                wordIndex++;
                                if (true == MonologueEnable)
                                {
                                    advUI.MonologueText.text = lineText[lineIndex].Substring(0, wordIndex);
                                    // 現在の行数を取得する為、テキストジェネレータを生成
                                    TextGenerator tg = new TextGenerator();
                                    tg.Populate(advUI.MonologueText.text, advUI.MonologueText.GetGenerationSettings(advUI.MonologueText.rectTransform.sizeDelta));

                                    // 表示範囲を超えている場合は、はみ出ている文字を次の行とするコマンドを追加 (文字が最大数を超えている || 改行数が最大範囲を超える))
                                    if (tg.lineCount >= advUI.monologueLineLimitNewLineNum 
                                    && (advUI.MonologueText.text.Length >= advUI.monologueLineLimitWordNum * advUI.monologueLineLimitNewLineNum 
                                    || IsNewLine(lineText[lineIndex].Substring(wordIndex, 1)) 
                                    || wordIndex - tg.lines[2].startCharIdx >= advUI.lineLimitWordNum))
                                    {
                                        lineText[lineIndex] = advUI.MonologueText.text + AUTO_NEXT_LINE_COMMAND_WORD + lineText[lineIndex].Substring(wordIndex, lineText[lineIndex].Length - wordIndex);
                                    }
                                }
                                else
                                {
                                    advUI.frameText.text = lineText[lineIndex].Substring(0, wordIndex);
                                    // 現在の行数を取得する為、テキストジェネレータを生成
                                    TextGenerator tg = new TextGenerator();
                                    tg.Populate(advUI.frameText.text, advUI.frameText.GetGenerationSettings(advUI.frameText.rectTransform.sizeDelta));

                                    // 表示範囲を超えている場合は、はみ出ている文字がある、もしくは再度改行しようとしている場合に次の行とするコマンドを追加
                                    if (tg.lineCount >= advUI.lineLimitNewLineNum
                                    && (advUI.frameText.text.Length >= advUI.lineLimitWordNum * advUI.lineLimitNewLineNum
                                    || IsNewLine(lineText[lineIndex].Substring(wordIndex, 1)) 
                                    || wordIndex - tg.lines[2].startCharIdx >= advUI.lineLimitWordNum))
                                    {
                                        lineText[lineIndex] = advUI.frameText.text + AUTO_NEXT_LINE_COMMAND_WORD + lineText[lineIndex].Substring(wordIndex, lineText[lineIndex].Length - wordIndex);
                                    }
                                }
                                advUI.logText.text += lineText[lineIndex].Substring(wordIndex - 1, 1);
                            }
                            timer += Time.deltaTime;
                        }
                        // 文の終わりに来たら
                        if (wordIndex >= lineText[lineIndex].Length)
                        {
                            // まだ文があれば一時停止する
                            if (lineText.Count > lineIndex + 1)
                            {
                                lineIndex++;
                                wordIndex = 0;
                                timer = 0;
                                state = StateReadScenario.STOP;

                                // ログに改行を追加する
                                advUI.logText.text += Environment.NewLine;
                            }
                            else
                            {
                                // 全部読んだら終わり
                                state = StateReadScenario.END;
                            }
                        }
                    }

                    // コメントや事前処理は全て飛ばす
                    else
                    {
                        lineIndex++;
                    }
                }
                break;

            case StateReadScenario.WAIT:
                if (waitTimer[0] > waitTime[0])
                {
                    waitTimer[0] = 0f;
                    state = StateReadScenario.READ;
                    // 移動フラグ型付け
                    if (moveStart)
                    {
                        moveStart = false;
                        movePosList.Clear();
                    }
                    // 画面振動フラグ型付け
                    if (shakeStart)
                    {
                        shakeStart = false;
                    }
                    // 立ち絵振動フラグ型付け
                    if (charaShakeStart)
                    {
                        charaShakeStart = false;
                    }
                    // フェードフラグ片付け
                    if (fadeStart)
                    {
                        fadeStart = false;
                        fadeColorList.Clear();
                    }
                    // 立ち絵フェードフラグ片付け
                    if (charaFadeStart)
                    { 
                        charaFadeStart = false;
                        fadeColorList.Clear();
                    }
                    // ズームフラグ片付け
                    if(zoomStart)
                    {
                        zoomStart = false;
                        zoomPosScaleList[1] = zoomPosScaleList[3] = Vector3.zero;
                        zoomPosScaleList[0] = zoomPosScaleList[2] = Vector3.zero;
                    }
                }
                // 移動フラグが立っていたら移動させる
                if ((moveStart) && (moveModal))
                    advUI.moveObj.transform.localPosition = Vector3.Lerp(movePosList[0], movePosList[1], waitTimer[0] / waitTime[0]);

                // 振動フラグが立っていたら振動させる
                else if (shakeStart && shakeModal)
                    advUI.bgImage[0].transform.localPosition = (verticalShake) ? new Vector3(advUI.bgImage[0].transform.localPosition.x
                                                                        , shakeLength * Mathf.Sin(Time.frameCount * shakeSpan + shakeLength)
                                                                        + advUI.bgImage[0].transform.localPosition.y
                                                                        , advUI.bgImage[0].transform.localPosition.z)
                                                                    : new Vector3(Mathf.Sin(Time.frameCount * shakeSpan + shakeLength)
                                                                        + advUI.bgImage[0].transform.localPosition.x
                                                                        , advUI.bgImage[0].transform.localPosition.y
                                                                        , advUI.bgImage[0].transform.localPosition.z);

                // 振動フラグが立っていたら振動させる
                else if (charaShakeStart && charaShakeModal)
                    advUI.shakeObj.transform.localPosition = (verticalCharaShake) ? new Vector3(advUI.shakeObj.transform.localPosition.x
                                                                        , charaShakeLength * Mathf.Sin(Time.frameCount * charaShakeSpan)
                                                                        + advUI.shakeObj.transform.localPosition.y
                                                                        , advUI.shakeObj.transform.localPosition.z)
                                                                    : new Vector3(charaShakeLength * Mathf.Sin(Time.frameCount * charaShakeSpan)
                                                                        + advUI.shakeObj.transform.localPosition.x
                                                                        , advUI.shakeObj.transform.localPosition.y
                                                                        , advUI.shakeObj.transform.localPosition.z);

                // ズームフラグが立っていたらズームさせる
                else if (zoomStart && zoomModal)
                {
                    advUI.bgImage[0].transform.localScale = Vector3.Lerp(zoomPosScaleList[1], zoomPosScaleList[3], waitTimer[0] / waitTime[0]);
                    advUI.bgImage[0].transform.position = Vector3.Lerp(zoomPosScaleList[0], zoomPosScaleList[2], waitTimer[0] / waitTime[0]);
                }

                // フェードフラグが立ってたらフェードさせる
                if (fadeStart)
                    advUI.fadePanel.color = Color.Lerp(fadeColorList[0], fadeColorList[1], waitTimer[0] / waitTime[0]);

                // 立ち絵フェードフラグが立ってたらフェードさせる
                if (charaFadeStart)
                    advUI.moveObj.color = Color.Lerp(fadeColorList[0], fadeColorList[1], waitTimer[0] / waitTime[0]);

                
                waitTimer[0] += state == StateReadScenario.WAIT ? Time.deltaTime : 0;
                break;
        }
        // 読み出し速度を初期化
        speed = 0.6f;

        // 移動フラグが立っていたら移動させる
        if ((true == moveStart) && (!moveModal))
        {
            if (waitTimer[1] > waitTime[1])
            {
                waitTimer[1] = 0f;
                moveStart = false;
                movePosList.Clear();
            }
            else
            {
                advUI.moveObj.transform.localPosition = Vector3.Lerp(movePosList[0], movePosList[1], waitTimer[1] / waitTime[1]);
                waitTimer[1] += Time.deltaTime;
            }
        }
        // 振動フラグが立っていたら振動させる
        else if ((shakeStart) && (!shakeModal))
        {
            if (waitTimer[1] > waitTime[1])
            {
                waitTimer[1] = 0f;
                advUI.bgImage[0].transform.localPosition = Vector3.zero;
                shakeStart = false;
            }
            else
            {
                advUI.bgImage[0].transform.localPosition = (verticalShake) ? new Vector3(advUI.bgImage[0].transform.localPosition.x
                                                                        , shakeLength * Mathf.Sin(Time.frameCount * shakeSpan + shakeLength)
                                                                        + advUI.bgImage[0].transform.localPosition.y
                                                                        , advUI.bgImage[0].transform.localPosition.z)
                                                                        : new Vector3(Mathf.Sin(Time.frameCount * shakeSpan + shakeLength)
                                                                        + advUI.bgImage[0].transform.localPosition.x
                                                                        , advUI.bgImage[0].transform.localPosition.y
                                                                        , advUI.bgImage[0].transform.localPosition.z);
                waitTimer[1] += Time.deltaTime;
            }
        }
        // 振動フラグが立っていたら振動させる
        else if ( charaShakeStart.Equals(true) && (false == charaShakeModal))
        {
            if (waitTimer[1] > waitTime[1])
            {
                waitTimer[1] = 0f;
                charaShakeStart = false;
                charaShakePosList.Clear();
            }
            else
            {
                advUI.shakeObj.transform.localPosition = (verticalCharaShake) ? new Vector3(advUI.shakeObj.transform.localPosition.x
                                                                        , charaShakeLength * Mathf.Sin(Time.frameCount * charaShakeSpan)
                                                                        + advUI.shakeObj.transform.localPosition.y
                                                                        , advUI.shakeObj.transform.localPosition.z)
                                                                        : new Vector3(charaShakeLength * Mathf.Sin(Time.frameCount * charaShakeSpan)
                                                                        + advUI.shakeObj.transform.localPosition.x
                                                                        , advUI.shakeObj.transform.localPosition.y
                                                                        , advUI.shakeObj.transform.localPosition.z);
                waitTimer[1] += Time.deltaTime;
            }
        }
        // 移動フラグが立っていたら移動させる
        else if ((true == zoomStart) && (!zoomModal))
        {
            if (waitTimer[1] > waitTime[1])
            {
                waitTimer[1] = 0f;
                zoomStart = false;
                zoomPosScaleList[1] = zoomPosScaleList[3] = Vector3.zero;
                zoomPosScaleList[0] = zoomPosScaleList[2] = Vector3.zero;
            }
            else
            {
                try
                {
                    advUI.bgImage[0].transform.position = Vector3.Lerp(zoomPosScaleList[0], zoomPosScaleList[2], waitTimer[1] / waitTime[1]);
                }
                catch
                {

                }
                
                advUI.bgImage[0].transform.localScale = Vector3.Lerp(zoomPosScaleList[1], zoomPosScaleList[3], waitTimer[1] / waitTime[1]);
                waitTimer[1] += Time.deltaTime;
            }
        }
        return endState;
    }

    /// <summary>
    /// 画面がクリックされたら
    /// </summary>
    public void OnClickDisplay()
    {
        // 読み出し最中のみ速度を上げる
        //speed = (state == StateReadScenario.READ) ? 0.2f : 0.6f;

        if (state == StateReadScenario.STOP && state != StateReadScenario.CHOICE)
        {
            state = StateReadScenario.READ;
        }
        if (state == StateReadScenario.END)
        {
            state = StateReadScenario.NONE;
            endState = true;
        }

        if (moveStart && !moveModal)
        {
            waitTimer[1] = 0f;
            moveStart = false;
            if (movePosList.Count >= 2)
            {
                advUI.moveObj.transform.localPosition = movePosList[1];
            }
            movePosList.Clear();
        }
        if (shakeStart && !shakeModal)
        {
            advUI.bgImage[0].transform.localPosition = Vector3.zero;
            waitTimer[1] = 0f;
            shakeStart = false;
        }
        if (charaShakeStart && !charaShakeModal)
        {
            waitTimer[1] = 0f;
            charaShakeStart = false;
            if (charaShakePosList.Count != 0)
            {
                advUI.shakeObj.transform.localPosition = charaShakePosList[0];
            }
            charaShakePosList.Clear();
        }
        if (zoomStart && !zoomModal)
        {
            waitTimer[1] = 0f;
            zoomStart = false;
            advUI.bgImage[0].transform.localScale = zoomPosScaleList[3];
            advUI.bgImage[0].transform.position = zoomPosScaleList[2];
            zoomPosScaleList[1] = zoomPosScaleList[3] = Vector3.zero;
            zoomPosScaleList[0] = zoomPosScaleList[2] = Vector3.zero;
        }
    }

    /// <summary>
    /// 一行テキストを取得
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <param name="line">一行まとめ</param>
    /// <returns></returns>
    private void CreateLineText(TextAsset txt)
    {
        lineText = txt.text.Split(new char[] { '\n', '\r' }).ToList<string>();
        lineText.RemoveAll(none => none == "\n" || none == "\r" || none == "");
    }

    private void CreateLineText(string txt)
    {
        lineText = txt.Split(new char[] { '\n', '\r' }).ToList<string>();
        lineText.RemoveAll(none => none == "\n" || none == "\r" || none == "");
    }

    /// <summary>
    /// コマンド
    /// </summary>
    /// <param name="command">コマンドデータ</param>
    private void Command(string[] command, ScenarioCommandType type)
    {
        switch (type)
        {
            case ScenarioCommandType.SHARP:

                ScenarioSharpCommand com = StrToSharpCommand(command[0].Substring(1));
                ICommand advCommand = CommandFactory.GetCommand(com);
                switch (com)
                {
                    case ScenarioSharpCommand.BGIMG:
                    case ScenarioSharpCommand.NEXT:
                        advCommand.RUN(command, advUI);
                        break;

                    case ScenarioSharpCommand.DISP:
                    case ScenarioSharpCommand.DISPCLEAR:
                        advCommand.SetData(ref charaObjList, ref charaNameList, ref movePosList, ref fadeColorList, defaultMovePosList);
                        advCommand.RUN(command, advUI);
                        break;

                    case ScenarioSharpCommand.MONOLOGUE:
                        advCommand.RUN(command, advUI);
                        MonologueEnable = (bool)advCommand.END();
                        break;

                    case ScenarioSharpCommand.BGM:
                        CommandBGM bgmCommand = (CommandBGM)advCommand;
                        bgmCommand.RUN(command, bgmObject);
                        break;

                    case ScenarioSharpCommand.SE:
                        CommandSE seCommand = (CommandSE)advCommand;
                        seCommand.RUN(command, seObject);
                        break;

                    case ScenarioSharpCommand.WAIT:
                        CommandWAIT waitCommand = (CommandWAIT)advCommand;
                        waitCommand.RUN(command, ref waitTime);
                        state = (StateReadScenario)waitCommand.END();
                        break;

                    case ScenarioSharpCommand.FADE:
                        // フェード開始
                        fadeStart = true;

                        CommandFADE fadeCommand = (CommandFADE)advCommand;
                        fadeCommand.SetData(ref charaObjList, ref charaNameList, ref movePosList, ref fadeColorList, defaultMovePosList);
                        fadeCommand.RUN(command, ref waitTime);
                        state = (StateReadScenario)fadeCommand.END();
                        break;

                    case ScenarioSharpCommand.CHARAFADE:
                        // フェード開始
                        charaFadeStart = true;

                        CommandCHARAFADE charaFadeCommand = (CommandCHARAFADE)advCommand;
                        charaFadeCommand.SetData(ref charaObjList, ref charaNameList, ref movePosList, ref fadeColorList, defaultMovePosList);
                        charaFadeCommand.RUN(command, advUI, ref waitTime);
                        state = (StateReadScenario)charaFadeCommand.END();
                        break;

                    case ScenarioSharpCommand.MOVE:
                        CommandMOVE moveCommand = (CommandMOVE)advCommand;
                        moveCommand.SetData(ref charaObjList, ref charaNameList, ref movePosList, ref fadeColorList, defaultMovePosList);
                        moveModal = moveCommand.RUN(command, advUI, ref waitTime);
                        moveStart = true;

                        state = (StateReadScenario)moveCommand.END();
                        break;

                    case ScenarioSharpCommand.SHAKE:
                        CommandSHAKE shakeCommand = (CommandSHAKE)advCommand;
                        shakeModal = shakeCommand.RUN(command, advUI, ref waitTime, ref shakeSpan, ref shakeLength, ref verticalShake);
                        shakeStart = true;

                        state = (StateReadScenario)shakeCommand.END();
                        break;

                    case ScenarioSharpCommand.CHARASHAKE:
                        CommandCHARASHAKE charaShakeCommand = (CommandCHARASHAKE)advCommand;
                        charaShakeCommand.SetData(ref charaObjList, ref charaNameList, ref charaShakePosList, ref fadeColorList, defaultMovePosList);
                        charaShakeModal = charaShakeCommand.RUN(command, advUI, ref waitTime, ref charaShakeSpan, ref charaShakeLength, ref verticalCharaShake, ref charaShakePosList);
                        charaShakeStart = true;

                        state = (StateReadScenario)charaShakeCommand.END();
                        break;

                    case ScenarioSharpCommand.FRONTBGCOLOR:
                        //色変更開始
                        if (fadeStart) break;
                        CommandFRONTBGCOLOR frontBGColorCommand = (CommandFRONTBGCOLOR)advCommand;
                        frontBGColorCommand.RUN(command, advUI);
                        break;

                    case ScenarioSharpCommand.BGZOOM:
                        CommandBGZOOM zoomCommand = (CommandBGZOOM)advCommand;
                        zoomCommand.SetData(ref charaObjList, ref charaNameList, ref zoomPosScaleList, ref fadeColorList, defaultMovePosList);
                        zoomModal = zoomCommand.RUN(command, advUI, ref waitTime);
                        zoomStart = true;
                        state = (StateReadScenario)zoomCommand.END();
                        break;

                    case ScenarioSharpCommand.MSG:
                        CommandMSGWIN msgwinCommand = (CommandMSGWIN)advCommand;
                        msgwinCommand.RUN(command, advUI);
                        break;
                }
                break;

            case ScenarioCommandType.LINECOM:
                ILineCommand lineCom = LineCommandFactory.GetCommand(command[0]);
                switch (command[0])
                {
                    // 一時停止
                    case CommandSTOP.LINECOMMAND:
                        lineCom.SetData(ref lineIndex);
                        lineCom.RUN(command, advUI, lineText);
                        state = (StateReadScenario)lineCom.END();
                        break;

                    // 改行
                    case CommandNEW.LINECOMMAND:
                        lineCom.SetData(ref lineIndex);
                        lineCom.RUN(command, advUI, lineText);
                        break;

                    case CommandCOMNEW.LINECOMMAND:
                        // 次の行の#コマンドをここで読む
                        Command(lineText[lineIndex + 1].Split(' '), ScenarioCommandType.SHARP);
                        lineCom.SetData(ref lineIndex);
                        lineCom.RUN(command, advUI, lineText);
                        break;

                    // 色替えコマンド
                    case CommandCOLOR.LINECOMMAND:
                        lineCom.SetData(ref lineIndex);
                        lineCom.RUN(command, advUI, lineText);
                        break;

                    case CommandNEXTWORD.LINECOMMAND:
                        lineCom.SetData(ref lineIndex);
                        lineCom.RUN(command, advUI, lineText);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// #コマンドを列挙型に変換する
    /// </summary>
    /// <param name="command">コマンド文字列</param>
    /// <returns>コマンド列挙タイプ</returns>
    private ScenarioSharpCommand StrToSharpCommand(String command)
    {
        Debug.Log(command);
        ScenarioSharpCommand com = ScenarioSharpCommand.NONE;
        try
        {
            com = CommonUtil.ParseEnum<ScenarioSharpCommand>(command.ToUpper());
        }
        catch (Exception) { }
        return com;
    }

    /// <summary>
    /// 改行文字コードを判定する
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private bool IsNewLine(string str) {
        return Regex.IsMatch(str, "\r") || Regex.IsMatch(str, "\n") || Regex.IsMatch(str, "\r\n");
    }
}
