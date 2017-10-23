using System.Collections;
using UnityEngine;

public interface PlayerMovementInterface
{
    //Movement function abstract
    void Move(bool up, bool left, bool down, bool right);
    void Move(float h, float v);
    //Function for attaching the interface to a specific player
    void IdentifyPlayer(CharacterController playerUsingInterface);
}
