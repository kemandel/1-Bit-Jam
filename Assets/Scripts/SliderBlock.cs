using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBlock : MonoBehaviour
{
    public float fallTime;

    // Update is called once per frame
    void Update()
    {
        foreach (NoteBlock n in GetComponentsInChildren<NoteBlock>())
        {
            n.fallTime = fallTime;
        }
    }
}
