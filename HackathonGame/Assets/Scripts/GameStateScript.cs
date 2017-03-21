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

    [SerializeField] ArrayList players;
    [SerializeField] ArrayList discs;
    [SerializeField] Transform debugPlayerToAdd1;
    [SerializeField] Transform debugPlayerToAdd2;

    //[SerializeField] Transform disc1;
    //[SerializeField] Transform disc2;

    //[SerializeField] Transform player1;
    //[SerializeField] Transform player2;

    //[SerializeField] Transform catchBox1;
    //[SerializeField] Transform catchBox2;

    [SerializeField] int countDown = 100;
    [SerializeField] float timeInterval = 1;
    float timer = 0;

    [SerializeField] GameObject TitleScreen;
    Image TitleScreenBackground;
    [SerializeField] GameObject TitleScreenElements;
    [SerializeField] GameObject PauseScreen;

    [SerializeField] Text timerDisplay;

    // Use this for initialization
    void Start ()
    {
        players = new ArrayList();
        AddPlayer(debugPlayerToAdd1);
        AddPlayer(debugPlayerToAdd1);
        PauseScreen.SetActive(false);
        TitleScreenBackground = TitleScreen.GetComponent<Image>();
        //paused = true;
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        PauseGame();
        UpdateTimer();
	}

    public void CheckScore(int scoreToCheck, Transform winningPlayer)
    {
        if (scoreToCheck > scoreMax)
        {
            //winningPlayer is winner
        }
    }

    public void AddPlayer(Transform playerToAdd)
    {
        players.Add(playerToAdd);
        discs.Add(playerToAdd.GetChild(1));
    } 

    public void PauseGame()
    {
        if (paused == false)
        {
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
            paused = false;
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

    IEnumerator FadeTitleScreen(bool fadeIn)
    {
        if (fadeIn == true)
        {
            if (TitleScreen.activeSelf == false)
            {
                TitleScreen.SetActive(true);
            }
            while (TitleScreenBackground.color.a > 0)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a -= Time.deltaTime;
                TitleScreenBackground.color = fadeColor;
                yield return null;
            }
            if (TitleScreenElements.activeSelf == false)
            {
                TitleScreenElements.SetActive(true);
            }
            Time.timeScale = 0f;
        }
        else
        {
            if (TitleScreenElements.activeSelf == true)
            {
                TitleScreenElements.SetActive(false);
            }
            while (TitleScreenBackground.color.a < 1)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a += Time.deltaTime;
                TitleScreenBackground.color = fadeColor;
                yield return null;
            }
            if (TitleScreen.activeSelf == true)
            {
                TitleScreen.SetActive(false);
            }
            Time.timeScale = 1f;
        }        
    }


    public void StartGame()
    {
        Time.timeScale = 1;
        paused = false;
        StartCoroutine(FadeTitleScreen(false));
    }

    public void EndGame()
    {
        StartCoroutine(FadeTitleScreen(true));
        ResetGame();
    }

    void ResetGame()
    {
        //Reset Player Position
        //player1.position = new Vector3(-10, 1, 0);
        //player1.rotation.eulerAngles.Set(0, -90, 0);
        //player2.position = new Vector3(10, 1, 0);
        //player2.rotation.eulerAngles.Set(0, 90, 0);

        //player1Score = 0;
        //player2Score = 0;
        //scoreDisplay1.text = 0.ToString();
        //scoreDisplay2.text = 0.ToString();
        //scoreSlider1.value = 0;
        //scoreSlider2.value = 0;
        //timerDisplay.text = 120.ToString();
        //countDown = 120;
        //timer = 0;

        //disc1.GetComponent<DiscScript>().SetValues(0, 500);
        //disc2.GetComponent<DiscScript>().SetValues(0, 500);
        //disc1.GetComponent<DiscScript>().CatchDisc(catchBox1);
        //disc2.GetComponent<DiscScript>().CatchDisc(catchBox2);
        //catchBox1.GetComponent<CatchScript>().ResetValues();
        //catchBox2.GetComponent<CatchScript>().ResetValues();
        //disc1.parent = player1;
        //disc2.parent = player2;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
