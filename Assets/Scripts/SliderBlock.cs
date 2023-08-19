using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SliderBlock : NoteBlock
{
    public float length;
    public float duration;

    public Vector3 EndPoint { get { return endPoint; } }
    private Vector3 endPoint;

    private SpriteRenderer[] sRenderers;

    public override void Start() 
    {
        sRenderers = GetComponentsInChildren<SpriteRenderer>();

        transform.GetChild(0).position = transform.position;
        transform.GetChild(2).position = new Vector2(transform.position.x, transform.position.y + length);

        transform.GetChild(1).localPosition = new Vector2(0, (transform.GetChild(2).transform.localPosition.y - transform.GetChild(0).transform.localPosition.y) / 2);
        transform.GetChild(1).localScale = new Vector3(.5f, length / 2);

        activeCoroutine = StartCoroutine(FallCoroutine());
    }

    public override void Update() 
    {
        day = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().LineX;

        endPoint = transform.GetChild(2).position;
        if (spriteDay != null && spriteNight != null)
        {
            foreach (SpriteRenderer sr in sRenderers)
            {
                if (sr.sprite == spriteNight || sr.sprite == spriteDay)
                {
                    sr.sprite = spriteNight;
                    if (day)
                    {
                        sr.sprite = spriteDay;
                    }
                }
            }
        }
    }

    public void StartCollapse()
    {
        StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(CollapseCoroutine());
    }

    public override void Miss()
    {
        StopCoroutine(activeCoroutine);

        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(FadeCoroutine(FADE_DURATION, sRenderers[i]));
        }

        FindObjectOfType<LevelManager>().ComboBreak(day ? 0 : 1);
    }

    public override void GoodHit()
    {
        StopCoroutine(activeCoroutine);

        sRenderers[0].enabled = false;
        sRenderers[1].enabled = false;

        StartCoroutine(FadeCoroutine(FADE_DURATION, sRenderers[2]));

        FindObjectOfType<LevelManager>().AddScore(day ? 0 : 1, LevelManager.SCORE_GOOD, true);
    }

    public override void PerfectHit()
    {
        StopCoroutine(activeCoroutine);

        sRenderers[0].enabled = false;
        sRenderers[1].enabled = false;

        sRenderers[2].sprite = altSpriteNight;
        if (day)
        {
            sRenderers[2].sprite = altSpriteDay;
        }

        StartCoroutine(FadeCoroutine(FADE_DURATION, sRenderers[2]));
        StopCoroutine(activeCoroutine);

        FindObjectOfType<LevelManager>().AddScore(day ? 0 : 1, LevelManager.SCORE_PERFECT, true);
    }

    private IEnumerator CollapseCoroutine()
    {
        float startTime = Time.time;
        float startingPosY = transform.GetChild(2).position.y;
        float goalPosY = transform.position.y;
        while (transform.GetChild(1).localScale.y > 0)
        {
            float timeFalling = Time.time - startTime;
            float ratio = timeFalling / duration;
            float newY = Mathf.LerpUnclamped(startingPosY, goalPosY, ratio);
            // Adjust end
            transform.GetChild(2).position = new Vector2(transform.position.x, newY);
            // Adjust bridge
            transform.GetChild(1).localPosition = new Vector2(0, (transform.GetChild(2).transform.localPosition.y - transform.GetChild(0).transform.localPosition.y) / 2);
            transform.GetChild(1).localScale = new Vector3(.5f, (transform.GetChild(2).transform.position.y - transform.GetChild(0).transform.position.y) / 2);
            yield return null;
        }
        yield return new WaitForSeconds((fallTime / (SongPlayer.NOTE_HEIGHT - FindObjectOfType<LevelManager>().hitBar.position.y)) * InputBlock.GOOD_DISTANCE_SLIDER);
        foreach (InputBlock input in FindObjectsOfType<InputBlock>())
        {
            if (input.currentNotes.Count > 0 && input.currentNotes[0] == this)
            {
                input.currentNotes.RemoveAt(0);
                input.sliding = false;
            }
        }
        Miss();
    }
}
