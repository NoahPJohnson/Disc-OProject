using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationScript : MonoBehaviour
{
    [SerializeField] Transform revMeasure;
    [SerializeField] float revFactor;
    [SerializeField] float revDecay = 50f;
    [SerializeField] float angleDifferenceDirection = 1f;

    [SerializeField] float adjustedRevAngle = 0f;
    [SerializeField] float adjustedRotationAngle = 0f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        adjustedRevAngle = revMeasure.rotation.eulerAngles.y;
        adjustedRotationAngle = transform.rotation.eulerAngles.y;
        if (adjustedRevAngle < 0)
        {
            adjustedRevAngle += 180f;
        }
        if (adjustedRotationAngle < 0)
        {
            adjustedRotationAngle += 180f;
        }
        if (Mathf.Abs(adjustedRotationAngle - adjustedRevAngle) > .2f)
        {
            angleDifferenceDirection = Mathf.Sign(adjustedRotationAngle - adjustedRevAngle);
            revMeasure.Rotate(0, revDecay * angleDifferenceDirection * Time.deltaTime, 0);
        }


        revFactor = adjustedRotationAngle - adjustedRevAngle;
        
    }
}
