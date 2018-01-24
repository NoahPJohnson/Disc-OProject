using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightButtonEventScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] GameObject subObject;
    [SerializeField] bool fadeSubObjectIn = false;
    [SerializeField] Text textToSet;
    [SerializeField] string defaultTemplateString = "Bot Handicap: ";

    Coroutine fadeCoroutine;


	// Use this for initialization
	void Start ()
    {
        
	    //GetComponent<Button>().On	
	}

    public void OnSelect(BaseEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeSubObject(fadeSubObjectIn));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        //fadeCoroutine = StartCoroutine(FadeSubObject(false));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeSubObject(fadeSubObjectIn));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        //fadeCoroutine = StartCoroutine(FadeSubObject(false));
    }

    IEnumerator FadeSubObject(bool fadeIn)
    {
        if (fadeIn == true)
        {
            if (subObject.activeSelf == false)
            {
                subObject.SetActive(true);
            }
            while (subObject.GetComponent<CanvasGroup>().alpha < 1)
            {
                subObject.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            while (subObject.GetComponent<CanvasGroup>().alpha > 0)
            {
                subObject.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
                yield return null;
            }
            if (subObject.activeSelf == true)
            {
                subObject.SetActive(false);
            }
        }
    }

    public void SetTextOnSubObject(float textValue)
    {
        textValue = 2 - (textValue);
        textToSet.text = defaultTemplateString + textValue.ToString("F2");
    }
}
