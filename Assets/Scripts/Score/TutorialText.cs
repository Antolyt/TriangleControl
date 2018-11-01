using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

    public string text;
    public Text textBox;

    private void OnEnable()
    {
        textBox.text = text;
    }
}
