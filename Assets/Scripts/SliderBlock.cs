using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SliderBlock : NoteBlock
{
    public float length;

    private SpriteRenderer[] sRenderers;

    public override void Start() 
    {
        sRenderers = GetComponentsInChildren<SpriteRenderer>();

        transform.GetChild(0).transform.position = transform.position;
        transform.GetChild(2).transform.position = new Vector2(transform.position.x, transform.position.y + length);
        transform.GetChild(1).transform.position = transform.GetChild(2).transform.position - transform.GetChild(0).transform.position;

        transform.GetChild(1).transform.localScale = new Vector3(1, length / 2);

        activeCoroutine = StartCoroutine(FallCoroutine());
    }

    public override void Update() 
    {
        if (spriteDay != null && spriteNight != null)
        {
            foreach (SpriteRenderer sr in sRenderers)
            {
                if (sr.sprite == spriteNight || sr.sprite == spriteDay)
                {
                    sr.sprite = spriteNight;
                    if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX)
                    {
                        sr.sprite = spriteDay;
                    }
                }
            }
        }
    }

    public override void Miss()
    {

    }

    public override void GoodHit()
    {
        
    }

    public override void PerfectHit()
    {
        
    }
}
