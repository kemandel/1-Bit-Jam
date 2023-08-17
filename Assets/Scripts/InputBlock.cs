using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InputBlock : MonoBehaviour
{
    public const float GOOD_DISTANCE = .5f;
    public const float PERFECT_DISTANCE = .125f;

    public const float GOOD_DISTANCE_SLIDER = 1f;
    public const float PERFECT_DISTANCE_SLIDER = .25f;

    public bool day;
    public KeyCode DayInput;
    public KeyCode NightInput;
    public Text controlUI;

    public List<NoteBlock> currentNotes = new List<NoteBlock>();
    [HideInInspector]
    public bool sliding = false;

    private void Update()
    {
        day = false;
        if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().LineX)
        {
            day = true;
        }

        KeyCode code = day ? DayInput : NightInput;

        UpdateUI(code);

        if (currentNotes.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, currentNotes[0].transform.position);
            if (Input.GetKeyDown(code))
            {
                if (!sliding)
                {
                    if (currentNotes[0] is SliderBlock)
                    {
                        sliding = true;
                        ((SliderBlock)currentNotes[0]).StartCollapse();
                    }
                    else
                    {
                        if (distance > GOOD_DISTANCE)
                        {
                            currentNotes[0].Miss();
                        }
                        else if (distance > PERFECT_DISTANCE)
                        {
                            currentNotes[0].GoodHit();
                        }
                        else
                        {
                            currentNotes[0].PerfectHit();
                        }
                        currentNotes.RemoveAt(0);
                    }
                }
            }
            else if (distance > GOOD_DISTANCE && currentNotes[0].transform.position.y < transform.position.y)
            {
                currentNotes[0].Miss();
                currentNotes.RemoveAt(0);
            }

            if (sliding && currentNotes.Count > 0 && currentNotes[0] is SliderBlock)
            {
                distance = Mathf.Abs(Vector2.Distance(transform.position, ((SliderBlock)currentNotes[0]).EndPoint));
                if (Input.GetKeyUp(code))
                {
                    if (distance > GOOD_DISTANCE_SLIDER)
                    {
                        currentNotes[0].Miss();
                    }
                    else if (distance > PERFECT_DISTANCE_SLIDER)
                    {
                        currentNotes[0].GoodHit();
                    }
                    else
                    {
                        currentNotes[0].PerfectHit();
                    }
                    currentNotes.RemoveAt(0);
                    sliding = false;
                }
                else if (distance > GOOD_DISTANCE_SLIDER && ((SliderBlock)currentNotes[0]).EndPoint.y < transform.position.y)
                {
                    currentNotes[0].Miss();
                    currentNotes.RemoveAt(0);
                    sliding = false;
                }
            }
            else if (currentNotes.Count > 0) sliding = false;
        }
    }

    private void UpdateUI(KeyCode code)
    {
        string s = code.ToString();
        controlUI.text = s[^1].ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentNotes.Add(other.GetComponent<NoteBlock>());
    }
}
