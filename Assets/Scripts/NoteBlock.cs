using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class NoteBlock : MonoBehaviour
{
    /// <summary>
    /// Time it takes for the note to fall
    /// </summary>
    public float fallTime;
    public Sprite spriteDay;
    public Sprite spriteNight;

    public SpriteRenderer sRenderer;
    public Coroutine activeCoroutine = null;

    public virtual void Start() 
    {
        activeCoroutine = StartCoroutine(FallCoroutine());
        sRenderer = FindObjectOfType<SpriteRenderer>();
    }

    public virtual void Update() 
    {
        if (spriteDay != null && spriteNight != null)
        {
            sRenderer.sprite = spriteNight;
            if (Camera.main.WorldToScreenPoint(transform.position).x / Screen.width <= FindObjectOfType<LevelManager>().lineX)
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

    }

    public virtual void GoodHit()
    {
        
    }

    public virtual void PerfectHit()
    {
        
    }
}
