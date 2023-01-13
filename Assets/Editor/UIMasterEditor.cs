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
    public int clickFunctionIndex = 1;

    private void OnEnable()
    {
        uMaster = (UIMaster)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        var uM = new SerializedObject(uMaster);
        //SerializedProperty joe = uM.FindProperty("functionOnClick");

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        //uM.FindProperty("functionOnClick").joe = EditorGUILayout.EnumPopup("Function On Click", uMaster.functionOnClick);

        uMaster.functionOnClick = (UIMaster.ClickFunction)EditorGUILayout.EnumPopup("Function On Click", uMaster.functionOnClick);
        uMaster.functionOnDrag = (UIMaster.DragFunction)EditorGUILayout.EnumPopup("Function On Drag", uMaster.functionOnDrag);

        clickFunctionIndex = EditorGUILayout.Popup("Function On Click (test)", clickFunctionIndex, clickFunctions);

        //yadda.FindProperty("yaddaValue").floatValue = EditorGUILayout.Slider("Joe Balls value", uMaster.yaddaValue, 0.1f, 10f);
        //yadda.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (uMaster.functionOnClick == UIMaster.ClickFunction.openOtherWindow)
        {
            uMaster.otherWindow = (GameObject)EditorGUILayout.ObjectField("Window to Spawn", uMaster.otherWindow, typeof(GameObject), allowSceneObjects: false);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References (will autofill on Start)", EditorStyles.boldLabel);
        uMaster.rectTransform = (RectTransform)EditorGUILayout.ObjectField("Rect Transform", uMaster.rectTransform, typeof(RectTransform), allowSceneObjects: true);
        uMaster.windowParent = (GameObject)EditorGUILayout.ObjectField("Parent Window", uMaster.windowParent, typeof(GameObject), allowSceneObjects: true);

        //EditorGUILayout.LabelField("Yadda Yadda Yadda");


    }
}
