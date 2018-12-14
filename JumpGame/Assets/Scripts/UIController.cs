using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonBehavior<UIController> {

    public GameObject storePowerSliderObj;   //蓄力条
    public GameObject scoreTextObj;  //分数显示框
    public GameObject infoTextObj;   //消息提示框

    Slider storePowerSlider;
    Text scoreText;
    Text infoText;

    void Start ()
    {
        storePowerSlider = storePowerSliderObj.GetComponent<Slider>();
        scoreText = scoreTextObj.GetComponent<Text>();
        infoText = infoTextObj.GetComponent<Text>();
	}

    public void StorePower(float power)
    {
        storePowerSlider.value = power;
    }

    public void ShowScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowInfo(string text)
    {
        infoText.text = text;
    }
}
