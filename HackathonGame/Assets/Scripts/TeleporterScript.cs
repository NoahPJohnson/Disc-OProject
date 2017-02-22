using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField] Transform otherTeleporter;
    [SerializeField] int offset;
    //[SerializeField] Vector3 newPosition;

    [FMODUnity.EventRef]
    [SerializeField]
    string portalEffectName = "event:/Cues/Passing Through Portal";
    FMOD.Studio.EventInstance portalEvent;

    // Use this for initialization
    void Start ()
    {
        portalEvent = FMODUnity.RuntimeManager.CreateInstance(portalEffectName);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider disc)
    {
        if (disc.tag == "Disc")
        {
            //Debug.Log("TELEPORT");
            disc.transform.position = new Vector3(otherTeleporter.position.x + offset, disc.transform.position.y, disc.transform.position.z);
            portalEvent.start();
        }
    }
}
