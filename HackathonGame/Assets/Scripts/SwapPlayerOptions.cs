using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapPlayerOptions : MonoBehaviour
{
    [SerializeField] GameObject inputOptions;
    [SerializeField] GameObject statOptions;
    [SerializeField] GameObject oppositeTab;
    

    public void SwapPlayerOptionTabs(bool input)
    {
        if (input == true && inputOptions.activeSelf == false)
        {
            inputOptions.SetActive(true);
            statOptions.SetActive(false);
            //GetComponent<Button>().enabled = false;
            //oppositeTab.GetComponent<Button>().enabled = true;
        }
        else if (input == false && statOptions.activeSelf == false)
        {
            statOptions.SetActive(true);
            inputOptions.SetActive(false);
            //GetComponent<Button>().enabled = false;
            //oppositeTab.GetComponent<Button>().enabled = true;
        }
    }
}
