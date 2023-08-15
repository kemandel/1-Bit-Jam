using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class NoteBlock : MonoBehaviour
{
    public const float FADE_DURATION = .5f;
    /// <summary>
    /// Time it takes for the note to fall
    /// </summary>
    public float fallTime;
    public Sprite spriteDay;
    public Sprite spriteNight;
    public Sprite altSpriteDay;
    public Sprite altSpriteNight;

    public Coroutine activeCoroutine = null;
    [HideInInspector]
    public bool day;

    private SpriteRenderer sRenderer;


    public virtual void Start() 
    {
        activeCoroutine = StartCoroutine(FallCoroutine());
        sRenderer = FindObjectOfType<SpriteRenderer>();
    }

    public virtual void Update() 
    {
        day = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX;
        if (sRenderer.sprite == spriteDay || sRenderer.sprite == spriteNight)
        {
            sRenderer.sprite = spriteNight;
            if (day)
            {
                sRenderer.sprite = spriteDay;
            }
        }
    }

    public IEnumerator FallCoroutine()
    {
        float startTime = Time.time;
        float startingPosY = transform.position.y;
        float goalPosY = FindObjectOfType<LevelManager>().hitBar.transform.position.y;
        while (true)
        {
            float timeFalling = Time.time - startTime;
            float ratio = timeFalling / fallTime;
            float newY = Mathf.LerpUnclamped(startingPosY, goalPosY, ratio);
            transform.position = new Vector2(transform.position.x, newY);
            yield return null;
        }
    }

    public virtual void Miss()
    {
        Debug.Log("Miss");

        FindObjectOfType<LevelManager>().ComboBreak(day ? 0 : 1);
    }

    public virtual void GoodHit()
    {
        Debug.Log("Good");
        StopCoroutine(activeCoroutine);
        StartCoroutine(FadeCoroutine(FADE_DURATION, sRenderer));
        
        FindObjectOfType<LevelManager>().AddScore(day ? 0 : 1, LevelManager.SCORE_GOOD);
    }

    public virtual void PerfectHit()
    {
        Debug.Log("Perfect");
        sRenderer.sprite = altSpriteNight;
        if (day)
        {
            sRenderer.sprite = altSpriteDay;
        }

        StopCoroutine(activeCoroutine);
        StartCoroutine(FadeCoroutine(FADE_DURATION, sRenderer));

        FindObjectOfType<LevelManager>().AddScore(day ? 0 : 1, LevelManager.SCORE_PERFECT);
    }

    public virtual IEnumerator FadeCoroutine(float fadeDuration, SpriteRenderer sRenderer)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        float startTime = Time.time;
        float passedTime = 0;
        while(passedTime < startTime)
        {
            passedTime = Time.time - startTime;
            float ratio = passedTime / fadeDuration;
            float newAlpha = Mathf.Lerp(1, 0, ratio);
            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, newAlpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
