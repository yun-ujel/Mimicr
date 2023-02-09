using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIMaster))]
public class UIMasterEditor : Editor
{
    UIMaster uMaster;
    Canvas canvas;

    bool showAnchorIndex;
    string[] resizeNames = { "Top Left (0)", "Top (1)", "Top Right (2)" , "Left (3)", "None (4)", "Right (5)", "Bottom Left (6)", "Bottom (7)", "Bottom Right (8)"};
    // index:                 0               1          2                 3           4           5            6                  7             8

    private void OnEnable()
    {
        uMaster = (UIMaster)target;
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public override void OnInspectorGUI()
    {
        var uM = new SerializedObject(uMaster);


        uM.FindProperty("canvas").objectReferenceValue = EditorGUILayout.ObjectField("Canvas", uMaster.canvas, typeof(Canvas), true);
        

        base.OnInspectorGUI();

        // Message to Send Field
        if (uMaster.functionOnClick == UIMaster.FunctionOnClick.sendMessageToCanvas)
        {
            uM.FindProperty("messageToSend").stringValue = EditorGUILayout.TextField("Message to Send to Canvas", uMaster.messageToSend);
        }

        // Window to Close Field
        else if (uMaster.functionOnClick == UIMaster.FunctionOnClick.close)
        {
            uM.FindProperty("windowToClose").objectReferenceValue = EditorGUILayout.ObjectField("Window To Close", uMaster.windowToClose, typeof(GameObject), allowSceneObjects: true);
        }

        EditorGUILayout.Space();

        // Drag Function Field
        if (uMaster.dragFunction == null && uMaster.TryGetComponent<DragFunction>(out DragFunction df))
        {
            uM.FindProperty("dragFunction").objectReferenceValue = df;   
        }
        else
        {
            EditorGUILayout.ObjectField("Drag Function", uMaster.dragFunction, typeof(DragFunction), true);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);

        // RectTransform Field
        uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("Target RectTransform", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);

        EditorGUILayout.Space();

        // Min Window Size Field
        if (uMaster.rectTransform == uMaster.GetComponent<RectTransform>() || uMaster.rectTransform == null)
        {
            uM.FindProperty("minWindowSize").vector2Value = EditorGUILayout.Vector2Field("Minimum Window Size", uMaster.minWindowSize);
            EditorGUILayout.Space();
        }

        // Anchor Index Field
        if (uMaster.dragFunction != null)
        {
            showAnchorIndex = EditorGUILayout.BeginFoldoutHeaderGroup(showAnchorIndex, "Anchor Index");

            if (showAnchorIndex)
            {
                uM.FindProperty("anchorIndex").intValue = GUILayout.SelectionGrid(uMaster.anchorIndex, resizeNames, 3, GUILayout.MaxHeight(160));
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        uM.ApplyModifiedProperties();
    }
}
