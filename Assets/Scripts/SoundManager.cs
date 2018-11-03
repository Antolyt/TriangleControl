using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance { get; set; }

    public AudioSource backgroundSource;
    public List<AudioSource> effectSources;

    public AudioClip menuMusic;

    public List<Sound> sounds;

    // Use this for initialization
    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        backgroundSource.clip = menuMusic;
        backgroundSource.Play();
    }

	// Update is called once per frame
	void Update () {
		
	}
}

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip audioClip;
}