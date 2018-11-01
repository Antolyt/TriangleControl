using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkLines : MonoBehaviour {

    public TriangleComplex triangleComplex;
    public MarkStep[] step;
    int stepIndex = 0;

    private float time;
    public float replayWaitingTime;

	// Use this for initialization
	void Start () {
        time = Time.time;
	}

    private void OnEnable()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepIndex < step.Length && time + step[stepIndex].waitingTime <= Time.time)
        {
            step[stepIndex].line.TakeControl(step[stepIndex].player, step[stepIndex].color);

            foreach (TrianglePiece triangle in step[stepIndex].line.trianglePieces)
            {
                if (triangle == null)
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
                    foreach (Line line in currentTriangle.lines)
                    {
                        if (checkedLines.Contains(line))
                        {
                            continue;
                        }
                        if (line.IsOuterLine() && line.controllingPlayer != step[stepIndex].player)
                        {
                            isBordered = false;
                            break;
                        }
                        if (line.controllingPlayer == step[stepIndex].player)
                        {
                            checkedLines.Add(line);
                            continue;
                        }
                        else
                        {
                            foreach (TrianglePiece t in line.trianglePieces)
                            {
                                if (t == null || checkedTriangles.Contains(t) || remainingTriangles.Contains(t))
                                {
                                    continue;
                                }
                                else
                                {
                                    remainingTriangles.Enqueue(t);
                                }
                            }
                        }
                        checkedLines.Add(line);
                    }
                }

                if (isBordered)
                {
                    foreach (TrianglePiece t in checkedTriangles)
                    {
                        t.TakeControl(step[stepIndex].player, step[stepIndex].color);
                    }

                    foreach (Line l in checkedLines)
                    {
                        l.TakeControl(step[stepIndex].player, step[stepIndex].color);
                    }
                }
            }
            time = Time.time;
            stepIndex++;
        }

        if (replayWaitingTime > 0 && time + replayWaitingTime <= Time.time)
        {
            Reset();
        }
    }

    public void Reset()
    {
        foreach (TrianglePiece t in triangleComplex.triangles)
        {
            t.Reset();
        }
        foreach (Line l in triangleComplex.lines)
        {
            l.Reset();
        }
        time = Time.time;
        stepIndex = 0;
    }
}

[Serializable]
public struct MarkStep
{
    public Line line;
    public float waitingTime;
    public Color color;
    public int player;
}