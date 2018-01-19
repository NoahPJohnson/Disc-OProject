using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayScript : MonoBehaviour
{
    [SerializeField] Text[] playerScoreCounters;
    [SerializeField] Slider[] playerScoreSliders;
    [SerializeField] int[] playerScoreValues;
    [SerializeField] List<Transform> players;
    [SerializeField] int playerIndex;
    [SerializeField] Transform debugPlayerToAdd;
    [SerializeField] Transform debugPlayerToAdd2;

    // Use this for initialization
    void Start ()
    {
        //playerScoreCounters = new Text[4];
        //playerScoreSliders = new Slider[4];
        players = new List<Transform>();
        AddPlayer(debugPlayerToAdd);
        AddPlayer(debugPlayerToAdd2);
        SetColor();
    }

    public void SetColor()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerScoreSliders[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().color = players[i].GetComponent<CatchScript>().GetPlayerColor();
        }
    }

    public Color DeclareWinner()
    {
        if (playerScoreValues[0] > playerScoreValues[1])
        {
            return playerScoreSliders[0].transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
        }
        else if (playerScoreValues[0] < playerScoreValues[1])
        {
            return playerScoreSliders[1].transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
        }
        else
        {
            return Color.white;
        }
    }

    public void AddPlayer(Transform playerToAdd)
    {
        players.Add(playerToAdd);
    }

    public void UpdateDisplay(int scoreValue, Transform scoringPlayer)
    {
        playerIndex = players.IndexOf(scoringPlayer);
        GetComponent<MusicManagerScript>().UpdateTheme(playerScoreValues);
        if (playerIndex > -1)
        {
            playerScoreCounters[playerIndex].text = scoreValue.ToString();
            playerScoreSliders[playerIndex].value = scoreValue;
            playerScoreValues[playerIndex] = scoreValue;
        }
    }

    public int GetPlayerScore(int index)
    {
        return playerScoreValues[index];
    }
}
