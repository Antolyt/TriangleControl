using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScorePresenter : MonoBehaviour {

    public Text scoreForFoodText;
    public float foodTime;
    public Text scoreForKilledZombiesText;
    public float killedZombieTime;
    public Text scoreForZombiesText;
    public float zombieTime;
    public Text scoreComplete;
    public float completeTime;

    private int step = 0;
    private float progressedTime = 0;

    public int scoreForFood;
    public int scoreForKilledZombies;
    public int numberOfZombies;
    public int zombiePointMultiplier;
    private int completeScore;

    public int waitTime;
    private float timeStemp;
    public UnityEvent action;

    void Start () {
        completeScore = scoreForFood + scoreForKilledZombies + numberOfZombies * zombiePointMultiplier;
        timeStemp = Time.time;
	}
	
    /// <summary>
    /// Go to next UI after input and time offset
    /// </summary>
	void Update () {

        float t;
        switch (step)
        {
            // Score for food
            case 0:
                t = Mathf.Min(progressedTime / foodTime, 1);
                scoreForFoodText.text = ((int)Mathf.Lerp(0, scoreForFood, t)).ToString();
                if (t >= 1)
                {
                    progressedTime = 0;
                    step++;
                }
                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit"))
                {
                    scoreForFoodText.text = scoreForFood.ToString();
                    progressedTime = 0;
                    step++;
                }
                break;
            // Score for killed zombies
            case 1:
                t = Mathf.Min(progressedTime / killedZombieTime, 1);
                scoreForZombiesText.text = ((int)Mathf.Lerp(0, scoreForKilledZombies, t)).ToString();
                if (t >= 1)
                {
                    progressedTime = 0;
                    step++;
                }
                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit"))
                {
                    scoreForZombiesText.text = scoreForKilledZombies.ToString();
                    progressedTime = 0;
                    step++;
                }
                break;
            // Score for catched zombies
            case 2:
                t = Mathf.Min(progressedTime / zombieTime, 1);
                scoreForFoodText.text = ((int)Mathf.Lerp(0, numberOfZombies * zombiePointMultiplier, t)).ToString();
                if (t >= 1)
                {
                    progressedTime = 0;
                    step++;
                }
                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit"))
                {
                    scoreForFoodText.text = (numberOfZombies * zombiePointMultiplier).ToString();
                    progressedTime = 0;
                    step++;
                }
                break;
            // Sum of Scores
            case 3:
                t = Mathf.Min(progressedTime / completeTime, 1);
                scoreComplete.text = ((int)Mathf.Lerp(0, completeScore, t)).ToString();
                if (t >= 1)
                {
                    progressedTime = 0;
                    step++;
                }
                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit")) {
                    scoreComplete.text = completeScore.ToString();
                    progressedTime = 0;
                    step++;
                }
                break;
            case 4:
                if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit")) && Time.time > timeStemp + waitTime)
                {
                    if (action != null) action.Invoke();
                    return;
                }
                break;
        }
        progressedTime += Time.deltaTime;
	}
}
