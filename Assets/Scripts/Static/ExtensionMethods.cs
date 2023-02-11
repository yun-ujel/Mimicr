using UnityEngine;
using System.Collections.Generic;

public static class ExtensionMethods
{
    public static Vector2 UnanchorPosition(this RectTransform rectTransform) // Calculate the position of the object as if the pivot and anchor was at the bottom left
    {
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
        Vector2 parentRectSize = parentTransform.rect.size;
        Vector2 rectSize = rectTransform.rect.size;


        Vector2 basePivotPosition = new Vector2
            (
                rectTransform.anchoredPosition.x - (rectSize.x * rectTransform.pivot.x), // Calculate the position of the object as if the pivot was at the bottom left
                rectTransform.anchoredPosition.y - (rectSize.y * rectTransform.pivot.y)  // This is done by subtracting the size of the object, multiplied by the pivot value
            );

        Vector2 baseAnchorPosition = new Vector2
        (
            basePivotPosition.x + (parentRectSize.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)), // Calculate the position of the object as if the anchor was at the bottom left
            basePivotPosition.y + (parentRectSize.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))  // This is done by taking the size of the parent and multiplying it by the anchors
        );                                                                                                             // 

        return baseAnchorPosition;
    }
    public static Vector2 ReanchorPosition(this RectTransform rectTransform, Vector2 unanchoredPosition) // Inverse of the UnanchorPosition method
    {
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
        Vector2 parentRectSize = parentTransform.rect.size;
        Vector2 rectSize = rectTransform.rect.size;

        Vector2 reAnchorPosition = new Vector2
        (
            unanchoredPosition.x - (parentRectSize.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)),
            unanchoredPosition.y - (parentRectSize.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))
        );

        Vector2 rePivotPosition = new Vector2
        (
            reAnchorPosition.x + (rectSize.x * rectTransform.pivot.x),
            reAnchorPosition.y + (rectSize.y * rectTransform.pivot.y)
        );

        return rePivotPosition;
    }

    public static int[] GetDigits(this int number)
    {
        string temp = number.ToString();
        int[] rtn = new int[temp.Length];
        for (int i = 0; i < rtn.Length; i++)
        {
            rtn[i] = int.Parse(temp[i].ToString());
        }
        return rtn;
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> returnedList = new List<T>();
        int count = list.Count;

        for (int i = 0; i < count; i++)
        {
            int selection = Random.Range(0, list.Count);
            returnedList.Add(list[selection]);
            list.Remove(list[selection]);
        }
        return returnedList;
    }
}
