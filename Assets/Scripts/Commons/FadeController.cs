using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public enum FADE_STATE { IN,OUT }; //!< フェード遷移制御ENUM
    private bool isFade = false;    //!< フェード状態管理用フラグ
    private bool reControllAfterFade = false; //!< フェード後に操作系統を戻すかどうか
    private FADE_STATE fadeState = FADE_STATE.IN;  //!< 現在のフェード状態制御
    private float fadeSpeed = 0.5f;
    private CommonSys windowSystem = null;

    public RawImage fadeBody;

    public void SetData(FADE_STATE state, CommonSys sys, bool reController){
        isFade = true;
        fadeState = state;
        reControllAfterFade = reController;
        windowSystem = sys;
    }

    void LateUpdate(){
        if(isFade){
            switch(fadeState){
                case FADE_STATE.IN:
                    fadeBody.color -= new Color(0,0,0,Time.deltaTime * fadeSpeed);
                    if(fadeBody.color.a < 0){
                        isFade = false;
                        windowSystem.FADE_COMPLETE = true;
                        windowSystem.reControllAfterFade = reControllAfterFade;
                    }
                break;
                case FADE_STATE.OUT:
                    fadeBody.color += new Color(0,0,0,Time.deltaTime * fadeSpeed);
                    if(fadeBody.color.a > 1){
                        isFade = false;
                        windowSystem.FADE_COMPLETE = true;
                        windowSystem.reControllAfterFade = reControllAfterFade;
                    }
                break;
            }
        }
    }
}
