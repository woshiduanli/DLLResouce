using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public List<int> Value = new List<int>();
    public string Lable;
    public bool And;

    public ConditionalHideAttribute(string lable, string conditionalSourceField, bool and, params int[] values)
    {
        this.Lable = lable;
        this.Value.AddRange(values);
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.And = and;
    }

    public ConditionalHideAttribute(bool hideInInspector)
    {
        this.ConditionalSourceField = string.Empty;
        this.HideInInspector = hideInInspector;
    }
}



