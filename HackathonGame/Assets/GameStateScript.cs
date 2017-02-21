using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{
    [SerializeField] int player1Score;
    [SerializeField] int player2Score;
    [SerializeField] int scoreMax;
    [SerializeField] bool paused;
    [SerializeField] Transform[] objectsToPause;

    [SerializeField] Transform disc1;
    [SerializeField] Transform disc2;

    [SerializeField] Transform player1;
    [SerializeField] Transform player2;

    [SerializeField] Transform catchBox1;
    [SerializeField] Transform catchBox2;

    [SerializeField] int countDown = 100;
    [SerializeField] float timeInterval = 1;
    float timer = 0;

    [SerializeField] GameObject TitleScreen;
    Image TitleScreenBackground;
    [SerializeField] GameObject TitleScreenElements;
    [SerializeField] GameObject PauseScreen;

    [SerializeField] Text scoreDisplay1;
    [SerializeField] Text scoreDisplay2;
    [SerializeField] Text timerDisplay;
    [SerializeField] Slider scoreSlider1;
    [SerializeField] Slider scoreSlider2;

    [FMODUnity.EventRef]
    [SerializeField] string musicTrackName = "event:/Music/Music";
    FMOD.Studio.EventInstance musicEvent;
    FMOD.Studio.ParameterInstance pauseMenu;
    FMOD.Studio.ParameterInstance points;
    FMOD.Studio.ParameterInstance inGame;
    FMOD.Studio.ParameterInstance themeDecision;

    // Use this for initialization
    void Start ()
    {
        PauseScreen.SetActive(false);
        TitleScreenBackground = TitleScreen.GetComponent<Image>();
        //paused = true;
        for (int i = 0; i < objectsToPause.Length; i++)
        {
            objectsToPause[i].GetComponent<CatchScript>().gamePaused = true;
            objectsToPause[i].GetComponent<PlayerRotationScript>().gamePaused = true;
        }
        Time.timeScale = 0;
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(musicTrackName);
        musicEvent.getParameter("Pause menu", out pauseMenu);
        musicEvent.getParameter("Points", out points);
        musicEvent.getParameter("In/Out Of Game", out inGame);
        musicEvent.getParameter("Theme Vs", out themeDecision);
        pauseMenu.setValue(0f);
        points.setValue((player1Score + player2Score) / 2);
        //inGame.setValue(1f);
        themeDecision.setValue(1f);
        musicEvent.start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        PauseGame();
        UpdateTimer();
        if (countDown <= 0)
        {
            EndGame();
        }
        if (player1Score >= scoreMax || player2Score >= scoreMax)
        {
            EndGame();
        }
        if (TitleScreenElements.activeSelf == false)
        {
            points.setValue((player1Score + player2Score) / 2);
            inGame.setValue(1);
            if (TitleScreenBackground.color.a > 0)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a -= Time.deltaTime;
                TitleScreenBackground.color = fadeColor;
            }
            else
            {
                TitleScreen.SetActive(false);
                
            }
        }
        else
        {
            inGame.setValue(0);
            TitleScreen.SetActive(true);
            if (TitleScreenBackground.color.a < 1)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a += Time.deltaTime;
                Debug.Log(fadeColor.a);
                TitleScreenBackground.color = fadeColor;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
	}

    public void UpdateScore(bool player1, int score)
    {
        if (player1 == true)
        {
            if (player1Score < scoreMax)
            {
                player1Score = score;
                scoreDisplay1.text = score.ToString();
                scoreSlider1.value = score;
                points.setValue((player1Score + player2Score) / 2);
                if (player1Score > player2Score)
                {
                    themeDecision.setValue(0f);
                }
                else if (player1Score < player2Score)
                {
                    themeDecision.setValue(2f);
                }
                else
                {
                    themeDecision.setValue(1f);
                }
            }
        }
        else
        {
            if (player2Score < scoreMax)
            {
                player2Score = score;
                scoreDisplay2.text = score.ToString();
                scoreSlider2.value = score;
                points.setValue((player1Score + player2Score) / 2);
                if (player1Score > player2Score)
                {
                    themeDecision.setValue(0f);
                }
                else if (player1Score < player2Score)
                {
                    themeDecision.setValue(2f);
                }
                else
                {
                    themeDecision.setValue(1f);
                }
            }
        }
    }

    void PauseGame()
    {
        if (Input.GetButtonDown("Pause1") || Input.GetButtonDown("Pause2"))
        {
            if (paused == false)
            {
                Time.timeScale = 0;
                for (int i = 0; i < objectsToPause.Length; i ++)
                {
                    objectsToPause[i].GetComponent<CatchScript>().gamePaused = true;
                    objectsToPause[i].GetComponent<PlayerRotationScript>().gamePaused = true;
                }
                PauseScreen.SetActive(true);
                pauseMenu.setValue(1);
                paused = true;
            }
            else
            {
                Time.timeScale = 1;
                for (int i = 0; i < objectsToPause.Length; i++)
                {
                    objectsToPause[i].GetComponent<CatchScript>().gamePaused = false;
                    objectsToPause[i].GetComponent<PlayerRotationScript>().gamePaused = false;
                }
                PauseScreen.SetActive(false);
                pauseMenu.setValue(0);
                paused = false;
            }
        }
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer > timeInterval && countDown > 0)
        {
            timer = 0;
            countDown--;
            timerDisplay.text = countDown.ToString();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        for (int i = 0; i < objectsToPause.Length; i++)
        {
            objectsToPause[i].GetComponent<CatchScript>().gamePaused = false;
            objectsToPause[i].GetComponent<PlayerRotationScript>().gamePaused = false;
        }
        paused = false;
        TitleScreenElements.SetActive(false);
        
    }

    public void EndGame()
    {
        for (int i = 0; i < objectsToPause.Length; i++)
        {
            objectsToPause[i].GetComponent<CatchScript>().gamePaused = true;
            objectsToPause[i].GetComponent<PlayerRotationScript>().gamePaused = true;
        }
        TitleScreenElements.SetActive(true);
        ResetGame();
    }

    void ResetGame()
    {
        //Reset Player Position
        player1.position = new Vector3(-10, 1, 0);
        player1.rotation.eulerAngles.Set(0, -90, 0);
        player2.position = new Vector3(10, 1, 0);
        player2.rotation.eulerAngles.Set(0, 90, 0);

        player1Score = 0;
        player2Score = 0;
        scoreDisplay1.text = 0.ToString();
        scoreDisplay2.text = 0.ToString();
        scoreSlider1.value = 0;
        scoreSlider2.value = 0;
        timerDisplay.text = 120.ToString();
        countDown = 120;
        timer = 0;

        disc1.GetComponent<DiscScript>().SetValues(0, 500);
        disc2.GetComponent<DiscScript>().SetValues(0, 500);
        disc1.GetComponent<DiscScript>().CatchDisc(catchBox1);
        disc2.GetComponent<DiscScript>().CatchDisc(catchBox2);
        catchBox1.GetComponent<CatchScript>().ResetValues();
        catchBox2.GetComponent<CatchScript>().ResetValues();
        //disc1.parent = player1;
        //disc2.parent = player2;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
