using UnityEngine;

/// <summary>
/// 整型数值选择属性描述
/// </summary>
public class IntPopupAttribute : PropertyAttribute
{
    public string Lable { get; private set; }
    public string[] DisplayedOptions { get; private set; }
    public int[] OptionValues { get; private set; }

    public IntPopupAttribute(string label, string[] displayedOptions, int[] optionValues)
    {
        this.Lable = label;
        this.DisplayedOptions = displayedOptions;
        this.OptionValues = optionValues;
    }
}

/// <summary>
/// 枚举选择属性描述
/// </summary>
public class EnumPopupAttribute : PropertyAttribute
{
    public string Lable { get; private set; }
    public string[] DisplayedOptions { get; private set; }

    public EnumPopupAttribute(string label, string[] displayedOptions)
    {
        this.Lable = label;
        this.DisplayedOptions = displayedOptions;
    }
}

public class CommonAttribute : PropertyAttribute
{
    public string Lable { get; private set; }

    public CommonAttribute(string label)
    {
        this.Lable = label;
    }
}