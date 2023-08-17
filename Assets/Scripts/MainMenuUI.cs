using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public BeatMap song1;
    public BeatMap song2;

    public Canvas gameCanvas;

    public Button playButton;
    public Button helpButton;
    public Button creditsButton;
    public Button backButton;
    public Button onePlayerButton;
    public Button twoPlayerButton;
    public Button song1Button;
    public Button song2Button;

    public Image helpInfograph;
    public Image creditsInfograph;

    public Text selectSongText;

    private int currentScreen;
    private bool twoPlayer;

    private void Start()
    {
        gameCanvas.gameObject.SetActive(false);
        if (song1 != null) song1Button.GetComponentInChildren<Text>().text = song1.songName;
        if (song2 != null) song1Button.GetComponentInChildren<Text>().text = song2.songName;
    }
    
    public void RestartUI()
    {
        playButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);

        currentScreen = 0;
    }

    public void PlayButton()
    {
        playButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        onePlayerButton.gameObject.SetActive(true);
        twoPlayerButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        currentScreen = 1;
    }

    public void HelpButton()
    {
        playButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        helpInfograph.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        currentScreen = 2;
    }

    public void CreditsButton()
    {
        playButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        creditsInfograph.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        currentScreen = 3;
    }

    public void OnePlayerButton()
    {
        twoPlayer = false;
        SongsScreen();
    }

    public void TwoPlayerButton()
    {
        twoPlayer = true;
        SongsScreen();
    }

    private void SongsScreen()
    {
        onePlayerButton.gameObject.SetActive(false);
        twoPlayerButton.gameObject.SetActive(false);

        selectSongText.gameObject.SetActive(true);
        song1Button.gameObject.SetActive(true);
        song2Button.gameObject.SetActive(true);

        currentScreen = 4;
    }

    public void Song1Button()
    {
        selectSongText.gameObject.SetActive(false);
        song1Button.gameObject.SetActive(false);
        song2Button.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        FindObjectOfType<LevelManager>().StartSong(twoPlayer, song1);
    }

    public void Song2Button()
    {
        selectSongText.gameObject.SetActive(false);
        song1Button.gameObject.SetActive(false);
        song2Button.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        FindObjectOfType<LevelManager>().StartSong(twoPlayer, song2);
    }

    public void BackButton()
    {
        switch (currentScreen)
        {
            case 1:
                playButton.gameObject.SetActive(true);
                helpButton.gameObject.SetActive(true);
                creditsButton.gameObject.SetActive(true);

                onePlayerButton.gameObject.SetActive(false);
                twoPlayerButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);

                currentScreen = 0;
                break;
            case 2:
                playButton.gameObject.SetActive(true);
                helpButton.gameObject.SetActive(true);
                creditsButton.gameObject.SetActive(true);

                helpInfograph.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);

                currentScreen = 0;
                break;
            case 3:
                playButton.gameObject.SetActive(true);
                helpButton.gameObject.SetActive(true);
                creditsButton.gameObject.SetActive(true);

                creditsInfograph.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);

                currentScreen = 0;
                break;
            case 4:
                onePlayerButton.gameObject.SetActive(true);
                twoPlayerButton.gameObject.SetActive(true);

                selectSongText.gameObject.SetActive(false);
                song1Button.gameObject.SetActive(false);
                song2Button.gameObject.SetActive(false);

                currentScreen = 1;
                break;
            default:
                break;
        }
    }
}
