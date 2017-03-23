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
        player1 = transform.parent.GetComponent<CatchScript>().GetPlayerType();
        if (player1 == true)
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.red);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor(Shader.PropertyToID("_Color"), Color.blue);
        }
        Physics.IgnoreLayerCollision(12, 8, true);
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
        catchBox.GetComponent<CatchScript>().IdentifyDisc();
        if (player1 == transform.parent.GetComponent<CatchScript>().GetPlayerType())
        {
            transform.parent.GetComponent<CatchScript>().Score(pointValue);
            if (player1 == true)
            {
                scoreEventValue.setValue(pointValue);
                scoreEvent.start();
            }
            pointValue = 0;
            discDisplay.text = pointValue.ToString();
            
        }
        else
        {
            discEventValue.setValue(2f);
            discEvent.start();
        }
        
        
        transform.localPosition = new Vector3(0, 0, 1);
        transform.forward = transform.parent.forward;
        initialForce = discRigidbody.velocity.magnitude * 50;
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
        if (initialForce > initialForceMin && transform.parent.GetComponent<Collider>().enabled == false)
        {
            initialForce -= Time.deltaTime * 200;
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

    public void SetValues(int valuePoints, float valueInitialForce)
    {
        pointValue = valuePoints;
        initialForce = valueInitialForce;
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
                discRigidbody.AddForce(transform.forward * incrementForce/*, ForceMode.Impulse*/);
            }
        }
        if (other.tag == "CatchBox")
        {
            CatchDisc(other.transform);
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall")
        {
            discEventValue.setValue(7f);
            discEvent.start();
        }
    }
}
