using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    //[SerializeField] int playerCount = 2;
    [SerializeField] string horizontalAxisName;
    [SerializeField] CharacterController playerController;
    [SerializeField] PlayerMovementInterface movementInterface;

	// Use this for initialization
	void Start ()
    {
        SetMovementType(new PlayerMovementType1());
        //movementInterface = moveType1;
        movementInterface.IdentifyPlayer(playerController);
	}
	
	// Update is called once per frame
	void Update ()
    {
        movementInterface.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

    void SetMovementType(PlayerMovementInterface moveType)
    {
        movementInterface = moveType;
    }
}
