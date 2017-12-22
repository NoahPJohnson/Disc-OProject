using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationType1 : PlayerRotationInterface
{
    Transform playerRotationAxis;
    Quaternion targetRotation;
    float rotationSpeed = 650f;
    //float rotationDirection = 0f;
    

    public void Rotate(float rightX, float rightY)
    {
        if (Mathf.Abs(rightX) > .05 || Mathf.Abs(rightY) > .05)
        {
            targetRotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(rightX, rightY) * Mathf.Rad2Deg, 0));
            playerRotationAxis.rotation = Quaternion.RotateTowards(playerRotationAxis.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void IdentifyPlayer(Transform playerUsingInterface)
    {
        playerRotationAxis = playerUsingInterface;
    }
}
