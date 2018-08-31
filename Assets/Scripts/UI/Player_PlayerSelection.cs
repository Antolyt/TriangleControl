using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_PlayerSelection : MonoBehaviour {

    public GameObject playerAdd;
    public GameObject selector;
    public GameObject color;
    [HideInInspector]public Image colorImage;
    public GameObject readyText;
    public bool ready;
    public GameObject arrow_left;
    public GameObject arrow_right;

    public void Start()
    {
        colorImage = color.GetComponent<Image>();
    }

    public void SetActive(bool active)
    {
        playerAdd.SetActive(!active);
        selector.SetActive(active);
    }

    public void SetReady(bool ready)
    {
        this.readyText.SetActive(ready);
        this.ready = ready;
        arrow_left.SetActive(!ready);
        arrow_right.SetActive(!ready);
    }
}
