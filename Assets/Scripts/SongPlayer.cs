using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public const float SPAWN_SPACE = 1.1875f;
    public const float NOTE_HEIGHT = 8.6875f;
    public const float SONG_LOAD_TIME = 1f;
    public const float SONG_END_TIME = 2f;

    private float songTime;
    private bool songPlaying = false;
    private float startVolume;

    private void Start() {
        startVolume = GetComponent<AudioSource>().volume;
    }

    public void PlaySong(BeatMap map)
    {
        StartCoroutine(PlaySongCoroutine(map.fallTime, map));
        StartCoroutine(PlayNoteCoroutine(map));
    }

    private IEnumerator PlayNoteCoroutine(BeatMap map)
    {
        int i = 0;
        while (i < map.beatmap.Length)
        {
            if (songPlaying && songTime >= map.beatmap[i].time - map.fallTime)
            {
                if (map.beatmap[i].LineCheck)
                {
                    if (!map.beatmap[i].slider)
                        StartCoroutine(CheckLineCoroutine(map.fallTime));
                    else
                        StartCoroutine(CheckLineCoroutine(map.fallTime + map.beatmap[i].sliderDuration));
                }
                SpawnNote(map.beatmap[i], map.fallTime);
                i++;
            }
            yield return null;
        }

        while (songPlaying)
        {
            yield return null;
        }

        StartCoroutine(FindObjectOfType<LevelManager>().EndSongCoroutine(SONG_END_TIME + map.fallTime));
    }

    private IEnumerator PlaySongCoroutine(float delay, BeatMap map)
    {
        AudioSource audio = GetComponent<AudioSource>();
        StartCoroutine(FadeAudioCoroutine(audio, SONG_LOAD_TIME));
        yield return new WaitForSeconds(SONG_LOAD_TIME);
        audio.clip = map.song;
        audio.PlayDelayed(delay);
        audio.volume = map.songVolume;
        audio.loop = false;
        songPlaying = true;

        float startTime = Time.time;
        float currentTime = 0;

        while (currentTime < delay)
        {
            songTime = currentTime - delay;
            yield return null;
            currentTime = Time.time - startTime;
        }

        while (audio.isPlaying)
        {
            songTime = audio.time - delay;
            yield return null;
        }
        songPlaying = false;
    }

    private IEnumerator FadeAudioCoroutine(AudioSource audio, float delay)
    {
        float startTime = Time.time;
        float currentTime = 0;

        while (currentTime < delay)
        {
            yield return null;
            currentTime = Time.time - startTime;
            float ratio = currentTime / delay;
            audio.volume = Mathf.Lerp(startVolume, 0, ratio);
        }
    }

    private IEnumerator CheckLineCoroutine(float fallTime)
    {
        yield return new WaitForSeconds(fallTime + fallTime / 12);
        FindObjectOfType<LevelManager>().CheckLine();
    }

    private void SpawnNote(Note note, float fallTime)
    {
        Debug.Log("Song Time: " + songTime + " Note Time: " + (note.time - fallTime));
        //Debug.Log("Desync: " + Mathf.Abs(FindObjectOfType<AudioSource>().time - note.time + fallTime));
        Vector3 startingPosition = new Vector3(SPAWN_SPACE * note.position - SPAWN_SPACE * (4 - .5f), NOTE_HEIGHT);

        if (note.slider)
        {
            GameObject slider = (GameObject)Resources.Load("Prefabs/SliderBlock");
            float lengthPerSecond = (startingPosition.y - FindObjectOfType<LevelManager>().hitBar.position.y) / fallTime;
            slider.GetComponent<SliderBlock>().length = lengthPerSecond * note.sliderDuration;
            slider.GetComponent<SliderBlock>().fallTime = fallTime;
            slider.GetComponent<SliderBlock>().duration = note.sliderDuration;
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
