using System.Collections;
using UnityEngine;

public interface PlayerRotationInterface
{
    void Rotate(float rightX, float rightY);
    void IdentifyPlayer(Transform playerUsingInterface);
}
