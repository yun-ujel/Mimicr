using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIMaster))]
public class UIMasterEditor : Editor
{
    UIMaster uMaster;
    string[] resizeNames = { "Top Left", "Top", "Top Right" , "Left", "None", "Right", "Bottom Left", "Bottom", "Bottom Right"};
    // index:                 0           1      2             3       4       5        6              7         8
    private void OnEnable()
    {
        uMaster = (UIMaster)target;
    }

    public override void OnInspectorGUI()
    {
        var uM = new SerializedObject(uMaster);

        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);

        if (uMaster.functionOnClick == UIMaster.ClickFunction.close)
        {
            uM.FindProperty("windowToClose").objectReferenceValue = EditorGUILayout.ObjectField("Window To Close", uMaster.windowToClose, typeof(GameObject), allowSceneObjects: true);
        }

        if (uMaster.functionOnClick == UIMaster.ClickFunction.sendMessageToCanvas)
        {
            uM.FindProperty("messageToSend").stringValue = EditorGUILayout.TextField("Message (to Send to Canvas)", uMaster.messageToSend);
        }

        if (uMaster.rectTransform == uMaster.GetComponent<RectTransform>() || uMaster.rectTransform == null)
        {
            uM.FindProperty("minWindowSize").vector2Value = EditorGUILayout.Vector2Field("Minimum Window Size", uMaster.minWindowSize);
        }


        if (uMaster.functionOnDrag == UIMaster.DragFunction.move)
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Move)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        }
        else if (uMaster.functionOnDrag == UIMaster.DragFunction.resize)
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Resize)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Resize From:");
            uM.FindProperty("anchorIndex").intValue = GUILayout.SelectionGrid(uMaster.anchorIndex, resizeNames, 3, GUILayout.MaxHeight(220), GUILayout.MaxWidth(280));
        }
        else if (uMaster.functionOnClick == UIMaster.ClickFunction.sendToTop)
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Send to Top)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        }

        uM.ApplyModifiedProperties();
    }
}
