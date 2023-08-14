using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BeatMap : ScriptableObject
{
    public float fallSpeed;
    public Note[] beatmap;
    public AudioClip song;
}

[System.Serializable]
public struct Note 
{
    public int position;
    public float time;
}
