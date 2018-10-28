using System;
using UnityEngine;

public class LevelList : MonoBehaviour {

    public LevelAndName[] Levels;
}

[Serializable]
public struct LevelAndName
{
    public string name;
    public GameObject prefab;
}