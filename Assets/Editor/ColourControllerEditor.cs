using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(ColourController))]
public class ColourControllerEditor : Editor
{
    ColourController cController;
    CanvasHandler cHandler;
    SerializedObject cC;

    private void OnEnable()
    {
        cController = (ColourController)target;
        cHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasHandler>();
        cC = new SerializedObject(cController);

        // Auto Assign UI Graphic to Colour Controller
        if (cController.image == null && cController.rawImage == null && cController.text == null)
        {
            cC.FindProperty("rawImage").objectReferenceValue = cController.GetComponent<RawImage>();
            cC.FindProperty("image").objectReferenceValue = cController.GetComponent<Image>();
            cC.FindProperty("text").objectReferenceValue = cController.GetComponent<TextMeshProUGUI>();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Colours", EditorStyles.boldLabel);

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        // Display UI Graphics
        EditorGUILayout.LabelField("Graphics", EditorStyles.boldLabel);

        if (cController.image != null)
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
        {  // Auto Assign Outline if empty
            cC.FindProperty("outline").objectReferenceValue = cController.GetComponent<Outline>();
        }
        else
        {
            EditorGUILayout.ObjectField("Outline: ", cController.outline, typeof(Outline), true);
        }

        cC.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Canvas Handler", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(cHandler, typeof(CanvasHandler), true);


        // Finds the Graphic and auto assigns the colour while in the editor
        if (GUILayout.Button("Apply Colour"))
        {
            ApplyColour();
        }
    }


    void ApplyColour()
    {
        if (cController.image != null)
        {
            var graphic = new SerializedObject(cController.image);

            graphic.FindProperty("m_Color").colorValue = FindBaseColor();

            graphic.ApplyModifiedProperties();
        }
        else if (cController.rawImage != null)
        {
            var graphic = new SerializedObject(cController.rawImage);

            graphic.FindProperty("m_Color").colorValue = FindBaseColor();

            graphic.ApplyModifiedProperties();
        }
        else if (cController.text != null)
        {
            var graphic = new SerializedObject(cController.text);

            graphic.FindProperty("m_fontColor").colorValue = FindBaseColor();

            graphic.ApplyModifiedProperties();
        }

        if (cController.outline != null)
        {
            var outline = new SerializedObject(cController.outline);

            outline.FindProperty("m_EffectColor").colorValue = FindOutlineColor();

            outline.ApplyModifiedProperties();
        }
    }

    Color FindBaseColor()
    {
        if (cController.colourType == ColourType.tone0)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone0;
        }
        else if (cController.colourType == ColourType.tone1)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone1;
        }
        else if (cController.colourType == ColourType.tone2)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone2;
        }
        else if (cController.colourType == ColourType.tone3)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone3;
        }
        else if (cController.colourType == ColourType.tone4)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone4;
        }
        else if (cController.colourType == ColourType.bright)
        {
            return cHandler.palettes[cHandler.currentPalette].Bright;
        }
        else if (cController.colourType == ColourType.outline)
        {
            return cHandler.palettes[cHandler.currentPalette].Outline;
        }
        else if (cController.colourType == ColourType.wildCard)
        {
            return cHandler.palettes[cHandler.currentPalette].WildCard;
        }

        return Color.black;
    }

    Color FindOutlineColor()
    {
        if (cController.outlineType == ColourType.tone0)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone0;
        }
        else if (cController.outlineType == ColourType.tone1)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone1;
        }
        else if (cController.outlineType == ColourType.tone2)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone2;
        }
        else if (cController.outlineType == ColourType.tone3)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone3;
        }
        else if (cController.outlineType == ColourType.tone4)
        {
            return cHandler.palettes[cHandler.currentPalette].Tone4;
        }
        else if (cController.outlineType == ColourType.bright)
        {
            return cHandler.palettes[cHandler.currentPalette].Bright;
        }
        else if (cController.outlineType == ColourType.outline)
        {
            return cHandler.palettes[cHandler.currentPalette].Outline;
        }
        else if (cController.outlineType == ColourType.wildCard)
        {
            return cHandler.palettes[cHandler.currentPalette].WildCard;
        }

        return Color.black;
    }
}
