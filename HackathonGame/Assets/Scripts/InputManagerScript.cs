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
    [SerializeField] PlayerCatchReleaseInterface catchInterface;

	// Use this for initialization
	void Start ()
    {
        SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1());
        //movementInterface = moveType1;
        movementInterface.IdentifyPlayer(playerController);
        rotationInterface.IdentifyPlayer(playerRotationTransform);
        catchInterface.IdentifyCatchBox(playerRotationTransform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        movementInterface.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rotationInterface.Rotate(Input.GetAxis("RightStick X"), Input.GetAxis("RightStick Y"));
        catchInterface.Catch(Input.GetButtonDown("Fire1Player1"));
        catchInterface.Throw(Input.GetButtonUp("Fire1Player1"));
	}

    void SetInterfaceType(PlayerMovementInterface moveType, PlayerRotationInterface rotationType, PlayerCatchReleaseInterface catchType)
    {
        movementInterface = moveType;
        rotationInterface = rotationType;
        catchInterface = catchType;
    }
}
