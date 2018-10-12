using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public new Camera camera;
    public TriangleComplex triangleComplex;

    public int activePlayer = 0;
    [HideInInspector]public int numberOfPlayers = 0;
    public PlayerUIInfo[] players;
    public float cursorSpeed;
    public GameObject[] playerCursors;
    public Color lineColor;

    public delegate void UpdateMode(Line line, PlayerUIInfo[] players, int numberOfPlayers, int activePlayer);
    public UpdateMode updateMode;

    private void Start()
    {
        //updateMode = UpdateMode_SingleTriangle;
        updateMode = UpdateMode_MultipleTriangleWithoutOverride;
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if (PlayerOptions.playerConfig[i].controller >= 0)
            {
                players[i].background.color = PlayerOptions.playerConfig[i].color;
                playerCursors[i].GetComponent<SpriteRenderer>().color = PlayerOptions.playerConfig[i].color;
                numberOfPlayers++;
            }
            else
            {
                players[i].gameObject.SetActive(false);
            }
        }

        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;
        for (int i = 0; i < players.Length; i++)
        {
            r += players[i].background.color.r;
            g += players[i].background.color.g;
            b += players[i].background.color.b;
            a += players[i].background.color.a;
        }

        r /= players.Length;
        g /= players.Length;
        b /= players.Length;
        a /= players.Length;

        lineColor = new Color(r, g, b, a);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Action" + players[activePlayer].controller))
        {
            Vector3 pos = Vector3.zero;
            if(Input.GetKeyDown(KeyCode.Mouse0))
                pos = camera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * camera.transform.position.z;
            if(Input.GetButtonDown("Action" + players[activePlayer].controller))
                pos = playerCursors[activePlayer].transform.position;
            Line nearestLine = GetNearestLine(pos);

            if (nearestLine.controllingPlayer < 0)
            {
                nearestLine.controllingPlayer = activePlayer;
                nearestLine.gameObject.GetComponent<SpriteRenderer>().color = lineColor;

                updateMode(nearestLine, players, numberOfPlayers, activePlayer);
            }
        }

        // Update Player cursors
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if (PlayerOptions.playerConfig[i].controller >= 0)
            {
                playerCursors[i].transform.position += cursorSpeed * Input.GetAxis("Horizontal" + PlayerOptions.playerConfig[i].controller) * Vector3.right + cursorSpeed * Input.GetAxis("Vertical" + PlayerOptions.playerConfig[i].controller) * Vector3.down;
                playerCursors[i].transform.position = new Vector3(Math.Min(PlayerOptions.horzExtent, Math.Max(-PlayerOptions.horzExtent, playerCursors[i].transform.position.x)), Math.Min(PlayerOptions.vertExtent, Math.Max(-PlayerOptions.vertExtent, playerCursors[i].transform.position.y)), 0);
            }
        }
    }

    public Line GetNearestLine(Vector3 pos)
    {
        Line target = null;
        float minDis = float.MaxValue;
        foreach(Line line in triangleComplex.lines)
        {
            if (line == null || !line.gameObject.activeSelf)
                continue;

            //ToDO effizienter
            Vector3 tPos = line.transform.position + line.GetCenter();
            float tmpDis = Vector3.Distance(pos, tPos);
            if (tmpDis <= minDis)
            {
                minDis = tmpDis;
                target = line;
            }
        }

        return target;
    }

    public static void UpdateMode_SingleTriangle(Line updatedLine, PlayerUIInfo[] players, int numberOfPlayers, int activePlayer)
    {
        foreach(Triangle triangle in updatedLine.triangles)
        {
            if(triangle == null)
            {
                continue;
            }

            bool allLinesControlled = true;
            foreach(Line line in triangle.lines)
            {
                if(line.controllingPlayer != activePlayer)
                {
                    allLinesControlled = false;
                    break;
                }
            }

            if (allLinesControlled)
            {
                triangle.spriteRenderer.color = PlayerOptions.playerConfig[activePlayer].color;
            }
        }
    }

    public static void UpdateMode_MultipleTriangleWithoutOverride(Line updatedLine, PlayerUIInfo[] players, int numberOfPlayers, int activePlayer)
    {
        foreach(Triangle triangle in updatedLine.triangles)
        {
            if(triangle == null)
            {
                continue;
            }

            Queue<Triangle> remainingTriangles = new Queue<Triangle>();
            remainingTriangles.Enqueue(triangle);
            List<Triangle> checkedTriangles = new List<Triangle>();
            List<Line> checkedLines = new List<Line>();

            bool isBordered = true;
            while (remainingTriangles.Count > 0)
            {
                Triangle currentTriangle = remainingTriangles.Dequeue();
                checkedTriangles.Add(currentTriangle);
                foreach(Line line in currentTriangle.lines)
                {
                    if(checkedLines.Contains(line))
                    {
                        continue;
                    }
                    if(line.IsOuterLine() && line.controllingPlayer != activePlayer)
                    {
                        isBordered = false;
                        break;
                    }
                    if(line.controllingPlayer == activePlayer)
                    {
                        checkedLines.Add(line);
                        continue;
                    }
                    else
                    {
                        foreach(Triangle t in line.triangles)
                        {
                            if(t == null || checkedTriangles.Contains(t) || remainingTriangles.Contains(t))
                            {
                                continue;
                            }
                            else
                            {
                                remainingTriangles.Enqueue(t);
                            }
                        }
                    }
                }
            }

            if(isBordered)
            {
                foreach(Triangle t in checkedTriangles)
                {
                    t.spriteRenderer.color = PlayerOptions.playerConfig[activePlayer].color;
                }
            }
        }
    }

    public static void UpdateMode_MultipleTriangleWithOverride(Line updatedLine, PlayerUIInfo[] players, int numberOfPlayers, int activePlayer)
    {

    }

    public static void TriangleWasFilled()
    {
        
    }
}


[Serializable]
public struct PlayerConfig
{
    public Color color;
    public int controller;
    public Text name;
    public Text score;
}
