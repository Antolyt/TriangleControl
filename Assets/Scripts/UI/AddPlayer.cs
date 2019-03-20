using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddPlayer : MonoBehaviour {

    public Player_PlayerSelection[] players;

    private int[] controllerPlayerMatch;
    private ColorOfPlayer cop;

    float[] timeStamp;
    public const float timeOffset = 0.25f;

    public string sceneAfterSelection;
    public string sceneByCancel;
    public CancelTimer cancelTimer;

    public SoundInterface soundInterface;

	// Use this for initialization
	void Start () {
		controllerPlayerMatch = new int[4]{ -1, -1, -1, -1 };
        timeStamp = new float[4];

        cop = this.transform.GetComponent<ColorOfPlayer>();

        PlayerOptions.playerConfig = new PlayerConfig[4];
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            PlayerOptions.playerConfig[i].controller = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (controllerPlayerMatch[i] < 0)
            {
                // Match Controller & Set corresponding Player active
                if (Input.GetButtonDown("Action" + i))
                {
                    int playerIndex = GetFirstFreePlayer();
                    controllerPlayerMatch[i] = playerIndex;
                    PlayerOptions.playerConfig[playerIndex].controller = i;
                    players[playerIndex].SetActive(true);
                    int color = cop.SetFirstFreeColor(playerIndex);
                    if (color >= 0)
                        players[playerIndex].colorImage.color = cop.playerColor[color].color;
                    timeStamp[i] = Time.time;
                    PlaySound("submit");
                }

                CancelSelection(i);
            }
            else
            {
                if (!players[controllerPlayerMatch[i]].ready)
                {
                    // Change Color
                    if (timeStamp[i] + timeOffset < Time.time)
                    {
                        if (Input.GetAxis("Horizontal" + i) < 0)
                        {
                            int color = cop.DecreasePlayerColor(controllerPlayerMatch[i]);
                            if (color >= 0)
                                players[controllerPlayerMatch[i]].colorImage.color = cop.playerColor[color].color;
                            players[controllerPlayerMatch[i]].AnimateLeftArrow();
                            timeStamp[i] = Time.time;
                            PlaySound("selectionChange");
                        }
                        if (Input.GetAxis("Horizontal" + i) > 0)
                        {
                            int color = cop.IncreasePlayerColor(controllerPlayerMatch[i]);
                            if (color >= 0)
                                players[controllerPlayerMatch[i]].colorImage.color = cop.playerColor[color].color;
                            players[controllerPlayerMatch[i]].AnimateRightArrow();
                            timeStamp[i] = Time.time;
                            PlaySound("selectionChange");
                        }
                    }

                    // Submit selection
                    if (Input.GetButtonDown("Submit" + i) || Input.GetButtonDown("Action" + i))
                    {
                        players[controllerPlayerMatch[i]].SetReady(true);
                        PlaySound("submit");

                        
                    }

                    // Go to MainMenu
                    CancelSelection(i);
                }
                else
                {
                    // Cancel selection submission
                    if (Input.GetButtonDown("Cancel" + i))
                    {
                        players[controllerPlayerMatch[i]].SetReady(false);
                    }

                    // Submit selection
                    if (Input.GetButtonDown("Submit" + i) || Input.GetButtonDown("Action" + i))
                    {
                        bool allPlayersReady = true;
                        foreach (Player_PlayerSelection player in players)
                        {
                            if (!player.ready && player.IsActiv())
                                allPlayersReady = false;
                        }
                        if (allPlayersReady)
                        {
                            for (int j = 0; j < players.Length; j++)
                            {
                                if (controllerPlayerMatch[j] >= 0 && players[controllerPlayerMatch[j]].IsActiv())
                                    PlayerOptions.playerConfig[controllerPlayerMatch[j]].color = players[controllerPlayerMatch[j]].colorImage.color;
                            }
                        }
                        PlaySound("submit");
                        SceneManager.LoadScene(sceneAfterSelection);
                    }
                }
            }
            
        }
    }

    public int GetFirstFreePlayer()
    {
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if (PlayerOptions.playerConfig[i].controller < 0)
            {
                return i;
            }
        }
        return -1;
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

    public bool AllPlayerReady()
    {
        bool allready = true;
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i].ready)
            {

            }
        }
    }

    public void CancelSelection(int controller)
    {
        cancelTimer.DoAction(controller);
    }

    public void PlaySound(string name)
    {
        soundInterface.PlaySound(name);
    }
}
