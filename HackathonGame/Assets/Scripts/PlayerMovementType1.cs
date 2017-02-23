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

    public void Move(float h, float v)
    {
        //if (h != 0 && v != 0)
        //{
            moveVector.z = v;
            moveVector.x = h;
            controllerToManipulate.Move(moveVector * moveSpeed * Time.deltaTime);
        //}
        
    }

    public void IdentifyPlayer(CharacterController playerUsingInterface)
    {
        if (playerUsingInterface != null)
        {
            controllerToManipulate = playerUsingInterface;
        }
    }
}
