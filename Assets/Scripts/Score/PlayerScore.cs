using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public Text playerName;
    public Image background;
    public Image filled;
    public Text score;

	public void SetColor(Color color)
    {
        filled.color = color;
    }

    public void UpdateScore(float value, float max)
    {
        filled.rectTransform.sizeDelta = new Vector2(value * background.rectTransform.sizeDelta.x / max, background.rectTransform.sizeDelta.y);
        score.text = value.ToString();
    }
}
