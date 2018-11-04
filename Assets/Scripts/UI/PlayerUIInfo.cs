using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIInfo : MonoBehaviour {

    public Image playerColor;
    public float aPlayerColor;
    public Image activeColor;
    public float aActiveColor;
    public int controller;
    public Text playerName;
    public Text score;

	public void SetActivePlayerColor(bool active)
    {
        if (active)
        {
            activeColor.color = new Color(1,1,1,aActiveColor);
        }
        else
        {
            activeColor.color = new Color(0, 0, 0, aActiveColor);
        }
    }
}
