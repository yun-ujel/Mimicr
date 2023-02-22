using System.Collections.Generic;
using UnityEngine;

public class AllAccountsDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform contentRectTransform;
    [SerializeField] private GameObject individualAcccountPrefab;

    [SerializeField] private RectTransform topRectTransform;
    private void UpdateAccountView(AccountInfo[] accountsInfo)
    {
        int childCount = contentRectTransform.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(contentRectTransform.GetChild(0).gameObject);
        }

        for (int i = 0; i < accountsInfo.Length; i++)
        {
            GameObject accountDisplay = Instantiate(individualAcccountPrefab, contentRectTransform.transform);
            accountDisplay.SendMessage("SetGraphics", accountsInfo[i]);
        }

        topRectTransform.offsetMax += new Vector2(0.01f, 0);
        // This one line of code somehow single-handedly fixed the layouts being slightly off on load
    }
}
