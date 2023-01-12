using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIMaster))]
[InitializeOnLoad]
public class UIMasterEditor : Editor
{
    UIMaster uMaster;


    private void OnEnable()
    {
        uMaster = (UIMaster)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);
        uMaster.functionOnClick = (UIMaster.ClickFunction)EditorGUILayout.EnumPopup("Function On Click", uMaster.functionOnClick);
        uMaster.functionOnDrag = (UIMaster.DragFunction)EditorGUILayout.EnumPopup("Function On Drag", uMaster.functionOnDrag);

        EditorGUILayout.Space();

        if(uMaster.functionOnClick == UIMaster.ClickFunction.openOtherWindow)
        {
            uMaster.otherWindow = (GameObject)EditorGUILayout.ObjectField("Window to Spawn", uMaster.otherWindow, typeof(GameObject), allowSceneObjects: false);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References (will autofill on Start)", EditorStyles.boldLabel);
        uMaster.rectTransform = (RectTransform)EditorGUILayout.ObjectField("Rect Transform", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        uMaster.windowParent = (GameObject)EditorGUILayout.ObjectField("Parent Window", uMaster.windowParent, typeof(GameObject), allowSceneObjects: true);
    }
}
