using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public GameObject[] objectsToSwap;

    public void OnTabSelected(int buttonIndex)
    {
        for (int i = 0; i < objectsToSwap.Length; i++)
        {
            if (i == buttonIndex)
            {
                objectsToSwap[i].transform.SetAsLastSibling();
            }
            else
            {
                objectsToSwap[i].transform.SetAsFirstSibling();
            }
        }
    }
}
