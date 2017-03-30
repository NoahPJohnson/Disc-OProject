using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    //[SerializeField] int playerCount = 2;
    [SerializeField] string horizontalAxisName;
    [SerializeField] string verticalAxisName;
    [SerializeField] string hTurmAxisName;
    [SerializeField] string vTurnAxisName;
    [SerializeField] string button1Name;
    [SerializeField] string pauseButtonName;
    [SerializeField] string button2Name;

    [SerializeField] CharacterController playerController;
    [SerializeField] Transform playerRotationTransform;
    [SerializeField] PlayerMovementInterface movementInterface;
    [SerializeField] PlayerRotationInterface rotationInterface;
    [SerializeField] PlayerCatchReleaseInterface catchInterface;
    [SerializeField] PlayerPauseInterface pauseInterface;

	// Use this for initialization
	void Start ()
    {
        SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        //movementInterface = moveType1;
        movementInterface.IdentifyPlayer(playerController);
        rotationInterface.IdentifyPlayer(playerRotationTransform);
        catchInterface.IdentifyCatchBox(playerRotationTransform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        movementInterface.Move(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));
        rotationInterface.Rotate(Input.GetAxis(hTurmAxisName), Input.GetAxis(vTurnAxisName));
        catchInterface.Catch(Input.GetButtonDown(button1Name));
        catchInterface.Throw(Input.GetButtonUp(button1Name));
        pauseInterface.Pause(Input.GetButtonDown(pauseButtonName));
	}

    void SetInterfaceType(PlayerMovementInterface moveType, PlayerRotationInterface rotationType, PlayerCatchReleaseInterface catchType, PlayerPauseInterface pauseType)
    {
        movementInterface = moveType;
        rotationInterface = rotationType;
        catchInterface = catchType;
        pauseInterface = pauseType;
    }
}
