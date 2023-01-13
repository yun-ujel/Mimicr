using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIMaster))]
//[InitializeOnLoad]
public class UIMasterEditor : Editor
{
    UIMaster uMaster;
    public string[] clickFunctions = new string[]
    {
        "None",
        "Send To Top",
        "Close",
        "Open Other Window"
    };
    public string[] dragFunctions = new string[]
{
        "None",
        "Move",
        "Resize Width and Height",
        "Resize Width",
        "Resize Height"
};


    private void OnEnable()
    {
        uMaster = (UIMaster)target;
    }

    public override void OnInspectorGUI()
    {
        

        var uM = new SerializedObject(uMaster);

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        uM.FindProperty("clickFunctionIndex").intValue = EditorGUILayout.Popup("Function On Click", uMaster.clickFunctionIndex, clickFunctions);
        uM.FindProperty("dragFunctionIndex").intValue = EditorGUILayout.Popup("Function On Drag", uMaster.dragFunctionIndex, dragFunctions);
        uM.ApplyModifiedProperties();


        EditorGUILayout.Space();

        if (uMaster.clickFunctionIndex == 4)
        {

        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References (will autofill on Start)", EditorStyles.boldLabel);
        uMaster.rectTransform = (RectTransform)EditorGUILayout.ObjectField("Rect Transform", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        uMaster.windowParent = (GameObject)EditorGUILayout.ObjectField("Parent Window", uMaster.windowParent, typeof(GameObject), allowSceneObjects: true);

        //EditorGUILayout.LabelField("Yadda Yadda Yadda");

        DrawDefaultInspector();
    }
}
