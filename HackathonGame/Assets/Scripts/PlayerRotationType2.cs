using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationType2 : PlayerRotationInterface
{
    Transform playerRotationAxis;
    Quaternion targetRotation;
    Quaternion rotationX;
    Quaternion rotationY;
    Vector3 mouseVector;
    Ray ray;
    RaycastHit hit;
    float rotationSpeed = 650f;
    float rotationDirection = 0f;


    public void Rotate(float rightX, float rightY)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            mouseVector.Set(hit.point.x-playerRotationAxis.position.x, 0, hit.point.z-playerRotationAxis.position.z);
            targetRotation.SetLookRotation(mouseVector);
            playerRotationAxis.rotation = Quaternion.RotateTowards(playerRotationAxis.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
    }

    public void IdentifyPlayer(Transform playerUsingInterface)
    {
        playerRotationAxis = playerUsingInterface;
    }
}
