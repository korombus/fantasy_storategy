using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBGZOOM : ICommand {

    private List<Vector3> zoomPosList;
    private List<Vector3> defaultList;
    private bool zoomModal;

    public override void SetData(ref Dictionary<string, Image> charaObjList, ref Dictionary<string, string> charaNameList, ref List<Vector3> zoomPosList, ref List<Color> fadeColorList, List<Vector3> defaultzoomPosList) {
        this.zoomPosList = zoomPosList;
        this.defaultList = defaultzoomPosList;
    }

    public override void RUN(string[] command, ReadScenario.ADVUI advUI) {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 「＃BGZOOM IN 50( ズーム率)　100(目標位置X座標) 200(目標位置Y座標)　３(目標位置/拡大率までの秒数)」
    /// </summary>
    /// <param name="command"></param>
    /// <param name="advUI"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public bool RUN(string[] command, ReadScenario.ADVUI advUI, ref float[] waitTime) {
        zoomPosList.Clear();                                    //移動座標初期化
        if (command.Length <= 1) return false;                  //パラメータ有り無し

        //背景イメージがなければ処理を抜ける。
        if (advUI.bgImage == null) return false;


        Vector3 startpos = Vector3.zero;                            //開始位置
        Vector3 targetpos = Vector3.zero;                           //目標位置
        Vector3 startscale = new Vector3(1.0f, 1.0f, 1.0f);         //開始スケール
        Vector3 targetscale = new Vector3(1.0f, 1.0f, 1.0f);        //目標スケール
        string zoomTime = "1";

        startpos = advUI.bgImage[0].transform.position;                //移動前位置
        startscale = advUI.bgImage[0].transform.localScale;            //移動前スケール

        //インのみパラメータを読み込む　アウトは時間のみ
        if (command[1].ToUpper().Contains("IN"))
        { 
            if (command.Length < 5)
            {
                //スケーリング
                float scalevalue = 1.0f;                                    //スケール率
                if (!float.TryParse(command[2], out scalevalue)) return false;
                zoomTime = command.Length > 3 ? command[3] : "1";    //移動時間
            }
            else if (command.Length > 5)
            {
                //スケーリング
                float scalevalue = 1.0f;                                    //スケール率
                if (!float.TryParse(command[2], out scalevalue)) return false;
                //移動
                zoomTime = command.Length > 5 ? command[5] : "1";    //移動時間

                targetscale = new Vector3(startscale.x * scalevalue, startscale.y * scalevalue, startscale.z * scalevalue);   //ターゲットのスケール
                targetpos = new Vector3(float.Parse(command[3]), float.Parse(command[4]), startpos.z);                  //ターゲット位置
            }
        }
        else
        {
            targetpos = defaultList[3];
            zoomTime = command.Length > 2 ? command[2] : "1";    //移動時間
        }

        //目標位置格納
        zoomPosList.Add(startpos);
        zoomPosList.Add(startscale);
        zoomPosList.Add(targetpos);
        zoomPosList.Add(targetscale);

        //モーダル設定
        if (true == command[1].ToUpper().StartsWith("M")) {
            //移動時間設定（デフォルト1秒）
            waitTime[0] = float.Parse(zoomTime);
            //モーダル/モードレス判定、コマンド実行
            zoomModal = true;
        } else {
            //移動時間設定（デフォルト1秒）
            waitTime[1] = float.Parse(zoomTime);
            //モーダル/モードレス判定、コマンド実行
            zoomModal = false;
        }
        return zoomModal;
    }

    public override object END() {
        return zoomModal ? StateReadScenario.WAIT : StateReadScenario.READ;
    }
}
