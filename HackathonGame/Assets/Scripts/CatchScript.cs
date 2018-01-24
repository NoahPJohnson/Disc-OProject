using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchScript : MonoBehaviour
{
    [SerializeField] ScoreTracker scoreTracker;
    [SerializeField] Transform caughtDisc;
    [SerializeField] GameObject visualization;
    [SerializeField] GameObject gameStateManager;
    GameStateScript gameStateScript;

    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 startRotation;

    //[SerializeField] bool player1;
    [SerializeField] Color playerColor;
    [SerializeField] Color catchboxColor;
    [SerializeField] int score;

    [SerializeField] float recoveryTime;
    [SerializeField] bool ableToCatch;
    public bool holdingDisc;
    [SerializeField] bool throwSignal;
    [SerializeField] bool throwAttempted;
    float time;

    public bool gamePaused;

    Coroutine disableCatchCoroutine;

	// Use this for initialization
	void Start ()
    {
        gameStateScript = gameStateManager.GetComponent<GameStateScript>();
        scoreTracker = new ScoreTracker(transform);
        IdentifyDisc();
        throwSignal = false;
        holdingDisc = true;
        ableToCatch = false;
        time = 0;
        GetComponent<Collider>().enabled = false;
    }

    public void AttemptCatch()
    {
        if (ableToCatch == true)
        {
            ableToCatch = false;
            throwSignal = false;
            //visualization.SetActive(false);
            //Debug.Log("Catch Attempted.");
            StartCoroutine("ActivateCatchBox");
        }
    }

    public void AttemptThrow()
    {
        throwSignal = true;
        if (holdingDisc == true && transform.childCount > 1)
        {
            //discScript.ThrowDisc();
            throwAttempted = true;
            caughtDisc = null;
            //discScript = null;
            holdingDisc = false;
            ableToCatch = false;
            if (disableCatchCoroutine != null)
            {
                StopCoroutine(disableCatchCoroutine);
            }
            disableCatchCoroutine = StartCoroutine("DisableCatch");
            //visualization.SetActive(false);
        }     
    }

    public void IdentifyDisc()
    {
        holdingDisc = true;
        caughtDisc = transform.GetChild(1);
    }

    /*public bool GetPlayerType()
    {
        return player1;
    }*/

    public Color GetPlayerColor()
    {
        return playerColor;
    }

    public void SetPlayerColor(Color newColor)
    {
        playerColor = newColor;
    }

    public void SetPlayerStats(float sliderValue)
    {

    }

    public void Score(int increment)
    {
        scoreTracker.UpdateScore(increment);
        
    }


    public int GetScore()
    {
        return score;
    }

    public bool GetThrow()
    {
        return throwAttempted;
    }

    public bool GetAbleToCatch()
    {
        return ableToCatch;
    }

    public bool GetThrowAttempted()
    {
        return throwAttempted;
    }

    public void SetThrowFalse()
    {
        throwAttempted = false;
    }

    public void SetThrowSignalOff()
    {
        throwSignal = false; 
    }

    //Function called by game state manager to increment point values.
    public void ResetValues()
    {
        transform.localRotation = Quaternion.Euler(startRotation);
        transform.parent.position = startPosition;
        visualization.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), catchboxColor);
        //transform.parent.rotation = Quaternion.Euler(startRotation);
        scoreTracker.ResetScore();
        throwSignal = false;
        holdingDisc = true;
        IdentifyDisc();
        visualization.SetActive(true);
        score = 0;
        time = 0;
    }

    //Coroutine for handling the time interval that the catchBox is active for
    IEnumerator ActivateCatchBox()
    {
        GetComponent<Collider>().enabled = true;
        visualization.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.green);
        yield return new WaitForSeconds(.2f);
        GetComponent<Collider>().enabled = false;

        if (throwSignal == true)
        {
            AttemptThrow();
            throwSignal = false;
        }
        if (disableCatchCoroutine != null)
        {
            StopCoroutine(disableCatchCoroutine);
        }
        disableCatchCoroutine = StartCoroutine("DisableCatch");
    }
    
    //Coroutine for handling the time interval between catch attempts
    IEnumerator DisableCatch()
    {
        
        visualization.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.black);
        
        //visualization.SetActive(false);
        yield return new WaitForSeconds(recoveryTime);
        ableToCatch = true;
        visualization.GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), catchboxColor);
        //visualization.SetActive(true);
        //Debug.Log(ableToCatch);
    } 
}
