using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float h;
    [SerializeField] float v;
    [SerializeField] bool player1;

    Vector3 moveVector = new Vector3();
    //RequireComponent(typeof)

    // Use this for initialization
    void Start ()
    {
        playerController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MovePlayer();
        if (transform.position.y != 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
	}

    void MovePlayer()
    {
        if (player1 == true)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        else
        {
            h = Input.GetAxis("Horizontal2");
            v = Input.GetAxis("Vertical2");
        }
        moveVector.z = v;
        moveVector.x = h;
        playerController.Move(moveVector * movementSpeed * Time.deltaTime);
        //transform.Translate(moveVector * movementSpeed * Time.deltaTime);
    }
}
