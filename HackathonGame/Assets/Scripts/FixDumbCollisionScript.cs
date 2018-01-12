using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDumbCollisionScript : MonoBehaviour
{
    float yPosition;

	// Use this for initialization
	void Start ()
    {
        yPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (transform.position.y != yPosition)
        {
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
        }
	}
}
