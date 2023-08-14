using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBlock : MonoBehaviour
{
    /// <summary>
    /// Time it takes for the note to fall
    /// </summary>
    public float fallTime;
    public Sprite spriteDay;
    public Sprite spriteNight;

    private void Start() 
    {
        StartCoroutine(FallCoroutine());
    }

    private void Update() 
    {
        Sprite newSprite = spriteNight;
        if (transform.position.x <= FindObjectOfType<LevelManager>().lineX)
        {
            newSprite = spriteDay;
        }
        FindObjectOfType<SpriteRenderer>().sprite = newSprite;
    }

    private IEnumerator FallCoroutine()
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
}
