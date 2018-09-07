using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreUI, HighscoreUI;
    private int score;
    private int highscore;

    void Start()
    {
        ScoreUI.text = "Score: " + score;
        highscore = PlayerPrefs.GetInt("Highscore");
        HighscoreUI.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
    }

    public void GiveScore(int x)
    {
        score += x + Player.instance.combo.currentCombo;
        UIUpdate();
    }

    public void SetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", score);
    }

    private void UIUpdate()
    {
        ScoreUI.text = "Score: " + score;
        if (score > highscore)
        {
            HighscoreUI.text = "Highscore: " + score;
        }
    }
}