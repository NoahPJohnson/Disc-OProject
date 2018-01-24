using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscScript : MonoBehaviour
{
    [SerializeField] Text discDisplay;

    [SerializeField] Transform playerOwner;
    [SerializeField] bool caught;
    [SerializeField] float speed;
    [SerializeField] float spinFactor;
    [SerializeField] float spinMax;
    [SerializeField] float spinDecay;
    [SerializeField] float speedMax;
    [SerializeField] float speedMin;
    [SerializeField] float speedDecay;
    [SerializeField] float bounceDecay;
    [SerializeField] float incrementForce;
    [SerializeField] float initialForce;
    [SerializeField] float initialForceMin;
    [SerializeField] int pointValue;
    [SerializeField] int pointMax;

    float defaultSpinMax = 60;
    float defaultSpinDecay = .8f;
    float defaultSpeedMax = 30;
    float defaultSpeedMin = 5;
    float defaultInitialForce = 5;
    float defaultInitialForceMin = 5;

    [SerializeField] Vector3 flyVector;
    [SerializeField] Vector3 spinVector;
    [SerializeField] Vector3 lockVector;

    Rigidbody discRigidbody;
    Collider discCollider;

    [FMODUnity.EventRef]
    [SerializeField] string discEffectName = "event:/Cues/Disk Sounds";
    FMOD.Studio.EventInstance discEvent;
    FMOD.Studio.ParameterInstance discEventValue;
    //0 = nothing;
    //1 = goal pass
    //2 = interception
    //3 = player1contact
    //4 = player2contact
    //5 = portal
    //6 = stun
    //7 = wall hit
    //FMOD.Studio.ParameterInstance contactEventValue;

    [FMODUnity.EventRef]
    [SerializeField] string scoreEffectName = "event:/Cues/Score Sounds";
    FMOD.Studio.EventInstance scoreEvent;
    FMOD.Studio.ParameterInstance scoreEventValue;

    // Use this for initialization
    void Start ()
    {
        discEvent = FMODUnity.RuntimeManager.CreateInstance(discEffectName);
        discEvent.getParameter("Parameters", out discEventValue);

        scoreEvent = FMODUnity.RuntimeManager.CreateInstance(scoreEffectName);
        scoreEvent.getParameter("Point Value", out scoreEventValue);

        discRigidbody = GetComponent<Rigidbody>();
        discCollider = GetComponent<CapsuleCollider>();
        caught = true;
        pointValue = 0;
        playerOwner = transform.parent;
        playerOwner.GetComponent<PlayerRotationScript>().SetRevMax(spinMax);
        GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), playerOwner.GetComponent<CatchScript>().GetPlayerColor());
        lockVector = new Vector3(1, 0, 1);

        Physics.IgnoreLayerCollision(12, 8, true);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (caught == true)
        {
            CaughtState();
        }
        else
        {
            FlyingState();
        }
	}

    public void ThrowDisc()
    {
        //flyVector = transform.parent.rotation.eulerAngles.normalized;
        spinFactor = playerOwner.GetComponent<PlayerRotationScript>().GetRevFactor();
        spinVector = new Vector3(0, spinFactor, 0);
        transform.parent = null;
        if (initialForce < initialForceMin)
        {
            initialForce = initialForceMin;
        }
        speed = initialForce;
        caught = false;
        discCollider.enabled = true;
        discRigidbody.constraints = RigidbodyConstraints.None;
        discRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        flyVector = Vector3.forward;
        //flyVector = transform.TransformDirection(flyVector);
        //discRigidbody.AddForce(transform.forward * speed/*, ForceMode.Impulse*/);
        //Debug.Log("Speed = " + speed);
    }

    public void CatchDisc(Transform catchBox)
    {
        //Debug.Log("Disc is caught!!");
        if (catchBox.childCount < 2)
        {
            caught = true;
            discCollider.enabled = false;
            transform.parent = catchBox;
            catchBox.GetComponent<CatchScript>().IdentifyDisc();
            if (playerOwner == transform.parent)
            {
                if (pointValue > 0)
                {
                    transform.parent.GetComponent<CatchScript>().Score(pointValue);
                }
                scoreEventValue.setValue(pointValue);
                scoreEvent.start();
            }
            else
            {
                discEventValue.setValue(2f);
                discEvent.start();
            }
            pointValue = 0;
            discDisplay.text = pointValue.ToString();
            transform.localPosition = new Vector3(0, 0, 1);
            transform.forward = transform.parent.forward;
            initialForce = speed;
            discRigidbody.velocity = Vector3.zero;
            discRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
            playerOwner = transform.parent;
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), playerOwner.GetComponent<CatchScript>().GetPlayerColor());
            /*if (playerOwner.GetComponent<CatchScript>().GetPlayerType() == true)
            {
                GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.red);
            }
            else
            {
                GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.blue);
            }*/
        }
    }

    /*public void DebugCatchDisc(Transform catchBox)
    {
        //Debug.Log("Disc is caught!!");
        if (catchBox.childCount < 2)
        {
            caught = true;
            discCollider.enabled = false;
            transform.parent = catchBox;
            catchBox.GetComponent<CatchScript>().IdentifyDisc();
            if (playerOwner == transform.parent)
            {
                if (pointValue > 0)
                {
                    transform.parent.GetComponent<CatchScript>().Score(pointValue);
                }
                scoreEventValue.setValue(pointValue);
                scoreEvent.start();
            }
            else
            {
                discEventValue.setValue(2f);
                discEvent.start();
            }
            pointValue = 0;
            discDisplay.text = pointValue.ToString();
            transform.localPosition = new Vector3(0, 0, 1);
            Debug.Log(transform.name + " Parent: " + transform.parent);
            transform.forward = transform.parent.forward;
            initialForce = speed;
            //transform.parent.GetComponent<PlayerRotationScript>().SetRevFactor(spinVector.y);
            discRigidbody.velocity = Vector3.zero;
            discRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
            playerOwner = transform.parent;
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), playerOwner.GetComponent<CatchScript>().GetPlayerColor());
            //transform.forward = transform.parent.forward;
        }
    }*/

    void FlyingState()
    {
        /*if (discRigidbody.velocity.magnitude < speedMin)
        {
            discRigidbody.velocity = transform.forward * speedMin;
        }
        if (discRigidbody.velocity.magnitude > speedMax)
        {
            discRigidbody.velocity = transform.forward * speedMax;
        }*/
        speed = Mathf.Clamp(speed, speedMin, speedMax);
        flyVector = Vector3.Scale(flyVector, lockVector);
        transform.Rotate(spinVector*Time.deltaTime);
        transform.Translate((flyVector) * speed * Time.deltaTime);
        //GetComponent<Rigidbody>().MoveRotation((spinVector) * Time.deltaTime);
        //GetComponent<Rigidbody>().MovePosition((flyVector) * speed * Time.deltaTime);
        
        //transform.forward = discRigidbody.velocity;
        //speed = discRigidbody.velocity.magnitude;
        //discRigidbody.AddForce(Vector3.forward * speed, ForceMode.Impulse);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void CaughtState()
    {
        if (initialForce > initialForceMin && transform.parent.GetComponent<Collider>().enabled == false)
        {
            initialForce -= speedDecay * Time.deltaTime;
        }
        if (transform.parent != null)
        {
            if (transform.parent.GetComponent<CatchScript>().GetThrow() == true && transform.parent.GetComponent<Collider>().enabled == false)
            {
                transform.parent.GetComponent<CatchScript>().SetThrowFalse();
                ThrowDisc();
            }
        }
    }

    public void SetPointMax(int newMax)
    {
        pointMax = newMax;
    }

    public void SetValues(int valuePoints, float valueInitialForce, float valueSpeedMax)
    {
        pointValue = valuePoints;
        initialForce = valueInitialForce;
        speedMax = valueSpeedMax;
    }

    public float GetInitialForce()
    {
        return initialForce;
    }

    public float GetPointValue()
    {
        return pointValue;
    }

    public float GetPointValueMax()
    {
        return pointMax;
    }

    public Transform GetPlayerOwner()
    {
        return playerOwner;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetSpeedMax()
    {
        return speedMax;
    }

    public void ResetDiscValues()
    {
        pointValue = 0;
        initialForce = initialForceMin;
        ThrowDisc();
        
    }

    public void ResetDiscValues2(Transform catchBox)
    {
        pointValue = 0;
        CatchDisc(catchBox);
    } 

    //These need to apply to the catchbox, not the individual disc
    public void ChangeDiscStats(float slideValue)
    {
        spinMax = defaultSpinMax + (slideValue * 6);
        spinDecay = defaultSpinDecay + (-slideValue * .05f);
        initialForce = defaultInitialForce * (Mathf.Pow(5, (-slideValue/10)));
        initialForceMin = speedMin = initialForce;
        speedMax = defaultSpeedMax + (-slideValue * 2);
    }

    public void DefaultDiscStats()
    {
        spinMax = defaultSpinMax;
        spinDecay = defaultSpinDecay;
        initialForce = defaultInitialForce;
        initialForceMin = speedMin = initialForce;
        speedMax = defaultSpeedMax;
    }

    void DidACoolThing()
    {
        if (speed > speedMax - 1 || Mathf.Abs(spinFactor) > spinMax - 1)
        {
            //Signal Music Manager    
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            discEventValue.setValue(1f);
            discEvent.start();
            if (pointValue < pointMax)
            {
                pointValue *= 2;
                if (pointValue < 1)
                {
                    pointValue += 1;
                }
                discDisplay.text = pointValue.ToString();
            }
            if (speed < speedMax)
            {
                //discRigidbody.AddForce(transform.forward * incrementForce/*, ForceMode.Impulse*/);
                speed += incrementForce;
                //Debug.Log("Speed2 = " + speed);
            }
        }
        if (other.tag == "CatchBox")
        {
            CatchDisc(other.transform);
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall" || other.collider.tag == "Disc" || other.collider.tag == "Player1" || other.collider.tag == "Player2")
        {
            discEventValue.setValue(7f);
            discEvent.start();
            spinVector *= spinDecay;
            if (speed > speedMin)
            {
                speed -= 1;
            }
            transform.forward = Vector3.Reflect(transform.forward.normalized, other.contacts[0].normal);
            transform.forward = Vector3.Scale(transform.forward, lockVector);
            //Debug.Log("Reflect is stupid: " + (flyVector * speed*Time.deltaTime).normalized*-1 + " reflected by " + other.contacts[0].normal + " = " + flyVector);
        }
    }
}
