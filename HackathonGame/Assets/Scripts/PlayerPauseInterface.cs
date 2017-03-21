using System.Collections;
using UnityEngine;

public interface PlayerPauseInterface
{
    void Pause(bool buttonDown);
    void IdentifyGameStateManager(Transform stateManager);
}
