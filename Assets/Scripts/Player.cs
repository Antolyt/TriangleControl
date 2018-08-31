using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public new Camera camera;
    public TriangleComplex tcb;

    public int activePlayer = 0;
    public PlayerConfig[] players;
    public Color lineColor;

    private void Start()
    {
        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;
        for (int i = 0; i < players.Length; i++)
        {
            r += players[i].color.r;
            g += players[i].color.g;
            b += players[i].color.b;
            a += players[i].color.a;
        }

        r /= players.Length;
        g /= players.Length;
        b /= players.Length;
        a /= players.Length;

        lineColor = new Color(r, g, b, a);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            Vector3 clickPos = camera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * camera.transform.position.z;
            Line nearestLine = GetNearestLine(clickPos);

            if(nearestLine.controllingPlayer < 0)
            {
                nearestLine.controllingPlayer = activePlayer;
                nearestLine.gameObject.GetComponent<SpriteRenderer>().color = lineColor;

                TriangleStartPoint tsp = nearestLine.tsp;

                bool triangleFilled = false;

                switch(nearestLine.type)
                {
                    case lineType.upper:
                        if(tsp.triangleStartPointPreviousUpper != null)
                        {
                            if(tsp.triangleStartPointPreviousUpper.lineMiddle.activeSelf && tsp.triangleStartPointPreviousUpper.lineLower.activeSelf
                                && tsp.triangleStartPointPreviousUpper.lineMiddle.GetComponent<Line>().controllingPlayer >= 0 && tsp.triangleStartPointPreviousUpper.lineLower.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleStartPointPreviousUpper.triangleLower.activeSelf)
                                {
                                    tsp.triangleStartPointPreviousUpper.triangleLower.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        if(tsp.triangleStartPointNextUpper != null)
                        {
                            if (tsp.triangleStartPointNextUpper.lineLower.activeSelf && tsp.lineMiddle.activeSelf
                                && tsp.triangleStartPointNextUpper.lineLower.GetComponent<Line>().controllingPlayer >= 0 && tsp.lineMiddle.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleUpper.activeSelf)
                                {
                                    tsp.triangleUpper.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        break;
                    case lineType.middle:
                        if (tsp.triangleStartPointNextUpper != null)
                        {
                            if (tsp.triangleStartPointNextUpper.lineLower.activeSelf && tsp.lineUpper.activeSelf
                                && tsp.triangleStartPointNextUpper.lineLower.GetComponent<Line>().controllingPlayer >= 0 && tsp.lineUpper.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleUpper.activeSelf)
                                {
                                    tsp.triangleUpper.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        if (tsp.triangleStartPointNextLower != null)
                        {
                            if (tsp.triangleStartPointNextLower.lineUpper.activeSelf && tsp.lineLower.activeSelf
                                && tsp.triangleStartPointNextLower.lineUpper.GetComponent<Line>().controllingPlayer >= 0 && tsp.lineLower.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleLower.activeSelf)
                                {
                                    tsp.triangleLower.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        break;
                    case lineType.lower:
                        if (tsp.triangleStartPointPreviousLower != null)
                        {
                            if (tsp.triangleStartPointPreviousLower.lineMiddle.activeSelf && tsp.triangleStartPointPreviousLower.lineUpper.activeSelf
                                && tsp.triangleStartPointPreviousLower.lineMiddle.GetComponent<Line>().controllingPlayer >= 0 && tsp.triangleStartPointPreviousLower.lineUpper.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleStartPointPreviousLower.triangleUpper.activeSelf)
                                {
                                    tsp.triangleStartPointPreviousLower.triangleUpper.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        if (tsp.triangleStartPointNextLower != null)
                        {
                            if (tsp.triangleStartPointNextLower.lineUpper.activeSelf && tsp.lineMiddle.activeSelf
                                && tsp.triangleStartPointNextLower.lineUpper.GetComponent<Line>().controllingPlayer >= 0 && tsp.lineMiddle.GetComponent<Line>().controllingPlayer >= 0)
                            {
                                if (tsp.triangleLower.activeSelf)
                                {
                                    tsp.triangleLower.GetComponent<SpriteRenderer>().color = players[activePlayer].color;
                                    players[activePlayer].score.text = (int.Parse(players[activePlayer].score.text) + 1).ToString();
                                    triangleFilled = true;
                                    TriangleWasFilled();
                                }
                            }
                        }
                        break;
                }

                if(!triangleFilled)
                {
                    activePlayer = (activePlayer + 1) % players.Length;
                }
            }
        }
	}

    public Line GetNearestLine(Vector3 pos)
    {
        Line target = null;
        float minDis = float.MaxValue;
        foreach(Transform t in tcb.lines)
        {
            if (t == null)
                continue;

            //ToDO effizienter
            Line line = t.gameObject.GetComponent<Line>();
            Vector3 tPos = t.position + line.centerVector;
            float tmpDis = Vector3.Distance(pos, tPos);
            if (tmpDis <= minDis)
            {
                minDis = tmpDis;
                target = line;
            }
        }

        return target;
    }

    public void TriangleWasFilled()
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
