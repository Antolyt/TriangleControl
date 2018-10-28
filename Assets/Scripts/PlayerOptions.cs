using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptions : MonoBehaviour {

    public static PlayerOptions instance { get; set; }
    public static int playerCount;
    public static PlayerConfig[] playerConfig;
    public static string level;

    public static float vertExtent;
    public static float horzExtent;

    // Use this for initialization
    void Awake () {
        if(!instance)
        {
            DontDestroyOnLoad(this);
            playerConfig = new PlayerConfig[4];
            for (int i = 0; i < playerConfig.Length; i++)
            {
                playerConfig[i].controller = -1;
            }
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public int GetControllerFromKeyCode(Event e)
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (j > 0)
                {
                    if (Input.GetKey("joystick " + j + " button " + i))
                    { return j; }
                }
            }
        }
        return -1;
    }
}
