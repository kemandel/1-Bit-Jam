using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InputBlock : MonoBehaviour
{
    public const float GOOD_DISTANCE = .5f;
    public const float PERFECT_DISTANCE = .125f;

    public const float GOOD_TIMING_SLIDER = .5f;
    public const float PERFECT_TIMING_SLIDER = .25f;

    public bool day;
    public KeyCode DayInput;
    public KeyCode NightInput;

    private NoteBlock currentNote = null;
    private bool sliding = false;
    private float slideStart = 0;

    private void Update()
    {
        day = false;
        if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX)
        {
            day = true;
        }

        KeyCode code = day ? DayInput : NightInput;
        if (Input.GetKeyDown(code))
        {
            if (currentNote != null && !sliding)
            {
                if (currentNote is SliderBlock)
                {
                    slideStart = Time.time;
                    sliding = true;
                }
                else
                {
                    float distance = Vector2.Distance(transform.position, currentNote.transform.position);

                    if (distance > GOOD_DISTANCE)
                    {
                        currentNote.Miss();
                    }
                    else if (distance > PERFECT_DISTANCE)
                    {
                        currentNote.GoodHit();
                    }
                    else
                    {
                        currentNote.PerfectHit();
                    }
                    currentNote = null;
                }
            }
        }
        if (sliding)
        {
            if (Input.GetKeyUp(code))
            {
                float timing = Mathf.Abs(Time.time - slideStart);
                if (timing > GOOD_TIMING_SLIDER)
                {
                    currentNote.Miss();
                }
                else if (timing > PERFECT_TIMING_SLIDER)
                {
                    currentNote.GoodHit();
                }
                else
                {
                    currentNote.PerfectHit();
                }
                currentNote = null;
                sliding = false;
            }
            else if (slideStart > Time.time + ((SliderBlock)currentNote).duration + 1f)
            {
                currentNote.Miss();
                currentNote = null;
                sliding = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        currentNote = other.GetComponent<NoteBlock>();
    }
}
