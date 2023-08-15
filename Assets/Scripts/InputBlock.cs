using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InputBlock : MonoBehaviour
{
    public const float GOOD_DISTANCE = .5f;
    public const float PERFECT_DISTANCE = .125f;

    public bool day;
    public KeyCode DayInput;
    public KeyCode NightInput;

    private NoteBlock currentNote = null;

    private void Update() 
    {
        day = false;
        if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX)
        {
            day = true;
        }

        if (currentNote != null)
        {
            KeyCode code = day ? DayInput : NightInput;
            if (Input.GetKeyDown(code))
            {
                float distance = Vector2.Distance(transform.position, currentNote.transform.position);
                Debug.Log("Distance: " + distance);

                if (distance > GOOD_DISTANCE)
                {
                    Debug.Log("Miss");
                    currentNote.Miss();
                }
                else if (distance > PERFECT_DISTANCE)
                {
                    Debug.Log("Good");
                    currentNote.GoodHit();
                }
                else
                {
                    Debug.Log("Perfect");
                    currentNote.PerfectHit();
                }
                currentNote = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Note Near Input");
        currentNote = other.GetComponent<NoteBlock>();
    }
}
