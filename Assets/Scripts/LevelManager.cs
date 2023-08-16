using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public const int SCORE_GOOD = 50;
    public const int SCORE_PERFECT = 100;
    public const int COMBO_DIVIDER = 100;
    public const int SLIDER_MULTI = 2;

    public const int COMBO_DIFFERENCE_NUM_1 = 3;
    public const int COMBO_DIFFERENCE_NUM_2 = 10;
    public const float COMBO_DIFFERENCE_RATIO_1 = 1.5f;
    public const float COMBO_DIFFERENCE_RATIO_2 = 2.0f;

    public const float LINE_MOVE_TIME = 1f;

    public bool moveMouse = false;

    public Text DayScoreText;
    public Text NightScoreText;
    public Text DayComboText;
    public Text NightComboText;

    public float LineX { get { return lineX; } }
    
    [HideInInspector]
    public int dayScore;
    [HideInInspector]
    public int nightScore;
    [HideInInspector]
    public int dayCombo;
    [HideInInspector]
    public int nightCombo;

    public Transform hitBar;

    private float lineX;

    private void Awake()
    {
        lineX = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMouse)
        {
            lineX = Input.mousePosition.x / Screen.width;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SongPlayer>().PlaySong((BeatMap)Resources.Load("Beatmaps/EnterTheLair"));
        }

        DayScoreText.text = $"Score: {dayScore:D6}";
        DayComboText.text = $"Combo: {dayCombo}x";
        NightScoreText.text = $"Score: {nightScore:D6}";
        NightComboText.text = $"Combo: {nightCombo}x";
    }

    /// <summary>
    /// Adds a set score to the player. (0 for player 1 or "day", 1 for player 2 or "night")
    /// </summary>
    /// <param name="player"></param>
    /// <param name="score"></param>
    public void AddScore(int player, int score)
    {
        if (player == 0)
        {
            dayScore += Mathf.RoundToInt(score + score * (((float)dayCombo) / COMBO_DIVIDER));
            dayCombo++;
            return;
        }
        nightScore += Mathf.RoundToInt(score + score * (((float)nightCombo) / COMBO_DIVIDER));
        nightCombo++;
    }

    /// <summary>
    /// Breaks a players combo, setting it to 0. (0 for player 1 or "day", 1 for player 2 or "night")
    /// </summary>
    /// <param name="player"></param>
    public void ComboBreak(int player)
    {
        if (player == 0)
        {
            dayCombo = 0;
            return;
        }
        nightCombo = 0;
    }

    public void CheckLine()
    {
        float delta = Mathf.Abs(dayCombo - nightCombo);
        float ratio = dayCombo >= nightCombo ? (dayCombo / (nightCombo > 0 ? nightCombo : 1)) : (nightCombo / (dayCombo > 0 ? dayCombo : 1));
        float newX = 0;

        if (delta >= COMBO_DIFFERENCE_NUM_2 && ratio >= COMBO_DIFFERENCE_RATIO_2)
        {
            newX = dayScore > nightScore ? SongPlayer.SPAWN_SPACE * 2 : -SongPlayer.SPAWN_SPACE * 2;
        }
        else if (delta > COMBO_DIFFERENCE_NUM_1 && ratio >= COMBO_DIFFERENCE_RATIO_1)
        {
            newX = dayScore > nightScore ? SongPlayer.SPAWN_SPACE : -SongPlayer.SPAWN_SPACE;
        }

        if (lineX != newX)
            StartCoroutine(MoveLineCoroutine(Camera.main.WorldToScreenPoint(new Vector2(newX, 0)).x / Screen.width, LINE_MOVE_TIME));
    }

    public IEnumerator MoveLineCoroutine(float newXPos, float moveTime)
    {
        float startTime = Time.time;
        float currentTime = 0;
        float oldXPos = newXPos;
        while (currentTime <= moveTime)
        {
            currentTime = Time.time - startTime;
            float ratio = currentTime / moveTime;
            lineX = Mathf.Lerp(oldXPos, newXPos, ratio);
            Debug.Log("Line Position: " + lineX);
            yield return null;
        }
    }
}
