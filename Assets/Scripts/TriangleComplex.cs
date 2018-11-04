using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleComplex : MonoBehaviour {

    public GameObject triangleContainer;
    public GameObject lineContainer;
    public List<TrianglePiece> triangles;
    public List<Line> lines;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsSomethingConquerable()
    {
        foreach(TrianglePiece tp in triangles)
        {
            if (!tp.gameObject.activeSelf || tp.controllingPlayer >= 0)
                continue;

            int controller = -1;
            bool tpConquerable = true;
            foreach(Line line in tp.lines)
            {
                if (!line || line.controllingPlayer < 0)
                    continue;

                // break if triangle is not conquerable
                if(line.controllingPlayer != controller)
                {
                    if(controller < 0)
                    {
                        controller = line.controllingPlayer;
                    }
                    else
                    {
                        tpConquerable = false;
                        break;
                    }
                }
                // triangle is conquerable
            }
            if (tpConquerable)
                return true;
        }
        // no conqueralbe triangle was found
        return false;
    }
}
