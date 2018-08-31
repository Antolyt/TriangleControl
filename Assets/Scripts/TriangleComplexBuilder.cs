using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleComplexBuilder : MonoBehaviour {

    public Player player;

    public GameObject parentObject;
    public Sprite spawnMask;
    public GameObject startPoint;
    public float resolution;
    public float scale;

    public TriangleStartPoint[,] matrix;

    

    // Use this for initialization
    public void BuildComplex()
    {
        TriangleStartPoint tsp = startPoint.GetComponent<TriangleStartPoint>();

        matrix = new TriangleStartPoint[(int)(spawnMask.texture.width * 2 * scale * resolution) + 2, (int)(spawnMask.texture.height * scale * resolution) + 2];

        GameObject triangleComplex = GameObject.Instantiate<GameObject>(parentObject);
        Transform parentTransform = triangleComplex.transform;
        List<Transform> lines = parentTransform.GetComponent<TriangleComplex>().lines;
        player.tcb = triangleComplex.GetComponent<TriangleComplex>();

        for (int i = 0; i < spawnMask.texture.width * 2 * scale * resolution; i++)
        {
            for (int j = i % 2; j < spawnMask.texture.height * scale * resolution; j += 2)
            {
                GameObject go = GameObject.Instantiate(startPoint);
                go.transform.parent = parentTransform;
                go.transform.localScale /= resolution;
                go.transform.localPosition = new Vector3(i * tsp.GetLength() / resolution / 2, j * tsp.GetHeight() / resolution / 2, 0);

                Vector3 upper = new Vector3(((float)i / 2.0f + tsp.GetTriangleUpperVector().x) / scale / resolution, ((float)j + tsp.GetTriangleUpperVector().y) / scale / resolution);
                Vector3 lower = new Vector3(((float)i / 2.0f + tsp.GetTriangleLowerVector().x) / scale / resolution, ((float)j + tsp.GetTriangleLowerVector().y) / scale / resolution);

                Vector3Int upperInt = new Vector3Int((int)Math.Round(upper.x), (int)Math.Round(upper.y), 0);
                Vector3Int lowerInt = new Vector3Int((int)Math.Round(lower.x), (int)Math.Round(lower.y), 0);

                bool upperInBounds = spawnMask.texture.GetPixel(upperInt.x, upperInt.y).a > 0.2f && upper.x <= spawnMask.texture.width && upper.y > 0 && upper.y <= spawnMask.texture.height;
                bool lowerInBounds = spawnMask.texture.GetPixel(lowerInt.x, lowerInt.y).a > 0.2f && lower.x <= spawnMask.texture.width && lower.y > 0 && lower.y <= spawnMask.texture.height;

                TriangleStartPoint goTsp = go.GetComponent<TriangleStartPoint>();
                goTsp.Instantiate();

                if (!upperInBounds && !lowerInBounds)
                {
                    if (i == 0)
                    {
                        DestroyImmediate(go);
                        continue;
                    }

                    bool activeLower = j >= 1 && matrix[i - 1, j - 1] != null && matrix[i - 1, j - 1].IsUpperTriangleActive();
                    bool activeUpper = j >= 1 && matrix[i - 1, j + 1] != null && matrix[i - 1, j + 1].IsLowerTriangleActive();

                    if (activeUpper && activeLower)
                    {

                    }
                    else if (activeUpper)
                    {
                        goTsp.DeactivateExceptUpperLine();
                    }
                    else if (activeUpper)
                    {
                        goTsp.DeactivateExceptLowerLine();
                    }
                    else
                    {
                        DestroyImmediate(go);
                        continue;
                    }

                }
                else if (upperInBounds && !lowerInBounds)
                {
                    goTsp.DeactivateLower();
                }
                else if (!upperInBounds && lowerInBounds)
                {
                    goTsp.DeactivateUpper();
                }
                else
                {

                }

                foreach (GameObject l in goTsp.activeLines)
                {
                    lines.Add(l.transform);
                }

                matrix[i, j] = goTsp;

                //previous lower
                if (i >= 1 && j >= 1 && matrix[i - 1, j - 1] != null)
                {
                    goTsp.triangleStartPointPreviousLower = matrix[i - 1, j - 1];
                    matrix[i - 1, j - 1].triangleStartPointNextUpper = goTsp;
                }

                //previous middle
                if (i >= 2 && matrix[i - 2, j] != null)
                {
                    goTsp.triangleStartPointPreviousMiddle = matrix[i - 2, j];
                    matrix[i - 2, j].triangleStartPointNextMiddle = goTsp;
                }

                //previous upper
                if (i >= 1 && matrix[i - 1, j + 1] != null)
                {
                    goTsp.triangleStartPointPreviousUpper = matrix[i - 1, j + 1];
                    matrix[i - 1, j + 1].triangleStartPointNextLower = goTsp;
                }
            }
        }
    }

    //public void BuildComplex()
    //{
    //    Instantiate(trianglePoint, this.transform.position, Quaternion.identity);
    //}
}
