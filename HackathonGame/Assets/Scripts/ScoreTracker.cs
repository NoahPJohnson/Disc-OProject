using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScoreTracker
{

    Transform assignedPlayer;

    GameObject gameStateManager;
    GameStateScript gameStateManagerScript;
    ScoreDisplayScript scoreDisplayScript;
    int playerScore;

    public ScoreTracker(Transform player)
    {
        assignedPlayer = player;
        playerScore = 0;
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager");
        gameStateManagerScript = gameStateManager.GetComponent<GameStateScript>();
        scoreDisplayScript = gameStateManager.GetComponent<ScoreDisplayScript>();
    }

    public void UpdateScore(int increment)
    {
        playerScore += increment;
        scoreDisplayScript.UpdateDisplay(playerScore, assignedPlayer);
        gameStateManagerScript.CheckScore(playerScore, assignedPlayer);
    }
}
