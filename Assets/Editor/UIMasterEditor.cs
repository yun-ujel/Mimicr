using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIMaster))]
//[InitializeOnLoad]
public class UIMasterEditor : Editor
{
    UIMaster uMaster;

    private void OnEnable()
    {
        uMaster = (UIMaster)target;
    }

    public override void OnInspectorGUI()
    {
        var uM = new SerializedObject(uMaster);

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.Space();



        if(uMaster.functionOnClick == UIMaster.ClickFunction.close)
        {
            uM.FindProperty("windowToClose").objectReferenceValue = EditorGUILayout.ObjectField("Window To Close", uMaster.windowToClose, typeof(GameObject), allowSceneObjects: true);
        }
        
        if(uMaster.functionOnDrag == UIMaster.DragFunction.move)
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Move)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
            uM.FindProperty("movementBoundaries").objectReferenceValue = EditorGUILayout.ObjectField("Movement Boundaries", uMaster.movementBoundaries, typeof(RectTransform), allowSceneObjects: true);
        }
        else if 
            (
                uMaster.functionOnDrag == UIMaster.DragFunction.resizeBoth ||
                uMaster.functionOnDrag == UIMaster.DragFunction.resizeWidth ||
                uMaster.functionOnDrag == UIMaster.DragFunction.resizeHeight
            )
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Resize)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        }
        else if(uMaster.functionOnClick == UIMaster.ClickFunction.sendToTop)
        {
            uM.FindProperty("rectTransform").objectReferenceValue = EditorGUILayout.ObjectField("RectTransform (to Send to Top)", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        }


        uM.ApplyModifiedProperties();
    }
}
