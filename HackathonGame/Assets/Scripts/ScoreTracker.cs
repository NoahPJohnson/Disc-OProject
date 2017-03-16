using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScoreTracker
{

    Transform assignedPlayer;
    GameObject gameStateManager;
    GameStateScript gameStateManagerScript;
    int playerScore;

    public ScoreTracker(Transform player)
    {
        assignedPlayer = player;
        playerScore = 0;
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager");
        gameStateManagerScript = gameStateManager.GetComponent<GameStateScript>();
    }

    public void UpdateScore(int increment)
    {
        playerScore += increment;
        gameStateManagerScript.CheckScore(playerScore, assignedPlayer);
    }
}
