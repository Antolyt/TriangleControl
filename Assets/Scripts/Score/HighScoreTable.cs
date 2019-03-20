using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HighScoreTable : MonoBehaviour {

    public NameScore[] nameScores;
    public GameObject inputField;
    public Text inputFieldName;
    public EventSystem es;

    /// <summary>
    /// Player is able to enter his name in scoreTable or not
    /// </summary>
    public bool interactible = true;

    public UnityEvent actionOnSubmit;

    /// <summary>
    /// Setup high score table
    /// </summary>
    void Start()
    {
        bool inputPlaced = false;

        if (!interactible)
        {
            inputField.SetActive(false);
            inputPlaced = true;
        }

        // foreach Textfield add score and name
        for(int i = 0, j = 0; i < nameScores.Length && j < nameScores.Length; i++)
        {
            // check if score in savefile exist
            if(j < SaveScore.scoreData.scoreNames.Count)
            {
                // place inputField if current score > score in savefile
                if(!inputPlaced && SaveScore.currentScore >= SaveScore.scoreData.scoreNames[i].score)
                {
                    inputField.transform.position = nameScores[i].name.transform.position;
                    es.SetSelectedGameObject(inputField);
                    nameScores[i].name.text = "";
                    //nameScores[i].score.text = Score.score.ToString();
                    inputPlaced = true;
                }
                // write score from savefile
                else
                {
                    nameScores[i].name.text = SaveScore.scoreData.scoreNames[j].name;
                    nameScores[i].score.text = SaveScore.scoreData.scoreNames[j].score.ToString();
                    j++;
                }
            }
            // if Textfields > savefile entries, place inputField on last position
            else if(!inputPlaced)
            {
                inputField.transform.position = nameScores[i].name.transform.position;
                es.SetSelectedGameObject(inputField);
                nameScores[i].name.text = "";
                //nameScores[i].score.text = Score.score.ToString();
                inputPlaced = true;
                break;
            }
        }

        // if savefile entries > TextFields and savefile scores > current score, don't place inputField
        if (!inputPlaced)
        {
            inputField.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if(interactible) SaveScore.AddPlayerToScoreTable(inputFieldName.text);
            if (actionOnSubmit != null) actionOnSubmit.Invoke();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            es.SetSelectedGameObject(inputField);
        }
    }
}

[Serializable]
public struct NameScore
{
    public Text name;
    public Text score;
}
