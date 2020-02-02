using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public float timer = 0f;  //panel timer

    public Text timerText;

    // Start is called before the first frame update
    void Start() {

        timerText.text = "timer: 00:00";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer = Mathf.Clamp(timer, 0f, Mathf.Infinity);
        timerText.text = "timer: " + string.Format("{0:00.00}", timer);
    }
}
