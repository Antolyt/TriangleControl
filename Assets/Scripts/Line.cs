using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lineType
{
    upper,
    middle,
    lower
}

public class Line : MonoBehaviour {

    public lineType type;
    public Vector3 centerVector;
    public TriangleStartPoint tsp;
    public int controllingPlayer = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetLength()
    {
        return this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void SetType(lineType type)
    {
        this.type = type;
        switch(type)
        {
            case lineType.upper:
                centerVector = GetLineUpperCenterVector();
                break;
            case lineType.middle:
                centerVector = GetLineMiddleCenterVector();
                break;
            case lineType.lower:
                centerVector = GetLineLowerCenterVector();
                break;
        }
    }

    public Vector3 GetLineUpperCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, 60) * length / 2;

        return vec;
    }

    public Vector3 GetLineMiddleCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = length / 2;

        return vec;
    }

    public Vector3 GetLineLowerCenterVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = Quaternion.Euler(0, 0, -60) * length / 2;

        return vec;
    }
}
