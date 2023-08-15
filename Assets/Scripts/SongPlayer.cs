using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public const float SPAWN_SPACE = 1.1875f;
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
        audio.PlayScheduled(AudioSettings.dspTime + map.fallTime - .125f);
        int i = 0;
        while (i < map.beatmap.Length)
        {
            SpawnNote(map.beatmap[i], map.fallTime);
            if (i + 1 < map.beatmap.Length)
                yield return new WaitForSeconds(map.beatmap[i+1].time - map.beatmap[i].time);
            i++;
        }
    }

    private void SpawnNote(Note note, float fallTime)
    {
        Vector3 startingPosition = new Vector3(SPAWN_SPACE * note.position - SPAWN_SPACE * (4 - .5f), NOTE_HEIGHT);

        if (note.slider)
        {
            GameObject slider = (GameObject)Resources.Load("Prefabs/SliderBlock");
            float lengthPerSecond = (startingPosition.y - FindObjectOfType<LevelManager>().hitBar.position.y) / fallTime;
            slider.GetComponent<SliderBlock>().length = lengthPerSecond * note.sliderDuration;
            slider.GetComponent<SliderBlock>().fallTime = fallTime;
            slider.transform.position = startingPosition;
            Instantiate(slider);
        }
        else
        {
            GameObject newNote = (GameObject)Resources.Load("Prefabs/NoteBlock");
            newNote.transform.position = startingPosition;
            newNote.GetComponent<NoteBlock>().fallTime = fallTime;
            Instantiate(newNote);
        }
    }
}
