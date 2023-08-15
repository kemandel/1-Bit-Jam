using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public const int SCORE_GOOD = 50;
    public const int SCORE_PERFECT = 100;
    public const int COMBO_DIVIDER = 100;
    public const int SLIDER_MULTI = 2;

    public Text DayScoreText;
    public Text NightScoreText;
    public Text DayComboText;
    public Text NightComboText;

    [HideInInspector]
    public float lineX;
    [HideInInspector]
    public int dayScore;
    [HideInInspector]
    public int nightScore;
    [HideInInspector]
    public int dayCombo;
    [HideInInspector]
    public int nightCombo;

    public Transform hitBar;

    private void Awake() 
    {
        lineX = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        lineX = Input.mousePosition.x / Screen.width;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SongPlayer>().PlaySong((BeatMap)Resources.Load("Beatmaps/Beatmap"));
        }

        DayScoreText.text = $"Score: {dayScore:D6}";
        DayComboText.text = $"Combo: {dayCombo}x" ;
        NightScoreText.text = $"Score: {nightScore:D6}";
        NightComboText.text = $"Combo: {nightCombo}x" ;
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
}
