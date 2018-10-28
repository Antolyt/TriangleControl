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
}
