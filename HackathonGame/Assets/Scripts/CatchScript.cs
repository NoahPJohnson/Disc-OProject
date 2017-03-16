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

    [SerializeField] bool player1;
    [SerializeField] int score;

    [SerializeField] float recoveryTime;
    [SerializeField] bool ableToCatch;
    [SerializeField] bool holdingDisc;
    [SerializeField] bool throwSignal;
    [SerializeField] bool throwAttempted;
    float time;

    public bool gamePaused;

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
            visualization.SetActive(false);
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
            StartCoroutine("DisableCatch");
            visualization.SetActive(false);
        }
    }

    public void IdentifyDisc()
    {
        holdingDisc = true;
        caughtDisc = transform.GetChild(1);
    }

    public bool GetPlayerType()
    {
        return player1;
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

    public void SetThrowFalse()
    {
        throwAttempted = false;
    }

    //Function called by game state manager to increment point values.
    public void ResetValues()
    {
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
        yield return new WaitForSeconds(.2f);
        //Debug.Log("Catch is ACTIVE.");
        GetComponent<Collider>().enabled = false;
        StartCoroutine("DisableCatch");
        if (throwSignal == true)
        {
            AttemptThrow();
            throwSignal = false;
        }
    }
    
    //Coroutine for handling the time interval between catch attempts
    IEnumerator DisableCatch()
    {
        ableToCatch = false;
        visualization.SetActive(false);
        yield return new WaitForSeconds(.4f);
        ableToCatch = true;
        visualization.SetActive(true);
        //Debug.Log(ableToCatch);
    } 
}
