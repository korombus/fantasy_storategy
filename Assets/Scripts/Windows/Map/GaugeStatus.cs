using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GaugeStatus : MonoBehaviour
{
    public float speed = 0.05f;
    public bool gauge_start = false;
    public Slider gauge_slider;
    public TextMeshProUGUI gauge_text;

    private Func<bool> m_func = null;

    public void SetData(Func<bool> i_func, string message){
        gauge_start = true;
        gauge_text.text = message;
        m_func = i_func;
        gauge_slider.gameObject.SetActive(true);
    }

    public void StopGauge(){
        gauge_start = false;
        gauge_text.text = "";
        m_func = null;
        gauge_slider.gameObject.SetActive(false);
        gauge_slider.value = 0;
    }

    void Update(){
        if(gauge_start){
            
            gauge_slider.value += speed * Time.deltaTime;

            if(gauge_slider.value >= 1){
                if(m_func != null){
                    m_func();
                }
                gauge_start = false;
            }
        }
    }
}
