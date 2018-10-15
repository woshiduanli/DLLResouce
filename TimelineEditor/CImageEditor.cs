using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(CImage))]
public class CImageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CImage image = target as CImage;
        GUI.changed = false;
        if (image.sprite != null)
        {
            if (image.sprite)
            {
                image.SpriteName = image.sprite.name.ToLower();
                image.AtlasName = image.sprite.texture.name.ToLower();
            }
        }
        EditorUtility.SetDirty(image);
    }
}
