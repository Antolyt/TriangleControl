﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lineType
{
    horizontal,
    rising,
    falling
}

public class Line : MonoBehaviour {

    public lineType type;
    public Vector3 centerVector;
    public SpriteRenderer sr_background;
    public SpriteRenderer sr_border;
    public SpriteRenderer sr_gradiant;
    public SpriteRenderer sr_grid;
    public int controllingPlayer = -1;
    public TrianglePiece[] trianglePieces;
    public int id;

    public Color defaultColor;
    public Color controllerColor;

    public Vector3 center;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeControl(Player player)
    {
        sr_gradiant.color = PlayerOptions.playerConfig[player.activePlayer].color;
        controllerColor = PlayerOptions.playerConfig[player.activePlayer].color;
        controllingPlayer = player.activePlayer;
    }

    public float GetLength()
    {
        return sr_border.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public Vector3 GetCenter()
    {
        //switch(type)
        //{
        //    case lineType.rising:
        //         return GetLineRisingCenterVector();
        //    case lineType.horizontal:
        //        return GetLineHorizontalCenterVector();
        //    case lineType.falling:
        //        return GetLineFallingCenterVector();
        //    default:
        //        throw new UnityException("line type is not defined!");
        //}
        return Vector3.zero;
    }

    public bool IsOuterLine()
    {
        foreach(TrianglePiece triangle in trianglePieces)
        {
            if (triangle == null)
                return true;
        }
        return false;
    }

    public Vector3 GetLineRisingCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, 60) * length / 2;

        return vec;
    }

    public Vector3 GetLineHorizontalCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = length / 2;

        return vec;
    }

    public Vector3 GetLineFallingCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, -60) * length / 2;

        return vec;
    }

    public override bool Equals(object other)
    {
        if (typeof(Line) == other.GetType())
            return this.id == (other as Line).id;
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
