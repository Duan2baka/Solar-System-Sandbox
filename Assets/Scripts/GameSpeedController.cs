using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Slider speedSlider;
    public Text text;
    private void Start() {
        speedSlider.onValueChanged.AddListener(delegate {ValueChangeCheck ();});
        speedSlider.value = 1;
        
    }
    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    private void Update() {
        text.text = "Time scale: " + speedSlider.value;
    }

    public void ValueChangeCheck(){
        SetGameSpeed(speedSlider.value);
    }
}