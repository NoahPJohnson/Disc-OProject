using System.Collections;
using UnityEngine;

public interface PlayerMovementInterface
{
    //Movement function abstract
    IEnumerator Move(float h, float v);

    //Function for attaching the interface to a specific player
    void IdentifyPlayer(CharacterController playerUsingInterface);
}
