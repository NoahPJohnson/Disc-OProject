using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationTrackerUIScript : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector2 outputPosition;
    

    // Use this for initialization
    void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, playerTransform.position);
        GetComponent<Image>().color = playerTransform.GetChild(0).GetComponent<CatchScript>().GetPlayerColor();
        rectTransform.position = new Vector3(pos.x, pos.y, rectTransform.position.z);
        rectTransform.right = playerTransform.forward;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, playerTransform.position);
        rectTransform.position = new Vector3(pos.x, pos.y, rectTransform.position.z);
        rectTransform.rotation = new Quaternion(rectTransform.rotation.x, rectTransform.rotation.y, playerTransform.GetChild(0).rotation.y*-1, playerTransform.GetChild(0).rotation.w);
        if (playerTransform.GetChild(0).GetComponent<PlayerRotationScript>().GetRevFactor() < 0)
        {
            GetComponent<Image>().fillClockwise = false;
        }
        else
        {
            GetComponent<Image>().fillClockwise = true;
        }
        GetComponent<Image>().fillAmount = Mathf.Abs(playerTransform.GetChild(0).GetComponent<PlayerRotationScript>().GetRevFactor()) / playerTransform.GetChild(0).GetComponent<PlayerRotationScript>().GetRevMax();
    }
}
