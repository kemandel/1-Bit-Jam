using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    /// <summary>
    /// Time it takes for the note to fall
    /// </summary>
    public float fallTime;

    private bool player1;

    private void Start() 
    {
        StartCoroutine(FallCoroutine());
    }

    private void Update() 
    {
        // Choose which player can hit the note
        if (transform.position.x < FindObjectOfType<LevelManager>().lineX)
            player1 = true;
        else player1 = false;
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
