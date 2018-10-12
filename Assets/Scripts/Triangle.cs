using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriangleTyp{
    up,
    down
}

public class Triangle : MonoBehaviour {

    public int id;
    public TriangleTyp type;
    public Line[] lines; // right, lower, left if facing downwards // upper, right, left if facing upwards

    public Vector3 center;
    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetLength()
    {
        return GetComponent<BoxCollider2D>().size.x;
    }

    public float GetHeight()
    {
        return GetComponent<BoxCollider2D>().size.y;
    }

    public override bool Equals(object other)
    {
        if (typeof(Triangle) == other.GetType())
            return this.id == (other as Triangle).id;
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
