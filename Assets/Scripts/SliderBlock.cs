using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SliderBlock : NoteBlock
{
    public float length;
    public float duration;

    private SpriteRenderer[] sRenderers;

    public override void Start() 
    {
        sRenderers = GetComponentsInChildren<SpriteRenderer>();

        transform.GetChild(0).transform.position = transform.position;
        transform.GetChild(2).transform.position = new Vector2(transform.position.x, transform.position.y + length);
        transform.GetChild(1).transform.position = new Vector2(transform.position.x, transform.GetChild(2).transform.position.y - transform.GetChild(0).transform.position.y + .55f);

        transform.GetChild(1).transform.localScale = new Vector3(.5f, length / 2);

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
        Debug.Log("Miss");
    }

    public override void GoodHit()
    {
        Debug.Log("Good");
    }

    public override void PerfectHit()
    {
        Debug.Log("Perfect");
    }

    public override IEnumerator FadeCoroutine(float fadeDuration)
    {
        float startTime = Time.time;
        float currentTime = 0;
        while (currentTime < startTime)
        {
            currentTime = Time.time;
            float ratio = currentTime / fadeDuration;
            float newAlpha = Mathf.Lerp(1, 0, ratio);
            foreach (SpriteRenderer sRenderer in sRenderers)
            {
                sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, newAlpha);
            }
            yield return null;
        }
    }
}
