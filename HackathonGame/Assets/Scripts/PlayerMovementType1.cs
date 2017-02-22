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

    public IEnumerator Move(float h, float v)
    {
        while (h != 0 && v != 0)
        {
            moveVector.z = v;
            moveVector.x = h;
            controllerToManipulate.Move(moveVector * moveSpeed * Time.deltaTime);
        }
        yield return null;
    }

    public void IdentifyPlayer(CharacterController playerUsingInterface)
    {
        if (playerUsingInterface != null)
        {
            controllerToManipulate = playerUsingInterface;
        }
    }
}
