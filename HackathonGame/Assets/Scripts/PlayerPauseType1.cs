using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPauseType1 : PlayerPauseInterface
{
    GameStateScript gameStateScript;
    public void Pause(bool buttonDown)
    {
        if (buttonDown == true)
        {
            gameStateScript.PauseGame();
        }
    }

    public void IdentifyGameStateManager(Transform gameStateManager)
    {
        gameStateScript = gameStateManager.GetComponent<GameStateScript>();
    }
}
