using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private int score;
    private int scaling = 9;
    private TextMeshProUGUI text;

    void Awake()
    {       
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.OnCubeSpawn += IncreseScore;
    }

    private void OnDestroy() 
    {
        GameManager.OnCubeSpawn -= IncreseScore;
    }

    private void IncreseScore()
    {
        score++;
        text.text = "Score:" + (score -1);
    }

    public bool IsDivisible()
    {
        return score != 0 && score % scaling == 0;
    }
}
