using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchScript : MonoBehaviour
{
    [SerializeField] Transform catchableDisc;
    [SerializeField] Transform caughtDisc;
    [SerializeField] GameObject visualization;
    DiscScript discScript;

    //[SerializeField] Text scoreDisplay;
    [SerializeField] GameObject gameStateManager;
    GameStateScript gameStateScript;

    [SerializeField] bool player1;
    [SerializeField] int score;

    [SerializeField] float recoveryTime;
    [SerializeField] bool ableToCatch;
    [SerializeField] bool holdingDisc;
    [SerializeField] bool discInBox;
    float time;

    public bool gamePaused;

	// Use this for initialization
	void Start ()
    {
        gameStateScript = gameStateManager.GetComponent<GameStateScript>();
        IdentifyDisc();
        discInBox = false;
        holdingDisc = true;
        ableToCatch = false;
        time = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gamePaused == false)
        {
            if (player1 == true)
            {
                if (Input.GetButtonDown("Fire1Player1"))
                {
                    AttemptCatch();
                }

                if (Input.GetButtonUp("Fire1Player1"))
                {
                    AttemptThrow();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1Player2"))
                {
                    AttemptCatch();
                }

                if (Input.GetButtonUp("Fire1Player2"))
                {
                    AttemptThrow();
                }
            }
        }
        if (ableToCatch == false && holdingDisc == false)
        {
            time += Time.deltaTime;
            if (time > recoveryTime)
            {
                ableToCatch = true;
                visualization.SetActive(true);
                time = 0;
            }
        }
	}

    void AttemptCatch()
    {
        if (ableToCatch == true)
        {
            ableToCatch = false;
            visualization.SetActive(false);
            //Debug.Log("Catch Attempted.");
            if (discInBox == true)
            {
                if (discScript != null)
                {
                    discScript.CatchDisc(transform);
                    discInBox = false;
                    holdingDisc = true;
                    IdentifyDisc();
                    visualization.SetActive(true);
                    gameStateScript.UpdateScore(player1, score);
                    //scoreDisplay.text = score.ToString();
                    //Debug.Log("Success, disk is: " + caughtDisc);
                }
            }
        }
    }

    void AttemptThrow()
    {
        if (holdingDisc == true && transform.childCount > 1)
        {
            discScript.ThrowDisc();
            caughtDisc = null;
            discScript = null;
            holdingDisc = false;
            visualization.SetActive(false);
        }
    }

    void IdentifyDisc()
    {
        caughtDisc = transform.GetChild(1);
        if (caughtDisc != null)
        {
            discScript = caughtDisc.GetComponent<DiscScript>();
        }
    }

    public bool GetPlayerType()
    {
        return player1;
    }

    public void IncrementScore(int increment)
    {
        score += increment;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetValues()
    {
        discInBox = false;
        holdingDisc = true;
        IdentifyDisc();
        visualization.SetActive(true);
        score = 0;
        time = 0;
    }

    void OnTriggerEnter(Collider disc)
    {
        if (disc.tag == "Disc")
        {
            if (ableToCatch == true)
            {
                catchableDisc = disc.transform;
                discScript = catchableDisc.GetComponent<DiscScript>();
                discInBox = true;
            }
        }
    }

    void OnTriggerExit(Collider disc)
    {
        if (disc.tag == "Disc")
        {
            if (ableToCatch == true && holdingDisc == false)
            {
                catchableDisc = null;
                discScript = null;
                discInBox = false;
            }
        }
    }
}
