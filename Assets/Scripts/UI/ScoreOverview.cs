using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOverview : MonoBehaviour {

    public PlayerScore[] playerScores;

    private int maxScore = 0;
    private int currentScore = 0;

    float time;
    public float updateSpeed;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if(PlayerOptions.playerConfig[i].controller >= 0)
            {
                int tmpScore = int.Parse(PlayerOptions.playerConfig[i].score.text);
                if (maxScore < tmpScore)
                {
                    maxScore = tmpScore;
                }
                playerScores[i].filled.color = PlayerOptions.playerConfig[i].color;
            }
            else
            {
                playerScores[i].gameObject.SetActive(false);
            }
        }
        time = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
	    if(currentScore <= maxScore && time + updateSpeed <= Time.time)
        {
            for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
            {
                if (PlayerOptions.playerConfig[i].controller >= 0 && currentScore <= int.Parse(PlayerOptions.playerConfig[i].score.text))
                {
                    playerScores[i].UpdateScore(currentScore, maxScore);
                }
            }
            currentScore++;

            time += updateSpeed;
        }
	}
}
