using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public const float SPAWN_SPACE = 1.5f;
    public const float NOTE_HEIGHT = 8.6875f;

    public void PlaySong(BeatMap map)
    {
        StartCoroutine(SongCoroutine(map.beatmap));
    }

    private IEnumerator SongCoroutine(Note[] map)
    {
        int i = 0;
        while (i < map.Length)
        {
            SpawnNote(map[i].position);
            try
            {
                yield return new WaitForSeconds(map[i+1].time - map[i].time);
            }
            finally { i++; }
        }
    }

    private void SpawnNote(int posX)
    {
        GameObject note = (GameObject)Resources.Load("Prefabs/NoteBlock");
        note.transform.position = new Vector3(SPAWN_SPACE * posX - SPAWN_SPACE * (4 - .5f), NOTE_HEIGHT);
    }
}
