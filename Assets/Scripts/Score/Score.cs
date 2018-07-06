using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public Text singleScoreText;
    public static int score;
    public const int receiptScoreMultiplier = 10;

    public ScorePresenter scorePresenter;

    private void Start()
    {
        
    }

    void Update()
    {

    }
}
