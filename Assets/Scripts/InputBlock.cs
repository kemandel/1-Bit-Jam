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

    public const float GOOD_DISTANCE_SLIDER = 1f;
    public const float PERFECT_DISTANCE_SLIDER = .25f;

    public bool day;
    public KeyCode DayInput;
    public KeyCode NightInput;

    private NoteBlock currentNote = null;
    private bool sliding = false;

    private void Update()
    {
        day = false;
        if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX)
        {
            day = true;
        }

        KeyCode code = day ? DayInput : NightInput;
        if (currentNote != null)
        {
            float distance = Vector2.Distance(transform.position, currentNote.transform.position);
            if (Input.GetKeyDown(code))
            {
                if (!sliding)
                {
                    if (currentNote is SliderBlock)
                    {
                        sliding = true;
                        ((SliderBlock)currentNote).StartCollapse();
                    }
                    else
                    {
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
            else if (distance > GOOD_DISTANCE && currentNote.transform.position.y < transform.position.y)
            {
                currentNote.Miss();
                currentNote = null;
            }

            if (sliding)
            {
                distance = Mathf.Abs(Vector2.Distance(transform.position, ((SliderBlock)currentNote).EndPoint));
                if (Input.GetKeyUp(code))
                {
                    if (distance > GOOD_DISTANCE_SLIDER)
                    {
                        currentNote.Miss();
                    }
                    else if (distance > PERFECT_DISTANCE_SLIDER)
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
                else if (distance > GOOD_DISTANCE_SLIDER && ((SliderBlock)currentNote).EndPoint.y < transform.position.y)
                {
                    currentNote.Miss();
                    currentNote = null;
                    sliding = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        currentNote = other.GetComponent<NoteBlock>();
    }
}
