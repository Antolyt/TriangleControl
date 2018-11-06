using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public new Camera camera;
    public SoundInterface soundInterface;
    public TriangleComplex triangleComplex;
    public CountDown countDown;
    [Space]
    public int activePlayer = 0;
    [HideInInspector]
    public int numberOfPlayers = 0;
    public PlayerUIInfo[] players;
    public float cursorSpeed;
    public float cursorCap;         // Between 0 and 1
    public GameObject[] playerCursors;
    public Color lineColor;
    public Line[] selectedLines;

    public Vector3 cap;
    public Vector3 tilt;

    public delegate void UpdateMode(Line line, Player player);
    public UpdateMode updateMode;

    public UnityEvent actionOnMatchEnd;

    private void Start()
    {
        //updateMode = UpdateMode_SingleTriangle;
        updateMode = UpdateMode_MultipleTriangleWithoutOverride;
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if (PlayerOptions.playerConfig[i].controller >= 0)
            {
                if(i == 0)
                {
                    players[i].SetActivePlayerColor(true);
                }

                Color tmpC = PlayerOptions.playerConfig[i].color;
                players[i].playerColor.color = new Color(tmpC.r, tmpC.g, tmpC.b, players[i].aPlayerColor);
                playerCursors[i].GetComponent<SpriteRenderer>().color = PlayerOptions.playerConfig[i].color;
                playerCursors[i].SetActive(true);
                PlayerOptions.playerConfig[i].score = players[i].score;
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
            r += players[i].playerColor.color.r;
            g += players[i].playerColor.color.g;
            b += players[i].playerColor.color.b;
            a += players[i].playerColor.color.a;
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
        UpdateCursor(activePlayer);

        // Set roll over color
        for (int i = 0; i < selectedLines.Length; i++)
        {
            if (selectedLines[i] == null)
                continue;

            float r = selectedLines[i].controllerColor.r;
            float g = selectedLines[i].controllerColor.g;
            float b = selectedLines[i].controllerColor.b;
            float a = selectedLines[i].controllerColor.a;
            r += players[i].playerColor.color.r;
            g += players[i].playerColor.color.g;
            b += players[i].playerColor.color.b;
            a += players[i].playerColor.color.a;

            r /= 2;
            g /= 2;
            b /= 2;
            a /= 2;

            selectedLines[i].sr_gradiant.color = new Color(r, g, b, a);
        }

        // MouseControl
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }

        if (PlayerOptions.playerConfig[activePlayer].controller >= 0 && Input.GetButtonDown("Action" + PlayerOptions.playerConfig[activePlayer].controller))
        {
            Vector3 pos = Vector3.zero;
            if(Input.GetKeyDown(KeyCode.Mouse0))
                pos = camera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * camera.transform.position.z;
            if(Input.GetButtonDown("Action" + players[activePlayer].controller))
                pos = playerCursors[activePlayer].transform.position;
            //Line nearestLine = GetNearestLine(pos);
            Line nearestLine = selectedLines[activePlayer];

            if (nearestLine.controllingPlayer < 0)
            {
                updateMode(nearestLine, this);
            }
        }

        // Update Player cursors
        for (int i = 0; i < PlayerOptions.playerConfig.Length; i++)
        {
            if (PlayerOptions.playerConfig[i].controller >= 0)
            {
                Vector3 input = new Vector3(Input.GetAxis("Horizontal" + PlayerOptions.playerConfig[i].controller), Input.GetAxis("Vertical" + PlayerOptions.playerConfig[i].controller));
                Vector3 capping = input.normalized;
                capping = capping * cursorCap;
                if (input.magnitude > capping.magnitude)
                    input = capping;
                playerCursors[i].transform.position += cursorSpeed * input.x * Vector3.right + cursorSpeed * input.y * Vector3.down;
                //playerCursors[i].transform.position += cursorSpeed * Math.Max(-capping.x, Math.Min(capping.x, input.x)) * Vector3.right + cursorSpeed * Math.Max(-capping.y, Math.Min(capping.y, input.y)) * Vector3.down;
                playerCursors[i].transform.position = new Vector3(Math.Min(PlayerOptions.horzExtent, Math.Max(-PlayerOptions.horzExtent, playerCursors[i].transform.position.x)), Math.Min(PlayerOptions.vertExtent, Math.Max(-PlayerOptions.vertExtent, playerCursors[i].transform.position.y)), 0);
            }
        }
    }

    public void UpdateCursor(int player)
    {
        Line target = null;
        float minDis = float.MaxValue;

        if (selectedLines[player] == null)
        {
            foreach (Line line in triangleComplex.lines)
            {
                if (line == null || !line.gameObject.activeSelf)
                    continue;

                Vector3 tPos = line.transform.position + line.GetCenter();
                float tmpDis = Vector3.Distance(playerCursors[player].transform.position, tPos);
                if (tmpDis <= minDis)
                {
                    minDis = tmpDis;
                    target = line;
                }
            }
        }
        else
        {
            foreach (Line l in triangleComplex.lines)
            {
                Vector3 tPos = l.transform.position + l.GetCenter();
                float tmpDis = Vector3.Distance(playerCursors[player].transform.position, tPos);
                if (tmpDis <= minDis)
                {
                    minDis = tmpDis;
                    target = l;
                }
            }

            //foreach (TrianglePiece t in selectedLines[player].trianglePieces)
            //{
            //    if (t == null)
            //        continue;

            //    foreach (Line l in t.lines)
            //    {
            //        if (l == null)
            //            continue;

            //        Vector3 tPos = l.transform.position + l.GetCenter();
            //        float tmpDis = Vector3.Distance(playerCursors[player].transform.position, tPos);
            //        if (tmpDis <= minDis)
            //        {
            //            minDis = tmpDis;
            //            target = l;
            //        }
            //    }
            //}

            // reset line Color if line was left
            if (selectedLines[player] != target)
            {
                selectedLines[player].sr_gradiant.color = selectedLines[player].controllerColor;
                soundInterface.PlaySound("switchLine", 0);
            }
        }

        selectedLines[player] = target;
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

    public static void UpdateMode_SingleTriangle(Line updatedLine, Player player)
    {
        updatedLine.controllingPlayer = player.activePlayer;
        updatedLine.sr_gradiant.color = player.lineColor;
        updatedLine.controllerColor = player.lineColor;

        foreach (TrianglePiece trianlgePiece in updatedLine.trianglePieces)
        {
            if(trianlgePiece == null)
            {
                continue;
            }

            bool allLinesControlled = true;
            foreach(Line line in trianlgePiece.lines)
            {
                if(line.controllingPlayer != player.activePlayer)
                {
                    allLinesControlled = false;
                    break;
                }
            }

            if (allLinesControlled)
            {
                trianlgePiece.triangle.sr_gradiant_hole.color = PlayerOptions.playerConfig[player.activePlayer].color;
            }
        }
    }

    public static void UpdateMode_MultipleTriangleWithoutOverride(Line updatedLine, Player player)
    {
        updatedLine.TakeControl(player);

        foreach(TrianglePiece triangle in updatedLine.trianglePieces)
        {
            if(triangle == null)
            {
                continue;
            }

            Queue<TrianglePiece> remainingTriangles = new Queue<TrianglePiece>();
            remainingTriangles.Enqueue(triangle);
            List<TrianglePiece> checkedTriangles = new List<TrianglePiece>();
            List<Line> checkedLines = new List<Line>();

            bool isBordered = true;
            while (remainingTriangles.Count > 0)
            {
                TrianglePiece currentTriangle = remainingTriangles.Dequeue();
                checkedTriangles.Add(currentTriangle);
                foreach(Line line in currentTriangle.lines)
                {
                    if(checkedLines.Contains(line))
                    {
                        continue;
                    }
                    if(line.IsOuterLine() && line.controllingPlayer != player.activePlayer)
                    {
                        isBordered = false;
                        break;
                    }
                    if(line.controllingPlayer == player.activePlayer)
                    {
                        checkedLines.Add(line);
                        continue;
                    }
                    else
                    {
                        foreach(TrianglePiece t in line.trianglePieces)
                        {
                            if(t == null || checkedTriangles.Contains(t) || remainingTriangles.Contains(t))
                            {
                                continue;
                            }
                            else
                            {
                                if(t.controllingPlayer == player.activePlayer || t.controllingPlayer < 0)
                                {
                                    remainingTriangles.Enqueue(t);
                                }
                                else
                                {
                                    isBordered = false;
                                    break;
                                }
                            }
                        }
                    }
                    checkedLines.Add(line);
                }
            }

            if(isBordered)
            {
                foreach(TrianglePiece t in checkedTriangles)
                {
                    if (t.gameObject.activeSelf)
                    {
                        t.TakeControl(player);
                    }
                }
                
                foreach(Line l in checkedLines)
                {
                    l.TakeControl(player);
                }
            }
        }
        player.soundInterface.PlaySound("takeControl", 1);
        if(!player.triangleComplex.IsSomethingConquerable())
            player.EndMatch();
        SetActivePlayer((player.activePlayer + 1) % player.numberOfPlayers, player);
        player.countDown.startTimeStemp = Time.time;
    }

    public void IncreaseActivePlayer()
    {
        SetActivePlayer((activePlayer + 1) % numberOfPlayers, this);
    }

    public static void SetActivePlayer(int i, Player player)
    {
        if (i >= player.numberOfPlayers)
            throw new Exception("Cannot set active player bigger then existing players!");
        player.activePlayer = i;
        for(int j = 0; j < player.players.Length; j++)
        {
            player.players[j].SetActivePlayerColor(i == j);
        }
    }

    public void TakeControlOfRandomLine()
    {
        List<Line> freeLines = new List<Line>();
        foreach (Line line in triangleComplex.lines)
        {
            if(line && line.gameObject.activeSelf && line.controllingPlayer < 0)
            {
                freeLines.Add(line);
            }
        }
        System.Random rnd = new System.Random();
        int randomLine = rnd.Next(0, freeLines.Count);

        updateMode(freeLines[randomLine], this);
    }

    public void EndMatch()
    {
        actionOnMatchEnd.Invoke();
    }

    public static void UpdateMode_MultipleTriangleWithOverride(Line updatedLine, Player player)
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
