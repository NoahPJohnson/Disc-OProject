using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayScript : MonoBehaviour
{
    [SerializeField] Text[] playerScoreCounters;
    [SerializeField] Slider[] playerScoreSliders;
    [SerializeField] int[] playerScoreValues;
    [SerializeField] ArrayList players;
    [SerializeField] int playerIndex;
    [SerializeField] Transform debugPlayerToAdd;
    [SerializeField] Transform debugPlayerToAdd2;

    // Use this for initialization
    void Start ()
    {
        //playerScoreCounters = new Text[4];
        //playerScoreSliders = new Slider[4];
        players = new ArrayList();
        AddPlayer(debugPlayerToAdd);
    }

    public void AddPlayer(Transform playerToAdd)
    {
        players.Add(playerToAdd);
    }

    public void UpdateDisplay(int scoreValue, Transform scoringPlayer)
    {
        playerIndex = players.IndexOf(scoringPlayer);
        if (playerIndex > -1)
        {
            playerScoreCounters[playerIndex].text = scoreValue.ToString();
            playerScoreSliders[playerIndex].value = scoreValue;
            playerScoreValues[playerIndex] = scoreValue;
        }
    }
}
