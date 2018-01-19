using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField] int countDownStart = 120;
    [SerializeField] int countDown = 100;
    [SerializeField] float timeInterval = 1;
    float timer = 0;

    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject OptionsScreen;
    [SerializeField] GameObject player1Stats;
    [SerializeField] GameObject player2Stats;
    [SerializeField] Image TitleScreenBackground;
    [SerializeField] GameObject TitleScreenElements;
    [SerializeField] GameObject TransitionScreen;
    [SerializeField] GameObject PauseScreen;

    [SerializeField] GameObject mapSelector;
    [SerializeField] GameObject[] mapArray;
    [SerializeField] GameObject countDownSelector;
    [SerializeField] int[] countDownArray;
    [SerializeField] GameObject scoreMaxSelector;
    [SerializeField] int[] scoreMaxArray;
    [SerializeField] GameObject discScoreMaxSelector;
    [SerializeField] int[] discScoreMaxArray;
    [SerializeField] GameObject player1ColorSelector;
    [SerializeField] GameObject player2ColorSelector;
    [SerializeField] Color[] player1ColorArray;
    [SerializeField] Color[] player2ColorArray;

    [SerializeField] GameObject InputManagerP1;
    [SerializeField] GameObject InputManagerP2;
    Player2AIScript AIScript;

    [SerializeField] Text timerDisplay;
    Coroutine countdownCoroutine;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start ()
    {
        players = new ArrayList();
        AddPlayer(debugPlayerToAdd1);
        AddPlayer(debugPlayerToAdd2);
        PauseScreen.SetActive(false);
        mapSelector.GetComponent<SelectIndexScript>().SetArraySize(mapArray.Length);
        countDownSelector.GetComponent<SelectIndexScript>().SetArraySize(countDownArray.Length);
        scoreMaxSelector.GetComponent<SelectIndexScript>().SetArraySize(scoreMaxArray.Length);
        discScoreMaxSelector.GetComponent<SelectIndexScript>().SetArraySize(discScoreMaxArray.Length);
        player1ColorSelector.GetComponent<SelectIndexScript>().SetArraySize(player1ColorArray.Length);
        player1ColorSelector.GetComponent<Image>().color = player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        player2ColorSelector.GetComponent<SelectIndexScript>().SetArraySize(player2ColorArray.Length);
        player2ColorSelector.GetComponent<Image>().color = player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        InputManagerP1.GetComponent<InputManagerScript>().SetInputManagerActive(false);
        InputManagerP2.GetComponent<InputManagerScript>().SetInputManagerActive(false);
        debugPlayerToAdd1.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        debugPlayerToAdd2.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        player1Stats.SetActive(false);
        player2Stats.SetActive(false);
        AIScript = debugPlayerToAdd2.parent.GetComponent<Player2AIScript>();
        //Time.timeScale = 0;
	}

    public void CheckScore(int scoreToCheck, Transform winningPlayer)
    {
        if (scoreToCheck >= scoreMax)
        {
            EndGame();
            //EndGame
            //winningPlayer is winner
        }
    }

    public void EnableBotMode(bool botEnabled)
    {
        if (botEnabled == true)
        {
            InputManagerP2.GetComponent<InputManagerScript>().enabled = false;
            debugPlayerToAdd2.parent.GetComponent<Player2AIScript>().enabled = true;
        }
        else
        {
            InputManagerP2.GetComponent<InputManagerScript>().enabled = true;
            debugPlayerToAdd2.parent.GetComponent<Player2AIScript>().enabled = false;
        }
    }

    public void AddPlayer(Transform playerToAdd)
    {
        players.Add(playerToAdd);
        //discs.Add(playerToAdd.GetChild(1));
    } 

    public void PauseGame()
    {
        if (paused == false)
        {
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(PauseScreen.transform.GetChild(1).gameObject);
            GetComponent<MusicManagerScript>().SetPaused(true);
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
            GetComponent<MusicManagerScript>().SetPaused(false);
            paused = false;
        }
    }

    IEnumerator StartAnimation()
    {
        TransitionScreen.transform.GetChild(0).GetComponent<Text>().text = "READY";
        TransitionScreen.SetActive(true);
        debugPlayerToAdd1.parent.position = new Vector3(debugPlayerToAdd1.parent.position.x, debugPlayerToAdd1.parent.position.y, debugPlayerToAdd1.parent.position.z + 5);
        float tempIForce1 = debugPlayerToAdd1.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().GetInitialForce();
        debugPlayerToAdd1.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().SetValues(0, 16);
        debugPlayerToAdd1.GetComponent<CatchScript>().AttemptThrow();

        debugPlayerToAdd2.parent.position = new Vector3(debugPlayerToAdd2.parent.position.x, debugPlayerToAdd2.parent.position.y, debugPlayerToAdd2.parent.position.z - 5);
        float tempIForce2 = debugPlayerToAdd2.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().GetInitialForce();
        debugPlayerToAdd2.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().SetValues(0, 16);
        debugPlayerToAdd2.GetComponent<CatchScript>().AttemptThrow();

        while (debugPlayerToAdd1.parent.position.z > -5 && debugPlayerToAdd2.parent.position.z < 5)
        {
            debugPlayerToAdd1.parent.Translate(Vector3.right * 7f * Time.deltaTime);
            debugPlayerToAdd2.parent.Translate(Vector3.right * 7f * Time.deltaTime);
            yield return null;
        }
        while (debugPlayerToAdd1.GetComponent<CatchScript>().holdingDisc == false && debugPlayerToAdd2.GetComponent<CatchScript>().holdingDisc == false)
        {
            if ((GameObject.FindGameObjectsWithTag("Disc")[0].transform.position - debugPlayerToAdd1.position).magnitude < 10 || (GameObject.FindGameObjectsWithTag("Disc")[1].transform.position - debugPlayerToAdd2.position).magnitude < 10)
            {
                TransitionScreen.transform.GetChild(0).GetComponent<Text>().text = "SET";
            }
            if ((GameObject.FindGameObjectsWithTag("Disc")[0].transform.position - debugPlayerToAdd1.position).magnitude < 4 || (GameObject.FindGameObjectsWithTag("Disc")[1].transform.position - debugPlayerToAdd2.position).magnitude < 4)
            {
                debugPlayerToAdd1.GetComponent<CatchScript>().AttemptCatch();
                debugPlayerToAdd2.GetComponent<CatchScript>().AttemptCatch();
            }
            yield return null;
        }
        TransitionScreen.transform.GetChild(0).GetComponent<Text>().text = "SET";
        while (debugPlayerToAdd1.parent.position.z < 0 && debugPlayerToAdd2.parent.position.z > 0)
        {
            debugPlayerToAdd1.parent.Translate(Vector3.left * 5 * Time.deltaTime);
            debugPlayerToAdd2.parent.Translate(Vector3.left * 5 * Time.deltaTime);
            if (debugPlayerToAdd1.parent.position.z > -2 && debugPlayerToAdd2.parent.position.z < 2)
            {
                TransitionScreen.transform.GetChild(0).GetComponent<Text>().text = "GO";
            }
            yield return null;
        }
        debugPlayerToAdd1.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().SetValues(0, tempIForce1);
        debugPlayerToAdd2.GetComponent<CatchScript>().transform.GetChild(1).GetComponent<DiscScript>().SetValues(0, tempIForce2);
        TransitionScreen.SetActive(false);
        yield return null;
    }

    IEnumerator CountDownTimer()
    {
        while (countDown > 0)
        {
            timer += Time.deltaTime;
            if (timer > timeInterval && countDown > 0)
            {
                timer = 0;
                countDown--;
                timerDisplay.text = countDown.ToString();
            }
            yield return null;
        }
        EndGame();
        //EndGame
    }

    IEnumerator FadeOptionScreen(bool fadeIn)
    {
        if (fadeIn == true)
        {
            EventSystem.current.SetSelectedGameObject(OptionsScreen.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
            if (OptionsScreen.activeSelf == false)
            {
                OptionsScreen.SetActive(true);
            }
            while (OptionsScreen.GetComponent<Image>().color.a < 1)
            {
                Color fadeColor = OptionsScreen.GetComponent<Image>().color;
                fadeColor.a += Time.deltaTime;
                OptionsScreen.GetComponent<Image>().color = fadeColor;
                if (OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y > 0)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(Vector3.down * (OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y) * 10 * Time.deltaTime);
                }
                if (OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y > 0)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(Vector3.down * (OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y) * 12 * Time.deltaTime);
                }
                if (OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition.y > 0)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(Vector3.down * (OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition.y) * 17 * Time.deltaTime);
                }
                yield return null;
            }
            TitleScreen.SetActive(false);
        }
        else
        {
            TitleScreen.SetActive(true);
            while (OptionsScreen.GetComponent<Image>().color.a > 0)
            {
                Color fadeColor = OptionsScreen.GetComponent<Image>().color;
                fadeColor.a -= Time.deltaTime;
                OptionsScreen.GetComponent<Image>().color = fadeColor;
                if (OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y < 360)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(Vector3.up * (360-OptionsScreen.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y) * 13 * Time.deltaTime);
                }
                if (OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y < 360)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(Vector3.up * (360-OptionsScreen.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition.y) * 9 * Time.deltaTime);
                }
                if (OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition.y < 360)
                {
                    OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(Vector3.up * (360-OptionsScreen.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition.y) * 15 * Time.deltaTime);
                }
                yield return null;
            }
            if (OptionsScreen.activeSelf == true && OptionsScreen.GetComponent<Image>().color.a <= 1)
            {
                OptionsScreen.SetActive(false);
            }
            //Time.timeScale = 1f;
        }
        yield return null;
    }

    IEnumerator FadeTitleScreen(bool fadeIn)
    {
        if (fadeIn == true)
        {
            //GetComponent<MusicManagerScript>().SetInGame(false);
            if (TitleScreen.activeSelf == false)
            {
                TitleScreen.SetActive(true);
            }
            while (TitleScreenBackground.color.a < 1)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a += Time.deltaTime;
                TitleScreenBackground.color = fadeColor;
                yield return null;
            }
            if (TitleScreenElements.activeSelf == false && TitleScreenBackground.color.a >= 1)
            {
                TitleScreenElements.SetActive(true);
            }
            //Time.timeScale = 0f;
        }
        else
        {
            GetComponent<MusicManagerScript>().SetInGame(true);
            if (TitleScreenElements.activeSelf == true)
            {
                TitleScreenElements.SetActive(false);
            }
            StartCoroutine(StartGameCountDown());
            while (TitleScreenBackground.color.a > 0)
            {
                Color fadeColor = TitleScreenBackground.color;
                fadeColor.a -= Time.deltaTime;
                TitleScreenBackground.color = fadeColor;
                yield return null;
            }
            if (TitleScreen.activeSelf == true && TitleScreenBackground.color.a <= 1)
            {
                TitleScreen.SetActive(false);
                
            }
        }        
    }

    IEnumerator StartGameCountDown()
    {
        //Turn Off Input Manager
        StartCoroutine(StartAnimation());
        yield return new WaitForSeconds(4.5f);
        countdownCoroutine = StartCoroutine(CountDownTimer());
        InputManagerP1.GetComponent<InputManagerScript>().SetInputManagerActive(true);
        InputManagerP2.GetComponent<InputManagerScript>().SetInputManagerActive(true);
        AIScript.SetAIActive(true);
    }

    public void StartGame()
    {
        countDown = countDownStart;
        timerDisplay.text = countDown.ToString();
        Time.timeScale = 1;
        paused = false;
        GetComponent<MusicManagerScript>().ResetMusicValues();
        GetComponent<ScoreDisplayScript>().SetColor();
        StartCoroutine(FadeTitleScreen(false));
    }

    public void OpenCloseOptions(bool open)
    {
        StartCoroutine(FadeOptionScreen(open));
    }

    public void EndGame()
    {
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
        GetComponent<MusicManagerScript>().SetPaused(false);
        paused = false;
        StopCoroutine(countdownCoroutine);
        Color winnerColor = GetComponent<ScoreDisplayScript>().DeclareWinner();
        winnerColor.a = 0;
        TitleScreen.GetComponent<Image>().color = winnerColor;
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

        InputManagerP1.GetComponent<InputManagerScript>().SetInputManagerActive(false);
        InputManagerP2.GetComponent<InputManagerScript>().SetInputManagerActive(false);
        AIScript.SetAIActive(false);

        GameObject[] catchBoxes = GameObject.FindGameObjectsWithTag("CatchBox");
        GameObject[] discs = GameObject.FindGameObjectsWithTag("Disc");
        for (int i = 0; i < discs.Length; i++)
        {
            catchBoxes[i].GetComponent<CatchScript>().SetThrowSignalOff();
            discs[i].GetComponent<DiscScript>().ResetDiscValues();
        }
        for (int i = 0; i < discs.Length; i++)
        {
            discs[i].GetComponent<DiscScript>().ResetDiscValues2(catchBoxes[i].transform);
            catchBoxes[i].GetComponent<CatchScript>().ResetValues();
        }
        
    }

    public void ChangeMap()
    {
        for (int i = 0; i < mapArray.Length; i ++)
        {
            if (mapArray[i] != null)
            {
                mapArray[i].SetActive(false);
            }
        }
        if (mapArray[mapSelector.GetComponent<SelectIndexScript>().GetIndex()] != null)
        {
            mapArray[mapSelector.GetComponent<SelectIndexScript>().GetIndex()].SetActive(true);
        }
    }

    public void ChangeCountDown()
    {
        countDown = countDownArray[countDownSelector.GetComponent<SelectIndexScript>().GetIndex()];
    }

    public void ChangeScoreMax()
    {
        scoreMax = scoreMaxArray[scoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()];
    }

    public void ChangeDiscScoreMax()
    {
        debugPlayerToAdd1.GetChild(1).GetComponent<DiscScript>().SetPointMax(discScoreMaxArray[discScoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        debugPlayerToAdd2.GetChild(1).GetComponent<DiscScript>().SetPointMax(discScoreMaxArray[discScoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()]);
    }

    public void ChangePlayer1Color(bool right)
    {
        if (player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex() == player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex())
        {
            player1ColorSelector.GetComponent<SelectIndexScript>().ChangeIndex(right);
        }
        debugPlayerToAdd1.GetComponent<CatchScript>().SetPlayerColor(player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        player1ColorSelector.GetComponent<Image>().color = player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        debugPlayerToAdd1.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
    }

    public void ChangePlayer2Color(bool right)
    {
        if (player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex() == player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex())
        {
            player2ColorSelector.GetComponent<SelectIndexScript>().ChangeIndex(right);
        }
        debugPlayerToAdd2.GetComponent<CatchScript>().SetPlayerColor(player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        player2ColorSelector.GetComponent<Image>().color = player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        debugPlayerToAdd2.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
    }

    public void SetDefaultRules()
    {
        discScoreMaxSelector.GetComponent<SelectIndexScript>().SetIndex(1);
        debugPlayerToAdd1.GetChild(1).GetComponent<DiscScript>().SetPointMax(discScoreMaxArray[discScoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        debugPlayerToAdd2.GetChild(1).GetComponent<DiscScript>().SetPointMax(discScoreMaxArray[discScoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()]);

        scoreMaxSelector.GetComponent<SelectIndexScript>().SetIndex(1);
        scoreMax = scoreMaxArray[scoreMaxSelector.GetComponent<SelectIndexScript>().GetIndex()];

        countDownSelector.GetComponent<SelectIndexScript>().SetIndex(2);
        countDown = countDownArray[countDownSelector.GetComponent<SelectIndexScript>().GetIndex()];

        mapSelector.GetComponent<SelectIndexScript>().SetIndex(0);
        for (int i = 0; i < mapArray.Length; i++)
        {
            if (mapArray[i] != null)
            {
                mapArray[i].SetActive(false);
            }
        }
        if (mapArray[mapSelector.GetComponent<SelectIndexScript>().GetIndex()] != null)
        {
            mapArray[mapSelector.GetComponent<SelectIndexScript>().GetIndex()].SetActive(true);
        }
    }

    public void SetDefaultControlsOrStats()
    {
        player1ColorSelector.GetComponent<SelectIndexScript>().SetIndex(0);
        debugPlayerToAdd1.GetComponent<CatchScript>().SetPlayerColor(player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        player1ColorSelector.GetComponent<Image>().color = player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        debugPlayerToAdd1.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player1ColorArray[player1ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);

        player2ColorSelector.GetComponent<SelectIndexScript>().SetIndex(1);
        debugPlayerToAdd2.GetComponent<CatchScript>().SetPlayerColor(player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
        player2ColorSelector.GetComponent<Image>().color = player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()];
        debugPlayerToAdd2.parent.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), player2ColorArray[player2ColorSelector.GetComponent<SelectIndexScript>().GetIndex()]);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
