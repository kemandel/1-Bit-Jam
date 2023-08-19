using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
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
    
    public bool botControlled;
    public float botAccuracy;
    public float botHitChance;
    private NoteBlock botQueuedInputNote;
    private float botInputRange;

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
        
        int botInput = 0;
        if (botControlled && !day) botInput = GetBotInput();

        if (currentNotes.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, currentNotes[0].transform.position);

            if (Input.GetKeyDown(code) || botInput == 1)
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
                        botQueuedInputNote = null;
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
                if (Input.GetKeyUp(code) || botInput == 2)
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

    private int GetBotInput()
    {
        if (currentNotes.Count < 1) 
        {
            botQueuedInputNote = null;
            return 0;
        }

        if (botQueuedInputNote == null)
        {
            if (Random.Range(0, 101) > botHitChance * 100)
            {
                botQueuedInputNote = currentNotes[0];
                botInputRange = 2 * GOOD_DISTANCE;
            }
            else
            {
                botQueuedInputNote = currentNotes[0];
                botInputRange = Random.Range(0, 100 *(PERFECT_DISTANCE + PERFECT_DISTANCE * (1 - botAccuracy))) / 100f;
            }
        }

        if (botQueuedInputNote != null)
        {
            if (sliding)
            {
                if (Vector2.Distance(transform.position, ((SliderBlock)botQueuedInputNote).EndPoint) < botInputRange)
                {
                    return 2;
                }
            }
            else if (Vector2.Distance(transform.position, botQueuedInputNote.transform.position) < botInputRange)
            {
                return 1;
            }
        }

        return 0;
    }

    private void UpdateUI(KeyCode code)
    {
        if (!botControlled || day)
        {
            string s = code.ToString();
            controlUI.text = s[^1].ToString();
        }
        else
        {
            controlUI.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentNotes.Add(other.GetComponent<NoteBlock>());
    }
}
