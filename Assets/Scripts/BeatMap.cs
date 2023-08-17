using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BeatMap : ScriptableObject
{
    public string songName;
    public AudioClip song;
    public float fallTime;
    public Note[] beatmap;
}

[System.Serializable]
public struct Note 
{
    public bool LineCheck;
    public bool slider;
    public float sliderDuration;
    public int position;
    public float time;
}
