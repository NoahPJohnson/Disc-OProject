using System.Collections;

using UnityEngine;

public class PlayerMovementType1 : PlayerMovementInterface
{
    CharacterController controllerToManipulate;
    Vector3 moveVector;
    float moveSpeed = 16;

	// Use this for initialization
	void Start ()
    {
		
	}

    public void Move(bool up, bool left, bool down, bool right)
    {
        if (up == true)
        {
            moveVector.z = 1;
        }
        else if (down == true)
        {
            moveVector.z = -1;
        }
        else if (up == true && down == true)
        {
            moveVector.z = 0;
        }
        else
        {
            moveVector.z = 0;
        }
        if (left == true)
        {
            moveVector.x = -1;
        }
        else if (right == true)
        {
            moveVector.x = 1;
        }
        else if (left == true && right == true)
        {
            moveVector.x = 0;
        }
        else
        {
            moveVector.x = 0;
        }
        controllerToManipulate.Move(moveVector * moveSpeed * Time.deltaTime);  
    }

    public void Move(float h, float v)
    {
        moveVector.x = h;
        moveVector.z = v;
        controllerToManipulate.Move(moveVector * moveSpeed * Time.deltaTime);
    }

    public void IdentifyPlayer(CharacterController playerUsingInterface)
    {
        if (playerUsingInterface != null)
        {
            controllerToManipulate = playerUsingInterface;
        }
    }
}
