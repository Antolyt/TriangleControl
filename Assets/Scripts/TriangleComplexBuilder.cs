using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleComplexBuilder : MonoBehaviour {

    public GameObject parentObject;
    public Sprite spawnMask;
    public GameObject trianglePieceUp;
    public GameObject trianglePieceDown;
    public float resolution;
    public float scale;

    public TrianglePiece[,] matrix;

    public void BuildComplex()
    {
        TrianglePiece triangleUpward = trianglePieceUp.GetComponent<TrianglePiece>();
        TrianglePiece triangleDownward = trianglePieceDown.GetComponent<TrianglePiece>();

        matrix = new TrianglePiece[(int)(spawnMask.texture.width * 4 * scale * resolution + 1), (int)(spawnMask.texture.height * 4 * scale * resolution) + 2];

        GameObject triangleComplex = GameObject.Instantiate<GameObject>(parentObject);
        TriangleComplex tc = triangleComplex.GetComponent<TriangleComplex>();

        int lineId = 0;
        int triangleId = 0;
        for (int i = 0; i < spawnMask.texture.height * scale * resolution * 2; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < spawnMask.texture.width * scale * resolution; j++)
                {
                    GameObject go = GameObject.Instantiate(trianglePieceUp);
                    go.transform.parent = tc.triangleContainer.transform;
                    go.transform.localScale /= resolution;
                    if (i % 4 == 0)
                        go.transform.localPosition = new Vector3(j * triangleUpward.GetLength() / resolution, i * triangleUpward.GetHeight() / resolution / 2, 0);
                    else
                        go.transform.localPosition = new Vector3((j + 0.5f) * triangleUpward.GetLength() / resolution, i * triangleUpward.GetHeight() / resolution / 2, 0);

                    TrianglePiece goTriangle = go.GetComponent<TrianglePiece>();
                    tc.triangles.Add(goTriangle);
                    goTriangle.id = triangleId++;
                    foreach(Line line in goTriangle.lines)
                    {
                        line.id = lineId++;
                        line.transform.parent = tc.lineContainer.transform;
                        tc.lines.Add(line);
                        line.gameObject.SetActive(false);
                    }

                    if (i % 4 == 0)
                        matrix[j * 2, i / 2] = goTriangle;
                    else
                        matrix[j * 2 + 1, i / 2] = goTriangle;

                    Vector3 pos = go.transform.position;
                    goTriangle.center = new Vector3(pos.x, pos.y + triangleUpward.GetLength() / 4 / resolution, 0);

                    Vector3Int bounds = new Vector3Int((int)Math.Round(goTriangle.center.x / 2 / scale), (int)Math.Round(goTriangle.center.y / 2 / scale), 0);
                    bool inBounds = spawnMask.texture.GetPixel(bounds.x, bounds.y).a > 0.2f && bounds.x <= spawnMask.texture.width && bounds.y > 0 && bounds.y <= spawnMask.texture.height;

                    if (!inBounds)
                    {
                        go.SetActive(false);
                    }
                }
            }
            else
            {
                for (int j = 0; j < spawnMask.texture.width * scale * resolution; j++)
                {
                    GameObject go = GameObject.Instantiate(trianglePieceDown);
                    go.transform.parent = tc.triangleContainer.transform;
                    go.transform.localScale /= resolution;
                    if (i % 4 == 1)
                        go.transform.localPosition = new Vector3((j + 0.5f) * triangleDownward.GetLength() / resolution, (i-1/3f) * triangleDownward.GetHeight() / resolution / 2, 0);
                    else
                        go.transform.localPosition = new Vector3((j) * triangleDownward.GetLength() / resolution, (i-1/3f) * triangleDownward.GetHeight() / resolution / 2, 0);

                    TrianglePiece goTriangle = go.GetComponent<TrianglePiece>();
                    tc.triangles.Add(goTriangle);
                    goTriangle.id = triangleId++;
                    foreach (Line line in goTriangle.lines)
                    {
                        line.id = lineId++;
                        line.transform.parent = tc.lineContainer.transform;
                        tc.lines.Add(line);
                        line.gameObject.SetActive(false);
                    }

                    if (i % 4 == 1)
                        matrix[j * 2 + 1, i / 2] = goTriangle;
                    else
                        matrix[j * 2, i / 2] = goTriangle;

                    Vector3 pos = go.transform.position;
                    goTriangle.center = new Vector3(pos.x, pos.y - triangleDownward.GetLength() / 4 / resolution, 0);

                    Vector3Int bounds = new Vector3Int((int)Math.Round(goTriangle.center.x / 2 / scale), (int)Math.Round(goTriangle.center.y / 2 / scale), 0);
                    bool inBounds = spawnMask.texture.GetPixel(bounds.x, bounds.y).a > 0.2f && bounds.x <= spawnMask.texture.width && bounds.y > 0 && bounds.y <= spawnMask.texture.height;

                    if(!inBounds)
                    {
                        go.SetActive(false);
                    }
                }
            }
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != null)
                {
                    if (matrix[i, j].type == TriangleTyp.down)
                    {
                        if (i + i < matrix.GetLength(0) && matrix[i + 1, j] != null)
                        {
                            tc.lines.Remove(matrix[i + 1, j].lines[2]);
                            GameObject.DestroyImmediate(matrix[i + 1, j].lines[2].gameObject);
                            matrix[i + 1, j].lines[2] = matrix[i, j].lines[1];
                            matrix[i + 1, j].lines[2].trianglePieces[1] = matrix[i + 1, j];
                        }
                        if (j + i < matrix.GetLength(1) && matrix[i, j + 1] != null)
                        {
                            tc.lines.Remove(matrix[i, j + 1].lines[1]);
                            GameObject.DestroyImmediate(matrix[i, j + 1].lines[1].gameObject);
                            matrix[i, j + 1].lines[1] = matrix[i, j].lines[0];
                            matrix[i, j + 1].lines[1].trianglePieces[1] = matrix[i, j + 1];
                        }
                    }
                    if (matrix[i, j].type == TriangleTyp.up)
                    {
                        if (i + i < matrix.GetLength(0) && matrix[i + 1, j] != null)
                        {
                            tc.lines.Remove(matrix[i + 1, j].lines[2]);
                            GameObject.DestroyImmediate(matrix[i + 1, j].lines[2].gameObject);
                            matrix[i + 1, j].lines[2] = matrix[i, j].lines[0];
                            matrix[i + 1, j].lines[2].trianglePieces[1] = matrix[i + 1, j];
                        }

                    }
                }
            }
        }

        foreach(TrianglePiece triangle in tc.triangles)
        {
            if(triangle.gameObject.activeSelf)
            {
                foreach(Line line in triangle.lines)
                {
                    line.gameObject.SetActive(true);
                }
            }
        }
    }

}
