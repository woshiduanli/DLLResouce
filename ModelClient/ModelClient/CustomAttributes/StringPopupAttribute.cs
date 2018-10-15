using UnityEngine;

/// <summary>
/// 字符串值选择属性描述
/// </summary>
public class StringPopupAttribute : PropertyAttribute
{
    public string Lable { get; private set; }
    public string[] DisplayedOptions { get; private set; }
    public string[] OptionValues { get; private set; }

    public StringPopupAttribute(string label, string[] displayedOptions, string[] optionValues)
    {
        this.Lable = label;
        this.DisplayedOptions = displayedOptions;
        this.OptionValues = optionValues;
    }
}