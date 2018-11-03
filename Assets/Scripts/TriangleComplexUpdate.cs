using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TriangleComplexUpdate : MonoBehaviour {

    public TriangleComplex triangleComplex;

    public void RemoveDeactivatedElements()
    {
        List<Line> tmpLines = new List<Line>();
        foreach(Line line in triangleComplex.lines)
        {
            if(line)
            {
                if (line.gameObject.activeSelf)
                {
                    tmpLines.Add(line);
                }
                else
                {
                    DestroyImmediate(line.gameObject);
                }
            }
        }
        triangleComplex.lines = tmpLines;

        List<TrianglePiece> tmpTrianglePieces = new List<TrianglePiece>();
        foreach (TrianglePiece trianglePiece in triangleComplex.triangles)
        {
            if(trianglePiece)
            {
                if (trianglePiece.gameObject.activeSelf)
                {
                    tmpTrianglePieces.Add(trianglePiece);
                }
                else
                {
                    DestroyImmediate(trianglePiece.gameObject);
                }
            }
        }
        triangleComplex.triangles = tmpTrianglePieces;
    }

    public void UpdateFunctionalityImage()
    {
        foreach (TrianglePiece tp in triangleComplex.triangles)
        {
            FlexibleElementData data = tp.GetComponentInChildren<Triangle>().data;

            int i = 0;
            while (i++ < 10)
            {
                Transform functionalityImage = tp.transform.Find("FunctionalityImage");
                if(functionalityImage)
                {
                    DestroyImmediate(functionalityImage.gameObject);
                }
                else
                {
                    break;
                }
            }

            switch (tp.functionality)
            {
                case Functionality.normal:
                    GameObject n = null;
                    if (tp.points == 1)
                        continue;
                    else if (tp.points < 0)
                    {
                        n = new GameObject("FunctionalityImage");

                        GameObject n1 = new GameObject("Minus", typeof(SpriteRenderer));
                        n1.transform.parent = n.transform;
                        SpriteRenderer n1Sr = n1.GetComponent<SpriteRenderer>();
                        n1Sr.color = data.numberColor;
                        n1Sr.sortingOrder = 1;

                        GameObject n2 = new GameObject("Number", typeof(SpriteRenderer));
                        n2.transform.parent = n.transform;
                        SpriteRenderer n2Sr = n2.GetComponent<SpriteRenderer>();
                        n2Sr.color = data.numberColor;
                        n2Sr.sortingOrder = 1;

                        n1Sr.sprite = data.minus;
                        switch (tp.points)
                        {
                            case -1:
                                n1.transform.localPosition = Vector3.left * 0.25f;
                                n2Sr.sprite = data.one;
                                n2.transform.localPosition = Vector3.right * 0.15f;
                                break;
                            case -2:
                                n1.transform.localPosition = Vector3.left * 0.32f;
                                n2Sr.sprite = data.two;
                                n2.transform.localPosition = Vector3.right * 0.18f;
                                break;
                            case -3:
                                n1.transform.localPosition = Vector3.left * 0.32f;
                                n2Sr.sprite = data.three;
                                n2.transform.localPosition = Vector3.right * 0.18f;
                                break;
                            default:
                                break;
                        }
                        n.transform.parent = tp.transform;
                        n.transform.localPosition = Vector3.zero;
                        n.transform.localScale = data.numberSize;
                    }
                    else
                    {
                        n = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                        SpriteRenderer nSr = n.GetComponent<SpriteRenderer>();
                        switch (tp.points)
                        {
                            case 2:
                                nSr.sprite = data.two;
                                break;
                            case 3:
                                nSr.sprite = data.three;
                                break;
                            default:
                                break;
                        }
                        nSr.color = data.numberColor;
                        nSr.sortingOrder = 1;
                        n.transform.parent = tp.transform;
                        n.transform.localPosition = Vector3.zero;
                        n.transform.localScale = data.numberSize;
                    }
                    break;
                case Functionality.bomb:
                    GameObject b = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                    SpriteRenderer bSr = b.GetComponent<SpriteRenderer>();
                    bSr.sprite = data.bomb;
                    bSr.color = data.bombColor;
                    bSr.sortingOrder = 1;
                    b.transform.parent = tp.transform;
                    b.transform.localPosition = Vector3.zero;
                    b.transform.localScale = data.bombSize;
                    break;
                case Functionality.conquerNext:
                    GameObject c = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                    SpriteRenderer cSr = c.GetComponent<SpriteRenderer>();
                    cSr.sprite = data.arrow;
                    cSr.color = data.arrowColor;
                    cSr.sortingOrder = 1;
                    c.transform.parent = tp.transform;
                    c.transform.localPosition = Vector3.zero;
                    switch (tp.conquerNextDirection)
                    {
                        case Direction.normal:
                            break;
                        case Direction.up:
                            break;
                        case Direction.upRight:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -60);
                            break;
                        case Direction.right:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            break;
                        case Direction.downRight:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -120);
                            break;
                        case Direction.down:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case Direction.downLeft:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 120);
                            break;
                        case Direction.left:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case Direction.upLeft:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 60);
                            break;
                    }
                    c.transform.localScale = data.arrowSize;
                    break;
                default:
                    break;
            }
        }
    }
}
