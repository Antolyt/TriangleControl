﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleElement : MonoBehaviour
{
    public FlexibleElementData data;

    protected virtual void OnSkinUI()
    {

    }

    public virtual void Awake()
    {
        OnSkinUI();
    }

    // ToDo: EditorScript that call Update Method, so that this is not updated, when the game is running
    public virtual void Update()
    {
        if(Application.isEditor)
        {
            OnSkinUI();
        }
    }
}
