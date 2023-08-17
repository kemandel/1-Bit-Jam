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

    public const int COMBO_DIFFERENCE_NUM_1 = 20;
    public const int COMBO_DIFFERENCE_NUM_2 = 50;
    public const float COMBO_DIFFERENCE_RATIO_1 = 1.5f;
    public const float COMBO_DIFFERENCE_RATIO_2 = 2.0f;

    public const int MISS_VALUE = 1;
    public const float MISS_DURATION = .5f;

    public const float LINE_MOVE_TIME = 1f;

    public bool moveMouse = true;

    public Text DayScoreText;
    public Text NightScoreText;
    public Text DayComboText;
    public Text NightComboText;

    public Text gameOverText;

    public GameObject DragonDay;
    public GameObject DragonNight;

    public GameObject MissDay;
    public GameObject MissNight;

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

    public AudioClip menuTheme;

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

        DayScoreText.text = $"Score: {dayScore:D6}";
        DayComboText.text = $"Combo: {dayCombo}x";
        NightScoreText.text = $"Score: {nightScore:D6}";
        NightComboText.text = $"Combo: {nightCombo}x";
    }

    /// <summary>
    /// Starts the selected song on the chosen mode
    /// </summary>
    public void StartSong(bool twoPlayer, BeatMap map)
    {
        dayScore = nightScore = dayCombo = nightCombo = 0;
        moveMouse = false;
        Cursor.visible = false;
        GetComponent<SongPlayer>().PlaySong(map);
        lineX = .5f;
    }

    public IEnumerator EndSongCoroutine(float delay)
    {
        gameOverText.gameObject.SetActive(true);
        string text = dayScore > nightScore ? "Player 1" : "Player 2";
        gameOverText.text = text + " Wins!";
        lineX = dayScore > nightScore ? 1f : 0;
        yield return new WaitForSeconds(delay);
        moveMouse = true;
        Cursor.visible = true;
        GetComponent<AudioSource>().clip = menuTheme;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
        FindObjectOfType<MainMenuUI>().RestartUI();
        FindObjectOfType<MainMenuUI>().gameCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Adds a set score to the player. (0 for player 1 or "day", 1 for player 2 or "night")
    /// </summary>
    /// <param name="player"></param>
    /// <param name="score"></param>
    public void AddScore(int player, int score, bool slider)
    {
        if (player == 0)
        {
            dayScore += Mathf.RoundToInt(score + score * (((float)dayCombo) / COMBO_DIVIDER)) * (slider ? SLIDER_MULTI : 1);
            dayCombo += 1 + (slider ? 1 : 0);
            return;
        }
        nightScore += Mathf.RoundToInt(score + score * (((float)nightCombo) / COMBO_DIVIDER)) * (slider ? SLIDER_MULTI : 1);
        nightCombo += 1 + (slider ? 1 : 0);
    }

    /// <summary>
    /// Breaks a players combo, setting it to 0. (0 for player 1 or "day", 1 for player 2 or "night")
    /// </summary>
    /// <param name="player"></param>
    public void ComboBreak(int player)
    {
        if (player == 0)
        {
            if (dayCombo >= MISS_VALUE) StartCoroutine(MissAnim(MISS_DURATION, player));
            dayCombo = 0;
            return;
        }
        if (nightCombo >= MISS_VALUE) StartCoroutine(MissAnim(MISS_DURATION, player));
        nightCombo = 0;
    }

    public IEnumerator MissAnim(float duration, int player)
    {
        if (player == 0)
        {
            StartCoroutine(MissAnimWord(MISS_DURATION * 2, MissDay));
            DragonDay?.GetComponent<Animator>().SetBool("ShowMiss", true);
            yield return new WaitForSeconds(duration);
            DragonDay?.GetComponent<Animator>().SetBool("ShowMiss", false);
            yield break;
        }

        StartCoroutine(MissAnimWord(MISS_DURATION * 2, MissNight));
        DragonNight?.GetComponent<Animator>().SetBool("ShowMiss", true);
        yield return new WaitForSeconds(duration);
        DragonNight?.GetComponent<Animator>().SetBool("ShowMiss", false);
    }

    public IEnumerator MissAnimWord(float fadeDuration, GameObject missSign)
    {
        missSign.GetComponent<Animator>().SetBool("ShowMiss", true);
        SpriteRenderer sRenderer = missSign.GetComponent<SpriteRenderer>();
        SpriteRenderer sRenderer2 = missSign.GetComponentsInChildren<SpriteRenderer>()[1];
        yield return new WaitForSeconds(MISS_DURATION);
        float startTime = Time.time;
        Color originalColor = sRenderer.color;
        Color originalColor2 = sRenderer2.color;
        while(sRenderer.color.a > 0 && sRenderer2.color.a > 0)
        {
            float passedTime = Time.time - startTime;
            float ratio = passedTime / fadeDuration;
            float newAlpha = Mathf.Lerp(1, 0, ratio);
            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, newAlpha);
            sRenderer2.color = new Color(sRenderer2.color.r, sRenderer2.color.g, sRenderer2.color.b, newAlpha);
            yield return null;
        }
        missSign.GetComponent<Animator>().SetBool("ShowMiss", false);
        sRenderer.color = originalColor;
        sRenderer2.color = originalColor2;
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
        float oldXPos = lineX;
        while (currentTime <= moveTime)
        {
            currentTime = Time.time - startTime;
            float ratio = currentTime / moveTime;
            lineX = Mathf.Lerp(oldXPos, newXPos, ratio);
            yield return null;
        }
    }
}
