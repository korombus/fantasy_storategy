using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicOption : MonoBehaviour
{
    public OptionWindow parent; //!< 親クラス
    public Slider bgmSlider;    //!< BGM用スライダー
    public TMP_Text bgmValueLabel;  //!< BGM音量表示ラベル
    public Slider seSlider;     //!< SE用スライダー
    public TMP_Text seValueLabel;   //!< SE音量表示ラベル

    public void SetData(float bgmVolume, float seVolume){
        bgmSlider.value = bgmVolume * 10;
        seSlider.value = seVolume * 10;

        bgmValueLabel.text = (bgmVolume * 100).ToString();
        seValueLabel.text = (seVolume * 100).ToString();
    }

    public void OnChangeBGMVolume(){
        // 音量変更を反映
        parent.optionOrigin.option.SetVolume(bgmSlider.value == 0 ? 0 : bgmSlider.value / 10, OptionBase.Sound.BGM, parent.optionOrigin.bgm);
        // 現在の音量数値を表示
        bgmValueLabel.text = (bgmSlider.value * 10).ToString();
    }

    public void OnChangeSEVolume(){
        // 音量変更を反映
        parent.optionOrigin.option.SetVolume(seSlider.value == 0 ? 0 : seSlider.value / 10, OptionBase.Sound.SE, parent.optionOrigin.se);
        // 現在の音量数値を表示
        seValueLabel.text = (seSlider.value * 10).ToString();
    }
}
