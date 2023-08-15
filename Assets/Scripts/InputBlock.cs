using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBlock : MonoBehaviour
{
    public const float GOOD_DISTANCE = .5f;
    public const float PERFECT_DISTANCE = .125f;

    public bool day;
    public KeyCode DayInput;
    public KeyCode NightInput;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(DayInput))
        {
            if (other.CompareTag("Note"))
            {
                float distance = Vector2.Distance(transform.position, other.transform.position);

                if (distance > GOOD_DISTANCE)
                {
                    other.GetComponentInChildren<NoteBlock>().Miss();
                }
                else if (distance > PERFECT_DISTANCE)
                {
                    other.GetComponentInChildren<NoteBlock>().GoodHit();
                }
                else 
                {
                    other.GetComponentInChildren<NoteBlock>().PerfectHit();
                }
            }
        }
    }
}
