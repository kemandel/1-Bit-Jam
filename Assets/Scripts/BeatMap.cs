using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BeatMap : ScriptableObject
{
    public float fallTime;
    public Note[] beatmap;
    public AudioClip song;
}

[System.Serializable]
public struct Note 
{
    public bool lineCheckAfter;
    public bool slider;
    public float sliderDuration;
    public int position;
    public float time;
}
