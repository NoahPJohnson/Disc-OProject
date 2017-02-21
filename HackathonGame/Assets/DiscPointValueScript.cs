using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscPointValueScript : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Transform discTransform;
    [SerializeField] Vector2 outputPosition;

	// Use this for initialization
	void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, discTransform.position);
        //rectTransform.position = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, mainCamera, );
        rectTransform.position = new Vector3(pos.x, pos.y, rectTransform.position.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, discTransform.position);
        //rectTransform.position = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, mainCamera, );
        rectTransform.position = new Vector3(pos.x, pos.y, rectTransform.position.z);
    }
}
