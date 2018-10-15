using UnityEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 可拖放对象到对象路径的转换属性描述
/// </summary>
public class AssetToFilePathAttribute : ConditionalHideAttribute
{
    public string AssetExt { get; private set; }

    public AssetToFilePathAttribute(string lable, string ext, string cond = "",bool and = true, params int[] values) : base(lable, cond, and, values)
    {
        this.Lable = lable;
        this.AssetExt = ext.ToLower();
    }

    public AssetToFilePathAttribute(bool hideInInspector = false) : base(hideInInspector) { }
}