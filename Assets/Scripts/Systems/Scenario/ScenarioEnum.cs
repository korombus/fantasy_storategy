/// <summary>
/// 演算子
/// </summary>
public enum Operator
{
    NONE,   // 不正
    PLUS,   // 足し算
    MINE,   // 引き算
    MULT,   // 掛け算
    DIVI,   // 割り算
}

/// <summary>
/// ステージの状態
/// </summary>
/*
public enum Stage
{
    NONE = 0,   // 不正データ
    TITLE,      
    STAGE1
}
*/
/// <summary>
/// シナリオ読み出しの状態
/// </summary>
public enum StateReadScenario
{
    NONE,   // 何もなし
    BEGINE, // 始まり
    READ,   // 読み出し中
    STOP,   // 停止
    END,    // 終了
    WAIT,   // 一時停止
    CHOICE  // 選択肢表示
}

/// <summary>
/// シナリオのコマンドタイプ
/// </summary>
public enum ScenarioCommandType
{
    NONE,       // 不正
    SHARP,      // #
    LINECOM,    // []内コマンド
}

public enum ScenarioSharpCommand
{
    NONE,       // 不正
    DISP,       // 画像表示
    DISPCLEAR,  // 画像非表示
    BGM,        // BGM再生
    SE,         // SE再生
    BGIMG,      // 背景画像設定
    FADEBGM,    // フェードBGM専用コマンド
    WAIT,       // ウェイトコマンド
    NEXT,       // 次のシーンを指定
    FADE,       // 背景のフェードイン/フェードアウトコマンド
    CHARAFADE,  // 立ち絵のフェードイン/フェードアウトコマンド
    MOVE,       // 立ち絵移動コマンド
    MONOLOGUE,  // モノローグコマンド
    SHAKE,      // 画面振動コマンド
    CHARASHAKE, // 立ち絵振動コマンド
    FRONTBGCOLOR,// 画面全体を塗りつぶすコマンド
    BGZOOM,      // 背景ズームコマンド
    MSG,         // メッセージウィンドウの表示切り替えコマンド
    CHOICE       // 選択肢コマンド
}