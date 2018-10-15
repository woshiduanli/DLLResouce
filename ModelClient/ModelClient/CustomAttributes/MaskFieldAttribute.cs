using UnityEngine;

/// <summary>
/// 掩码属性描述
/// </summary>
public class MaskFieldAttribute : PropertyAttribute
{
    public string Lable { get; private set; }
    public string[] DisplayedOptions { get; private set; }

    public MaskFieldAttribute(string label, string[] displayedOptions)
    {
        this.Lable = label;
        this.DisplayedOptions = displayedOptions;
    }
}