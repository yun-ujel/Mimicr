using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CanvasHandler))]
public class CanvasHandlerEditor : Editor
{
    CanvasHandler cHandler;
    public int paletteIndex;
    private void OnEnable()
    {
        cHandler = (CanvasHandler)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}
