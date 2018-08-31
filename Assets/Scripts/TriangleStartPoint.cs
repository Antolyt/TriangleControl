using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleStartPoint : MonoBehaviour {

    public GameObject lineUpper;
    public GameObject lineMiddle;
    public GameObject lineLower;

    public GameObject triangleUpper;
    public GameObject triangleLower;

    public TriangleStartPoint triangleStartPointPreviousUpper;
    public TriangleStartPoint triangleStartPointPreviousMiddle;
    public TriangleStartPoint triangleStartPointPreviousLower;

    public TriangleStartPoint triangleStartPointNextUpper;
    public TriangleStartPoint triangleStartPointNextMiddle;
    public TriangleStartPoint triangleStartPointNextLower;

    public GameObject[] activeLines;
    public GameObject[] activeTriangles;
    public GameObject[] activeTriagleStartPoints;

    // Use this for initialization
    void Start () {
        activeLines = new GameObject[] { lineUpper, lineMiddle, lineLower };
        activeTriangles = new GameObject[] { triangleUpper, triangleLower };

        lineUpper.GetComponent<Line>().SetType(lineType.upper);
        lineMiddle.GetComponent<Line>().SetType(lineType.middle);
        lineLower.GetComponent<Line>().SetType(lineType.lower);
    }

    public void Instantiate()
    {
        activeLines = new GameObject[] { lineUpper, lineMiddle, lineLower };
        activeTriangles = new GameObject[] { triangleUpper, triangleLower };

        lineUpper.GetComponent<Line>().SetType(lineType.upper);
        lineMiddle.GetComponent<Line>().SetType(lineType.middle);
        lineLower.GetComponent<Line>().SetType(lineType.lower);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeactivateUpper()
    {
        lineUpper.SetActive(false);
        triangleUpper.SetActive(false);

        activeLines = new GameObject[]{ lineMiddle, lineLower };
        activeTriangles = new GameObject[] { triangleLower };
    }

    public void DeactivateLower()
    {
        lineLower.SetActive(false);
        triangleLower.SetActive(false);

        activeLines = new GameObject[] { lineUpper, lineMiddle };
        activeTriangles = new GameObject[] { triangleUpper };
    }

    public void DeactivateExceptUpperLine()
    {
        DeactivateLower();
        lineMiddle.SetActive(false);
        triangleUpper.SetActive(false);

        activeLines = new GameObject[] { lineUpper };
        activeTriangles = new GameObject[0];
    }

    public void DeactivateExceptLowerLine()
    {
        DeactivateUpper();
        lineMiddle.SetActive(false);
        triangleLower.SetActive(false);

        activeLines = new GameObject[] { lineLower };
        activeTriangles = new GameObject[0];
    }

    public float GetLength()
    {
        return lineMiddle.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public float GetHeight()
    {
        return 2 * (Quaternion.Euler(0, 0, 60) * new Vector3(GetLength(), 0, 0)).y;
    }

    public Vector3 GetTriangleUpperVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = (length + Quaternion.Euler(0, 0, 60) * length) / 3;

        return vec;
    }

    public Vector3 GetTriangleLowerVector()
    {
        Vector3 length = new Vector3(GetLength(), 0, 0);
        Vector3 vec = (length + Quaternion.Euler(0, 0, -60) * length) / 3;

        return vec;
    }

    public bool IsUpperTriangleActive()
    {
        return triangleUpper.activeSelf;
    }

    public bool IsLowerTriangleActive()
    {
        return triangleLower.activeSelf;
    }
}
