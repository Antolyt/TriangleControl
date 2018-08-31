using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOfPlayer : MonoBehaviour {

    public PlayerColor[] playerColor;

    public void Start()
    {
        for (int i = 0; i < playerColor.Length; i++)
        {
            playerColor[i].assignedPlayer = -1;
        }
    }

    public int SetFirstFreeColor(int player)
    {
        for (int i = 0; i < playerColor.Length; i++)
        {
            if (playerColor[i].assignedPlayer < 0)
            {
                playerColor[i].assignedPlayer = player;
                return i;
            }
        }
        return -1;
    }

    public int DecreasePlayerColor(int player)
    {
        for (int i = 0; i < playerColor.Length; i++)
        {
            if(playerColor[i].assignedPlayer == player)
            {
                int previousColor = (i + playerColor.Length - 1) % playerColor.Length;
                while (playerColor[previousColor].assignedPlayer >= 0)
                {
                    previousColor = (previousColor + playerColor.Length - 1) % playerColor.Length;
                }
                playerColor[i].assignedPlayer = -1;
                playerColor[previousColor].assignedPlayer = player;
                return previousColor;
            }
        }

        return -1;
    }

    public int IncreasePlayerColor(int player)
    {
        for (int i = 0; i < playerColor.Length; i++)
        {
            if(playerColor[i].assignedPlayer == player)
            {
                int previousColor = (i + 1) % playerColor.Length;
                while (playerColor[previousColor].assignedPlayer >= 0)
                {
                    previousColor = (previousColor + 1) % playerColor.Length;
                }
                playerColor[i].assignedPlayer = -1;
                playerColor[previousColor].assignedPlayer = player;
                return previousColor;
            }
        }

        return -1;
    }
}

[Serializable]
public struct PlayerColor
{
    public Color color;
    public int assignedPlayer;
}
