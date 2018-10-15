using System.Reflection;

public class EditorGuiUtility
{
    private static PropertyInfo LookLikeInspectorProperty = null;

    public static bool LookLikeInspector
    {
        get
        {
            if (LookLikeInspectorProperty == null)
            {
                LookLikeInspectorProperty = typeof(UnityEditor.EditorGUIUtility).GetProperty("LookLikeControls",
                                                                                    BindingFlags.Static |
                                                                                    BindingFlags.Public);
            }
            return (bool)LookLikeInspectorProperty.GetValue(null, null);
        }
    }
}