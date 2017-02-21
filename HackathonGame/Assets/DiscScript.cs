using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscScript : MonoBehaviour
{
    [SerializeField] Text discDisplay;

    [SerializeField] bool player1;
    [SerializeField] bool caught;
    [SerializeField] float speed;
    [SerializeField] float speedMax;
    [SerializeField] float speedMin;
    [SerializeField] float incrementForce;
    [SerializeField] float initialForce;
    [SerializeField] float initialForceMin;
    [SerializeField] int pointValue;
    [SerializeField] int pointMax;
    Rigidbody discRigidbody;
    Collider discCollider;

    [FMODUnity.EventRef]
    [SerializeField] string wallHitEffectName = "event:/Cues/Wall Hit";
    FMOD.Studio.EventInstance wallHitEvent;

    //Player 1 Scores
    [FMODUnity.EventRef]
    [SerializeField] string p1pt1EffectName = "event:/Cues/P1 1 pt";
    FMOD.Studio.EventInstance score1Pt1Event;

    [FMODUnity.EventRef]
    [SerializeField] string p1pt2EffectName = "event:/Cues/P1 2 pt";
    FMOD.Studio.EventInstance score1Pt2Event;

    [FMODUnity.EventRef]
    [SerializeField] string p1pt4EffectName = "event:/Cues/P1 4 pt";
    FMOD.Studio.EventInstance score1Pt4Event;

    [FMODUnity.EventRef]
    [SerializeField] string p1pt8EffectName = "event:/Cues/P1 8 pt";
    FMOD.Studio.EventInstance score1Pt8Event;


    //Player 2 Scores
    [FMODUnity.EventRef]
    [SerializeField] string p2pt1EffectName = "event:/Cues/P2 1 pt";
    FMOD.Studio.EventInstance score2Pt1Event;

    [FMODUnity.EventRef]
    [SerializeField] string p2pt2EffectName = "event:/Cues/P2 2 pt";
    FMOD.Studio.EventInstance score2Pt2Event;

    [FMODUnity.EventRef]
    [SerializeField] string p2pt4EffectName = "event:/Cues/P2 4 pt";
    FMOD.Studio.EventInstance score2Pt4Event;

    [FMODUnity.EventRef]
    [SerializeField] string p2pt8EffectName = "event:/Cues/P2 8 pt";
    FMOD.Studio.EventInstance score2Pt8Event;


    [FMODUnity.EventRef]
    [SerializeField] string goalEffectName = "event:/Cues/Goal Pass";
    FMOD.Studio.EventInstance goalEvent;

    [FMODUnity.EventRef]
    [SerializeField] string interceptionEffectName = "event:/Cues/Interception";
    FMOD.Studio.EventInstance interceptEvent;



    // Use this for initialization
    void Start ()
    {
        wallHitEvent = FMODUnity.RuntimeManager.CreateInstance(wallHitEffectName);
        //contact1Event = FMODUnity.RuntimeManager.CreateInstance(contact1EffectName);
        //contact2Event = FMODUnity.RuntimeManager.CreateInstance(contact2EffectName);

        score1Pt1Event = FMODUnity.RuntimeManager.CreateInstance(p1pt1EffectName);
        score1Pt2Event = FMODUnity.RuntimeManager.CreateInstance(p1pt2EffectName);
        score1Pt4Event = FMODUnity.RuntimeManager.CreateInstance(p1pt4EffectName);
        score1Pt8Event = FMODUnity.RuntimeManager.CreateInstance(p1pt8EffectName);

        score2Pt1Event = FMODUnity.RuntimeManager.CreateInstance(p2pt1EffectName);
        score2Pt2Event = FMODUnity.RuntimeManager.CreateInstance(p2pt2EffectName);
        score2Pt4Event = FMODUnity.RuntimeManager.CreateInstance(p2pt4EffectName);
        score2Pt8Event = FMODUnity.RuntimeManager.CreateInstance(p2pt8EffectName);

        goalEvent = FMODUnity.RuntimeManager.CreateInstance(goalEffectName);

        interceptEvent = FMODUnity.RuntimeManager.CreateInstance(interceptionEffectName);
        discRigidbody = GetComponent<Rigidbody>();
        discCollider = GetComponent<CapsuleCollider>();
        caught = true;
        pointValue = 0;
        player1 = transform.parent.GetComponent<CatchScript>().GetPlayerType();
        if (player1 == true)
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.red);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.blue);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
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
        discRigidbody.AddForce(transform.forward * speed/*, ForceMode.Impulse*/);
    }

    public void CatchDisc(Transform catchBox)
    {
        //Debug.Log("Disc is caught!!");
        caught = true;
        discCollider.enabled = false;
        transform.parent = catchBox;
        if (player1 == transform.parent.GetComponent<CatchScript>().GetPlayerType())
        {
            transform.parent.GetComponent<CatchScript>().IncrementScore(pointValue);
            if (player1 == true)
            {
                if (pointValue <= 1)
                {
                    score1Pt1Event.start();
                }
                else if (pointValue == 2)
                {
                    score1Pt2Event.start();
                }
                else if (pointValue == 4)
                {
                    score1Pt4Event.start();
                }
                else if (pointValue == 8)
                {
                    score1Pt8Event.start();
                }
            }
            else
            {
                if (pointValue <= 1)
                {
                    score2Pt1Event.start();
                }
                else if (pointValue == 2)
                {
                    score2Pt2Event.start();
                }
                else if (pointValue == 4)
                {
                    score2Pt4Event.start();
                }
                else if (pointValue == 8)
                {
                    score2Pt8Event.start();
                }
            }
            pointValue = 0;
            discDisplay.text = pointValue.ToString();
            
        }
        else
        {
            interceptEvent.start();
        }
        
        
        transform.localPosition = new Vector3(0, 0, 1);
        transform.forward = transform.parent.forward;
        initialForce = discRigidbody.velocity.magnitude * 100/3;
        discRigidbody.velocity = Vector3.zero;
        discRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        player1 = transform.parent.GetComponent<CatchScript>().GetPlayerType();
        if (player1 == true)
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.red);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.blue);
        }
    }

    void FlyingState()
    {
        if (discRigidbody.velocity.magnitude < speedMin)
        {
            discRigidbody.velocity = transform.forward * speedMin;
        }
        if (discRigidbody.velocity.magnitude > speedMax)
        {
            discRigidbody.velocity = transform.forward * speedMax;
        }
        transform.forward = discRigidbody.velocity;
        speed = discRigidbody.velocity.magnitude;
        //discRigidbody.AddForce(Vector3.forward * speed, ForceMode.Impulse);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void CaughtState()
    {
        if (initialForce > initialForceMin)
        {
            initialForce -= Time.deltaTime * 200;
        }
    }

    public void SetValues(int valuePoints, float valueInitialForce)
    {
        pointValue = valuePoints;
        initialForce = valueInitialForce;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            goalEvent.start();
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
                discRigidbody.AddForce(transform.forward * incrementForce/*, ForceMode.Impulse*/);
            }
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall")
        {
            wallHitEvent.start();
        }
    }
}
