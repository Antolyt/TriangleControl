using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInterface : MonoBehaviour {

    public SoundManager sm;

    // Use this for initialization
    void Start () {
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void PlaySound(string name)
    {
        foreach (Sound sound in sm.sounds)
        {
            if (sound.name == name)
            {
                sm.effectSources[0].clip = sound.audioClip;
                sm.effectSources[0].Play();
            }
        }
    }
}
