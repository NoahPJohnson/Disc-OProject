using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    //[SerializeField] int playerCount = 2;
    [SerializeField] string horizontalAxisName;
    [SerializeField] CharacterController playerController;
    [SerializeField] Transform playerRotationTransform;
    [SerializeField] PlayerMovementInterface movementInterface;
    [SerializeField] PlayerRotationInterface rotationInterface;

	// Use this for initialization
	void Start ()
    {
        SetMovementType(new PlayerMovementType1(), new PlayerRotationType1());
        //movementInterface = moveType1;
        movementInterface.IdentifyPlayer(playerController);
        rotationInterface.IdentifyPlayer(playerRotationTransform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        movementInterface.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationInterface.Rotate(Input.GetAxis("RightStick X"), Input.GetAxis("RightStick Y"));
	}

    void SetMovementType(PlayerMovementInterface moveType, PlayerRotationInterface rotationType)
    {
        movementInterface = moveType;
        rotationInterface = rotationType;
    }
}
