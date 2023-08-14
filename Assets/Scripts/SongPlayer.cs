using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public const float SPAWN_SPACE = 1.5f;
    public const float NOTE_HEIGHT = 8.6875f;

    public void PlaySong(BeatMap map)
    {
        StartCoroutine(SongCoroutine(map));
    }

    private IEnumerator SongCoroutine(BeatMap map)
    {
        yield return new WaitForSeconds(1f);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = map.song;
        audio.PlayScheduled(AudioSettings.dspTime + map.fallTime);
        int i = 0;
        while (i < map.beatmap.Length)
        {
            SpawnNote(map.beatmap[i].position, map.fallTime);
            if (i + 1 < map.beatmap.Length)
                yield return new WaitForSeconds(map.beatmap[i+1].time - map.beatmap[i].time);
            i++;
        }
    }

    private void SpawnNote(int posX, float fallTime)
    {
        GameObject note = (GameObject)Resources.Load("Prefabs/NoteBlock");
        note.transform.position = new Vector3(SPAWN_SPACE * posX - SPAWN_SPACE * (4 - .5f), NOTE_HEIGHT);
        note.GetComponent<NoteBlock>().fallTime = fallTime;
        Instantiate(note);
    }
}
