using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2AIScript : MonoBehaviour
{
    [SerializeField] bool activeAI;
    [SerializeField] CharacterController player2Controller;
    [SerializeField] PlayerRotationInterface rotationInterface;
    [SerializeField] Vector3 targetDiscRelativePosition;
    [SerializeField] Vector3 targetDiscAnticipatePosition;
    [SerializeField] Vector3 originRelativePosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float moveSpeed = 16;
    [SerializeField] Transform player2CatchBox;
    CatchScript player2CatchScript;

    [SerializeField] bool anticipateMode = true;
    [SerializeField] int missCount = 0;
    [SerializeField] int missThreshold = 6;
    [SerializeField] bool catchAttempted = false;
    [SerializeField] bool discThrown = false;

    [SerializeField] Transform player1Reference;
    [SerializeField] Vector3 throwTargetVector;
    [SerializeField] bool throwTargetConfirmed;
    [SerializeField] float timeToThrow;
    float throwTimer = 0;

    [SerializeField] Transform disc1;
    [SerializeField] float oldDisc1Points = 0;
    [SerializeField] Transform oldDisc1Owner = null;
    [SerializeField] float disc1Priority = 0;

    [SerializeField] Transform disc2;
    [SerializeField] float oldDisc2Points = 0;
    [SerializeField] Transform oldDisc2Owner = null;
    [SerializeField] float disc2Priority = 0;

    [SerializeField] Transform targetDisc;
    [SerializeField] Transform targetDiscLowPriority;

    [SerializeField] Transform GoalLine;
    [SerializeField] Transform Teleporter1;
    [SerializeField] Transform Teleporter2;

    [SerializeField] float thinkTime = 1;
    float timer;


	// Use this for initialization
	void Start ()
    {
        player2CatchScript = player2CatchBox.GetComponent<CatchScript>();
        rotationInterface = new PlayerRotationType1();
        rotationInterface.IdentifyPlayer(player2CatchBox);
        player1Reference = GameObject.FindGameObjectWithTag("Player1").transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (activeAI == true)
        {
            if (missCount > missThreshold)
            {
                missCount = 0;
                anticipateMode = !anticipateMode;
                //Debug.Log("Switch: " + anticipateMode);
            }
            if (player2CatchScript.holdingDisc == true)
            {

                //If in possession of two discs, in the lead, and at least one disc has points, let that one go until it has 4, mark it as target until it is caught or intercepted

                if (Mathf.Abs(Mathf.Abs(player1Reference.position.x) - Mathf.Abs(GoalLine.position.x)) < Mathf.Abs(Mathf.Abs(player1Reference.position.x) - Mathf.Abs(Teleporter1.position.x)) && throwTargetConfirmed == false)
                {
                    //Player is closer to goal, throw toward goal but away from player
                    //throwTargetVector = GoalLine.position - player2CatchBox.position;
                    if (Mathf.Abs(player1Reference.position.z - 6) < Mathf.Abs(player1Reference.position.z + 6))
                    {
                        throwTargetVector = new Vector3(player1Reference.position.x - transform.position.x, 0, -6 - transform.position.z);
                    }
                    else
                    {
                        throwTargetVector = new Vector3(player1Reference.position.x - transform.position.x, 0, 6 - transform.position.z);
                    }
                    throwTimer = 0;
                    timeToThrow = Mathf.Abs(Mathf.Atan2(throwTargetVector.normalized.x, throwTargetVector.normalized.z) - Mathf.Atan2(player2CatchBox.forward.x, player2CatchBox.forward.z));
                    
                    throwTargetConfirmed = true;
                }
                else if (Mathf.Abs(Mathf.Abs(player1Reference.position.x) - Mathf.Abs(GoalLine.position.x)) >= Mathf.Abs(Mathf.Abs(player1Reference.position.x) - Mathf.Abs(Teleporter1.position.x)) && throwTargetConfirmed == false)
                {
                    //Player is closer to teleporter, throw toward teleporter but away from player
                    //throwTargetVector = Teleporter2.position - transform.GetChild(0).position;
                    if (Mathf.Abs(player1Reference.position.z - 6) < Mathf.Abs(player1Reference.position.z + 6))
                    {
                        throwTargetVector = new Vector3(2*Teleporter2.position.x + (player1Reference.position.x - transform.position.x), 0, -6 - transform.position.z);
                    }
                    else
                    {
                        throwTargetVector = new Vector3(2*Teleporter2.position.x + (player1Reference.position.x - transform.position.x), 0, 6 - transform.position.z);
                    }
                    throwTimer = 0;
                    timeToThrow = Mathf.Abs(Mathf.Atan2(throwTargetVector.normalized.x, throwTargetVector.normalized.z) - Mathf.Atan2(player2CatchBox.forward.x, player2CatchBox.forward.z));
                    
                    throwTargetConfirmed = true;
                }
                throwTimer += Time.deltaTime;
                //timer based on distance from target vector
                if (throwTimer < timeToThrow/8)
                {
                    rotationInterface.Rotate(throwTargetVector.normalized.x, throwTargetVector.normalized.z);
                }
                else
                {
                    throwTargetConfirmed = false;
                    player2CatchScript.AttemptThrow();
                    if (discThrown == false)
                    {
                        discThrown = true;
                        if (missCount > 0)
                        {
                            missCount -= 1;
                        }
                    }
                }
                targetDisc = null;
            }
            else
            {
                if (discThrown == false && catchAttempted == true)
                {
                    missCount += 4;
                    catchAttempted = false;
                }
                else if (discThrown == true && catchAttempted == true)
                {
                    discThrown = false;
                    catchAttempted = false;
                }

                timer += Time.deltaTime;
                if (timer > thinkTime)
                {
                    timer = 0;
                    TargetADisc();
                }

                originRelativePosition = new Vector3(GoalLine.position.x + 10, GoalLine.position.y, GoalLine.position.z) - transform.GetChild(0).position;
                if (targetDisc != null)
                {
                    targetDiscRelativePosition = targetDisc.position - transform.GetChild(0).position;
                    targetDiscAnticipatePosition = targetDiscRelativePosition + (targetDisc.forward * targetDisc.GetComponent<DiscScript>().GetSpeed());
                    
                    //transform.GetChild(0).LookAt(targetDisc);
                    
                    rotationInterface.Rotate(targetDiscRelativePosition.normalized.x, targetDiscRelativePosition.normalized.z);
                    //transform.GetChild(0).rotation = new Quaternion(0, transform.GetChild(0).rotation.y, 0, transform.GetChild(0).rotation.w);
                    if (targetDisc.position.x > GoalLine.position.x && targetDisc.position.x < Teleporter2.position.x)
                    {
                        if (targetDisc.GetComponent<DiscScript>().GetSpeed() >= moveSpeed)
                        {
                            if (anticipateMode == true)
                            {
                                movementVector = new Vector3(targetDiscAnticipatePosition.x, 0, targetDiscAnticipatePosition.z);
                            }
                            else
                            {
                                movementVector = new Vector3(targetDiscRelativePosition.x, 0, targetDiscRelativePosition.z);
                            }
                        }
                        else if (targetDisc.GetComponent<DiscScript>().GetSpeed() < moveSpeed || Mathf.Abs((targetDiscAnticipatePosition - transform.GetChild(0).position).magnitude) <= 24)
                        {
                            movementVector = new Vector3(targetDiscRelativePosition.x, 0, targetDiscRelativePosition.z);
                        }
                    }
                    else
                    {
                        movementVector = new Vector3(originRelativePosition.x, 0, targetDiscRelativePosition.z);
                    }
                    

                    player2Controller.Move(movementVector.normalized * moveSpeed * Time.deltaTime);
                    
                    
                    if (player2CatchScript.GetAbleToCatch() == true && Mathf.Abs(targetDiscRelativePosition.magnitude) < 5)
                    {
                        player2CatchScript.AttemptCatch();
                        if (catchAttempted == false)
                        {
                            catchAttempted = true;
                        }

                    }
                }
                else if (targetDisc == null && targetDiscLowPriority != null)
                {
                    timer += Time.deltaTime;
                    if (timer > thinkTime)
                    {
                        timer = 0;
                        targetDisc = targetDiscLowPriority;
                        targetDiscLowPriority = null;
                    }
                }
                else if (targetDisc == null && targetDiscLowPriority == null)
                {
                    movementVector = new Vector3(originRelativePosition.x, 0, originRelativePosition.z);
                    player2Controller.Move(movementVector.normalized * moveSpeed * Time.deltaTime);
                }
            }
        }
	}

    public void SetAIActive(bool value)
    {
        activeAI = value;
    }

    public void SetThinkTime(float thinkValue)
    {
        thinkTime = thinkValue;
    }

    void TargetADisc()
    {
        //if ((disc1.position.x <= GoalLine.position.x+1 && disc1.position.x >= GoalLine.position.x-1)|| disc1.position.x == Teleporter1.position.x || disc1.position.x == Teleporter2.position.x || disc2.position.x == GoalLine.position.x || disc2.position.x == Teleporter1.position.x || disc2.position.x == Teleporter2.position.x)
        if (disc1.GetComponent<DiscScript>().GetPointValue() != oldDisc1Points || disc2.GetComponent<DiscScript>().GetPointValue() != oldDisc2Points || disc1.GetComponent<DiscScript>().GetPlayerOwner() != oldDisc1Owner || disc2.GetComponent<DiscScript>().GetPlayerOwner() != oldDisc2Owner)
        {
            oldDisc1Points = disc1.GetComponent<DiscScript>().GetPointValue();
            oldDisc2Points = disc2.GetComponent<DiscScript>().GetPointValue();
            oldDisc1Owner = disc1.GetComponent<DiscScript>().GetPlayerOwner();
            oldDisc2Owner = disc2.GetComponent<DiscScript>().GetPlayerOwner();
            //Debug.Log("Target!");
            //PointValue
            if (disc1.GetComponent<DiscScript>().GetPointValue() > disc2.GetComponent<DiscScript>().GetPointValue())
            {
                disc1Priority += 3;
            }
            else if (disc1.GetComponent<DiscScript>().GetPointValue() < disc2.GetComponent<DiscScript>().GetPointValue())
            {
                disc2Priority += 3;
            }
            //Distance
            if ((disc1.position - transform.GetChild(0).position).magnitude < (disc2.position - transform.GetChild(0).position).magnitude)
            {
                disc1Priority += 2;
            }
            else if ((disc1.position - transform.GetChild(0).position).magnitude > (disc2.position - transform.GetChild(0).position).magnitude)
            {
                disc2Priority += 2;
            }
            //Owner
            if (disc1.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0) && disc2.GetComponent<DiscScript>().GetPlayerOwner() != transform.GetChild(0))
            {
                disc1Priority += 2;
            }
            else if (disc1.GetComponent<DiscScript>().GetPlayerOwner() != transform.GetChild(0) && disc2.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0))
            {
                disc2Priority += 2;
            }
            if (disc1.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0) && disc2.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0) && GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreDisplayScript>().GetPlayerScore(1) > GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreDisplayScript>().GetPlayerScore(0))
            {
                if (disc1.GetComponent<DiscScript>().GetPointValue() == disc1.GetComponent<DiscScript>().GetPointValueMax() && disc1.GetComponent<DiscScript>().GetPointValue() > disc2.GetComponent<DiscScript>().GetPointValue())
                {
                    disc1Priority += 4;
                    //Debug.Log("MAX PRIORITY ACHIEVED DISC 1");
                }
                else if (disc2.GetComponent<DiscScript>().GetPointValue() == disc2.GetComponent<DiscScript>().GetPointValueMax() && disc2.GetComponent<DiscScript>().GetPointValue() > disc1.GetComponent<DiscScript>().GetPointValue())
                {
                    disc2Priority += 4;
                    //Debug.Log("MAX PRIORITY ACHIEVED DISC 2");
                }
                else if (disc1.GetComponent<DiscScript>().GetPointValue() > 0 || disc1.GetComponent<DiscScript>().GetPointValue() > disc2.GetComponent<DiscScript>().GetPointValue())
                {
                    disc1Priority -= 3;
                    //Debug.Log("Risk Taken on disc 1");
                }
                else if (disc2.GetComponent<DiscScript>().GetPointValue() > 0 || disc2.GetComponent<DiscScript>().GetPointValue() > disc1.GetComponent<DiscScript>().GetPointValue())
                {
                    disc2Priority -= 3;
                    //Debug.Log("Risk Taken on disc 2");
                }
            }
            if (disc1.GetComponent<DiscScript>().GetPointValue() == 0 && disc1.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0))
            {
                disc1Priority = 0;
            }
            else if (disc2.GetComponent<DiscScript>().GetPointValue() == 0 && disc2.GetComponent<DiscScript>().GetPlayerOwner() == transform.GetChild(0))
            {
                disc2Priority = 0;
            }


            if (disc1Priority > disc2Priority)
            {
                targetDisc = disc1;
                targetDiscLowPriority = disc2;
                disc1Priority = 0;
                disc2Priority = 0;
            }
            else if (disc1Priority < disc2Priority)
            {
                targetDisc = disc2;
                targetDiscLowPriority = disc1;
                disc1Priority = 0;
                disc2Priority = 0;
            }
            else
            {
                disc1Priority = 0;
                disc2Priority = 0;
            }
        }
    }
}
