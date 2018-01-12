using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationScript : MonoBehaviour
{
    //[SerializeField] Transform revMeasure;
    [SerializeField] float revFactor;
    [SerializeField] float revSpeed;
    [SerializeField] float revThreshhold = 1f;
    [SerializeField] float revMax;
    [SerializeField] float revMin;
    [SerializeField] float resetRevTime;
    [SerializeField] float setRevTime = .1f;
    float timer;
    float timer2;
    //float rotationDirection = 1f;

    //[SerializeField] float angleDifferenceDirection = 1f;

    [SerializeField] float adjustedRotationAngle = 0f;
    [SerializeField] float oldAdjustedRotationAngle = 0f;
    [SerializeField] float deltaAdjustedRotationAngle = 0f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer2 += Time.deltaTime; 
        adjustedRotationAngle = transform.rotation.eulerAngles.y;
        if (adjustedRotationAngle < 0)
        {
            adjustedRotationAngle += 180f;
        }
        
        if (timer2 > setRevTime)
        {
            timer2 = 0;
            deltaAdjustedRotationAngle = (adjustedRotationAngle - oldAdjustedRotationAngle);
            if (deltaAdjustedRotationAngle < -355)
            {
                deltaAdjustedRotationAngle = ((adjustedRotationAngle + 360) - oldAdjustedRotationAngle);
                //Debug.Log(deltaAdjustedRotationAngle);
            }
            else if (deltaAdjustedRotationAngle > 355)
            {
                deltaAdjustedRotationAngle = ((adjustedRotationAngle - 360) - oldAdjustedRotationAngle);
            }
            oldAdjustedRotationAngle = adjustedRotationAngle;
            if (deltaAdjustedRotationAngle > revThreshhold)
            {
                revFactor += revSpeed * Time.deltaTime;
                timer = 0;
            }
            else if (deltaAdjustedRotationAngle < -revThreshhold)
            {
                revFactor -= revSpeed * Time.deltaTime;
                timer = 0;
            }
            else
            {
                revFactor = 0;
                /*timer += Time.deltaTime;
                if (timer > resetRevTime)
                {
                    revFactor = 0;
                }*/
            }
        }
        
        
        revFactor = Mathf.Clamp(revFactor, revMin, revMax);
    }

    public float GetRevFactor()
    {
        return revFactor;
    }

    public float GetRevMax()
    {
        return revMax;
    }

    public void SetRevMax(float newMax)
    {
        revMax = newMax;
        revMin = -revMax;
    }

    public void SetRevFactor(float revValue)
    {
        revFactor += revValue;
        //Debug.Log("starting rev factor = " + revFactor);
    }
}
