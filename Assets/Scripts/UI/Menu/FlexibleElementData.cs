using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Flexible Element Data")]
public class FlexibleElementData : ScriptableObject
{
    [Header("Triangle Sprites")]
    public Sprite triangleBackground;
    public Color triangleBackgroundColor;
    public Sprite triangleInnerBorder;
    public Color triangleInnerBorderColor;
    public Sprite triangleOuterBorder;
    public Color triangleOuterBorderColor;
    public Sprite triangleGradiant;
    public Color triangleGradiantColor;
    public Sprite triangleGradiantWithHole;
    public Color triangleGradiantWithHoleColor;

    [Header("Line Sprites")]
    public Sprite lineBackground;
    public Color lineBackgroundColor;
    public Sprite lineBorder;
    public Color lineBorderColor;
    public Sprite lineGradiant;
    public Color lineGradiantColor;
    public Sprite lineGrid;
    public Color lineGridColor;

    [Header("Functionality Sprites")]
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite minus;
    public Vector3 numberSize;
    public Color numberColor;
    [Space]
    public Sprite bomb;
    public Vector3 bombSize;
    public Color bombColor;
    [Space]
    public Sprite arrow;
    public Vector3 arrowSize;
    public Color arrowColor;
}