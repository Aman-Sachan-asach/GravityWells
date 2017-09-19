using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text counterText;
    public float minutes, seconds;

	// Use this for initialization
	void Start ()
    {
        counterText = GetComponent<Text>() as Text;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float _time = Time.timeSinceLevelLoad;
        minutes = (int)(_time/60.0f);
        seconds = (int)(_time%60.0f);
        counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
