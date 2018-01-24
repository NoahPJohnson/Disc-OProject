using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    [SerializeField] int playerCount = 2;
    [SerializeField] int totalScore = 0;
    [SerializeField] int maxScore = 30;
    [SerializeField] int leadScoreIndex = 0;
    [SerializeField] float coolHighlightDelay;
    float timer;
    [SerializeField] bool coolHighlightDisabled;
    [SerializeField] bool tied;

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
        musicEvent = FMODUnity.RuntimeManager.CreateInstance(musicTrackName);
        musicEvent.getParameter("Pause menu", out pauseMenu);
        musicEvent.getParameter("Points", out points);
        musicEvent.getParameter("In/Out Of Game", out inGame);
        musicEvent.getParameter("Theme Vs", out themeDecision);
        ResetMusicValues();
        musicEvent.start();
    }
	
    public void UpdateTheme (int[] scoreArray)
    {
        maxScore = GetComponent<GameStateScript>().GetScoreMax();
        totalScore = 0;
        leadScoreIndex = 0;
        for (int i = 0; i < playerCount; i ++)
        {
            totalScore += scoreArray[i];
            if (scoreArray[i] > scoreArray[leadScoreIndex])
            {
                leadScoreIndex = i;
                tied = false;
            }
            else if (scoreArray[i] == scoreArray[leadScoreIndex])
            {
                tied = true;
            }
        }
        
        points.setValue((totalScore/playerCount)*(50f / maxScore));
        //Debug.Log((totalScore/playerCount) * (50f / maxScore));
        
        if (tied == true)
        {
            themeDecision.setValue(-1f);
        }
        else
        {
            themeDecision.setValue(leadScoreIndex);
        }
    }

    public void SetPaused(bool pauseValue)
    {
        if (pauseValue == true)
        {
            pauseMenu.setValue(1f);
        }
        else
        {
            pauseMenu.setValue(0f);
        }
    }

    public void SetInGame(bool inGameValue)
    {
        if (inGameValue == true)
        {
            inGame.setValue(1f);
        }
        else
        {
            inGame.setValue(0f);
        }
    }

    public void PlayCoolHighlight(Transform player)
    {
        if (player.tag == "Player1" && coolHighlightDisabled == false)
        {
            //Play Highlight for P1
            coolHighlightDisabled = true;
            StartCoroutine(CoolHighlightDelayTime());
        }
        else
        {
            //Highlight P2
            coolHighlightDisabled = true;
            StartCoroutine(CoolHighlightDelayTime());
        }
    }

    IEnumerator CoolHighlightDelayTime()
    {
        while (coolHighlightDisabled == true)
        {
            timer += Time.deltaTime;
            if (timer > coolHighlightDelay)
            {
                coolHighlightDisabled = false;
            }
            yield return null;
        }
    }


    public void ResetMusicValues()
    {
        pauseMenu.setValue(0f);
        points.setValue(0);
        themeDecision.setValue(1f);
        tied = true;
    }
}
