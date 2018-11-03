using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : FlexibleElement
{
    public SpriteRenderer sr_background;
    public SpriteRenderer sr_border_inner;
    public SpriteRenderer sr_border_outer;
    public SpriteRenderer sr_gradiant;
    public SpriteRenderer sr_gradiant_hole;

    public Color defaultColor;
    public Color controllerColor;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        //sr_background.sprite = data.triangleBackground;
        //sr_border_inner.sprite = data.triangleInnerBorder;
        //sr_border_outer.sprite = data.triangleOuterBorder;
        //sr_gradiant.sprite = data.triangleGradiant;
        //sr_gradiant_hole.sprite = data.triangleGradiantWithHole;

        //sr_background.color = data.triangleBackgroundColor;
        //sr_border_inner.color = data.triangleInnerBorderColor;
        //sr_border_outer.color = data.triangleOuterBorderColor;
        //sr_gradiant.color = data.triangleGradiantColor;
        //sr_gradiant_hole.color = data.triangleGradiantWithHoleColor;

        //defaultColor = data.triangleBackgroundColor;
    }
}