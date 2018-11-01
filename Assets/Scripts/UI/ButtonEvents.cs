using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour {

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

	public void Close()
    {
        Application.Quit();
    }

    public void SetLevelInPlayerOptions(string level)
    {
        PlayerOptions.level = level;
    }

    public void TutorialButtonClick(Object element)
    {
        GameObject go = element as GameObject;
        foreach (Transform child in go.transform.parent.GetComponentInChildren<Transform>())
        {
            child.gameObject.SetActive(false);
        }
        go.SetActive(true);

        MarkLines markLines = go.GetComponent<MarkLines>();
        if(markLines)
            go.GetComponent<MarkLines>().Reset();
    }
}
