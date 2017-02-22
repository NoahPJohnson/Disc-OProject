using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    [SerializeField] int playerCount = 2;
    [SerializeField] List<CharacterController> players;
    List<PlayerMovementInterface> movementInterfaceList;
    PlayerMovementType1 moveType1;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < playerCount; i ++)
        {
            movementInterfaceList.Add(moveType1);
            movementInterfaceList[i].IdentifyPlayer(players[i]);
            //Set other values like speed?
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void InitilizeInterfaceList()
    {
        movementInterfaceList = new List<PlayerMovementInterface>();
    }
}
