using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(ColourController))]
public class ColourControllerEditor : Editor
{
    ColourController cController;

    private void OnEnable()
    {
        cController = (ColourController)target;
    }

    public override void OnInspectorGUI()
    {
        var cC = new SerializedObject(cController);

        base.OnInspectorGUI();

        if (cController.image == null && cController.rawImage == null && cController.text == null)
        {
            cC.FindProperty("rawImage").objectReferenceValue = cController.GetComponent<RawImage>();
            cC.FindProperty("image").objectReferenceValue = cController.GetComponent<Image>();
            cC.FindProperty("text").objectReferenceValue = cController.GetComponent<TextMeshProUGUI>();
        }
        else if (cController.image != null)
        {
            EditorGUILayout.ObjectField("Graphic: ", cController.image, typeof(Image), true);
        }
        else if (cController.rawImage != null)
        {
            EditorGUILayout.ObjectField("Graphic: ", cController.rawImage, typeof(RawImage), true);
        }
        else if (cController.text != null)
        {
            EditorGUILayout.ObjectField("Graphic: ", cController.text, typeof(TextMeshProUGUI), true);
        }
        else
        {
            EditorGUILayout.HelpBox("No UI Graphic Found! check for Image, Raw Image or TextMeshPro Text", MessageType.Error);
        }

        if (cController.outline == null)
        {
            cC.FindProperty("outline").objectReferenceValue = cController.GetComponent<Outline>();
        }
        else
        {
            EditorGUILayout.ObjectField("Outline: ", cController.outline, typeof(Outline), true);
        }

        cC.ApplyModifiedProperties();
    }
}
