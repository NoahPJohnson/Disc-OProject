using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectIndexScript : MonoBehaviour
{
    [SerializeField] GameObject labelObject;
    [SerializeField] string[] namesArray;
    [SerializeField] int contentArraySize;
    [SerializeField] int index = 0;

    public void SetArraySize(int size)
    {
        contentArraySize = size;
    }

    public void ChangeIndex(bool right)
    {
        if (right == true && index < (contentArraySize - 1))
        {
            index++;
        }
        else if (right == true && index == (contentArraySize - 1))
        {
            index = 0;
        }
        else if (right == false && index > 0)
        {
            index--;
        }
        else if (right == false && index == 0)
        {
            index = (contentArraySize - 1);
        }
        if (labelObject != null)
        {
            labelObject.GetComponent<Text>().text = namesArray[index];
        }
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
        if (labelObject != null)
        {
            labelObject.GetComponent<Text>().text = namesArray[index];
        }
    }

    public int GetIndex()
    {
        return index;
    }
}
