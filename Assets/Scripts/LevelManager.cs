using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Player player;
    public GameObject defaultLevel;

	// Use this for initialization
	void Start () {
        GameObject level = Resources.Load<GameObject>(PlayerOptions.level);
        TriangleComplex tc;

        if (level != null)
            tc = Instantiate(level).GetComponent<TriangleComplex>();
        else
            tc = Instantiate(defaultLevel).GetComponent<TriangleComplex>();

        player.triangleComplex = tc;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
