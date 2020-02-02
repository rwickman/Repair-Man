using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int curScore;

    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("ScoreUI").GetComponent<Text>();
    }


    public void UpdateScore(int points)
    {
        curScore += points;
        scoreText.text = "Score: " + curScore;
    }
}
