using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

public static class Extension{

	public static readonly List<string>TakeTwoLowers=new List<string>
	{
		"AI",
		"AD",
		"HP",
		"MP",
		"AP"
	};

	public static List<GameObject> GetAllChildren(this GameObject parent)
	{
		List<GameObject> gos = new List<GameObject>();
		for (int i = 0; i < parent.transform.childCount; i++)
		{
			gos.Add(parent.transform.GetChild(i).gameObject);
		}
		return gos;
	}

	public static Vector3 FixPosition(this Vector3 v, float offsetZ)
	{
		return new Vector3(v.x, v.y, v.y * 10 + offsetZ);
	}

	public static Vector3 FixPosition(this Vector3 v)
	{
		return v.FixPosition(0);
	}

	public static T ToEnum<T>(this string str)
	{
		return (T)Enum.Parse(typeof(T), str);
	}
	
	public static string GetDescriptionByName<T>(this T enumItemName)
	{
		FieldInfo fi = enumItemName.GetType().GetField(enumItemName.ToString());
		
		DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
			typeof(DescriptionAttribute), false);
		
		if (attributes != null && attributes.Length > 0)
		{
			return attributes[0].Description;
		}
		else
		{
			return enumItemName.ToString();
		}
	}

    public static void RemoveAll<K, V>(this Dictionary<K, V> dict,
                                               Func<KeyValuePair<K, V>, bool> condition)
    {
        foreach ( var cur in dict.Where(condition).ToList() ) {
            dict.Remove(cur.Key);
        }
    }
    public static void WriteField(this LitJson.JsonWriter writer ,string propertyName,bool value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.Write(value);
    }
    public static void WriteField(this LitJson.JsonWriter writer, string propertyName, int value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.Write(value);
    }
    public static void WriteField(this LitJson.JsonWriter writer, string propertyName, float value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.Write(value);
    }
    public static void WriteField(this LitJson.JsonWriter writer, string propertyName, string value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.Write(value);
    }

    public static void WriteField(this LitJson.JsonWriter writer,string propertyName,Vector3 value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.WriteObjectStart();
        writer.WriteField("x", value.x);
        writer.WriteField("y", value.y);
        writer.WriteField("z",value.z);
        writer.WriteObjectEnd();
    }

    public static void WriteField(this LitJson.JsonWriter writer, string propertyName, Vector2 value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.WriteObjectStart();
        writer.WriteField("x",value.x);
        writer.WriteField("y",value.y);
        writer.WriteObjectEnd();
    }

	public static void WriteIntField(this LitJson.JsonWriter writer, string propertyName, Vector2 value)
	{
		propertyName = propertyName.Replace(".", "#");
		writer.WritePropertyName(propertyName);
		writer.WriteObjectStart();
		writer.WriteField("x",(int)value.x);
		writer.WriteField("y", (int)value.y);
		writer.WriteObjectEnd();
	}

	public static void WriteIntField(this LitJson.JsonWriter writer, string propertyName, Vector3 value)
	{
		propertyName = propertyName.Replace(".", "#");
		writer.WritePropertyName(propertyName);
		writer.WriteObjectStart();
		writer.WriteField("x",(int)value.x);
		writer.WriteField("y", (int)value.y);
		writer.WriteField("z", (int)value.z);
		writer.WriteObjectEnd();
	}

	public static void WriteField(this LitJson.JsonWriter writer, string propertyName, int[] value)
	{
        propertyName = propertyName.Replace(".", "#");
		writer.WritePropertyName(propertyName);
		writer.WriteArrayStart();
		for(int i = 0; i < value.Length; i++) writer.Write(value[i]);
		writer.WriteArrayEnd();
	}

    public static void WriteField(this LitJson.JsonWriter writer, string propertyName, string[] value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.WriteArrayStart();
        for (int i=0;i<value.Length;i++)
        {
            writer.Write(value[i]);
        }
        writer.WriteArrayEnd();
    } 

    public static void WriteField(this LitJson.JsonWriter writer,string propertyName,Quaternion value)
    {
        propertyName = propertyName.Replace(".", "#");
        writer.WritePropertyName(propertyName);
        writer.WriteObjectStart();
        writer.WriteField("x", value.eulerAngles.x);
        writer.WriteField("y", value.eulerAngles.y);
        writer.WriteField("z",value.eulerAngles.z);
        writer.WriteObjectEnd();
    }
    public static int ReadIntField(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return json.ContainsKey(propertyName)? json[propertyName].ToString().ToInt():0;
    }

    public static int ToInt(this string value)
    {
        var f = 0;
        int.TryParse(value, out f);
        return f;
    }
    public static float ReadFloatField(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
		float f;
		float.TryParse(json.ReadStringField(propertyName),out f);
		return f;
    }
    public static string ReadStringField(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return (json[propertyName]??"").ToString();
    }
    public static string[] ReadStringArray(this LitJson.JsonData json, string propertyName)
    {
        LitJson.JsonData strData = json[propertyName];
        int count = strData.Count;
        string[] array= new string[count];
        for(int i = 0; i < count; i++)
        {
            array[i] = strData[i].ToString();
        }
        return array;
    }
    public static bool ReadBoolField(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return (bool)json[propertyName];
    }

    public static Color ReadColorField(this LitJson.JsonData json, string propertyName)
    {
        var colorJson = json[propertyName];

        var color = new Color(colorJson.ReadFloatField("r"), colorJson.ReadFloatField("g"),
            colorJson.ReadFloatField("b"), colorJson.ReadFloatField("a"));
        return color;
    }

	public static int[] ReadIntArray(this LitJson.JsonData json, string propertyName)
	{
		LitJson.JsonData d = json[propertyName];

		int count = d.Count;
		int[] results = new int[count];
		for(int i = 0; i < count; i++)
		{
			results[i] = (int)d[i];
		}
		return results;
	}

    public static bool ContainsKey(this LitJson.JsonData json, string key)
    {
        return (json as IDictionary).Contains(key);
    }

    public static bool RemoveKey(this LitJson.JsonData json,string key)
    {
        if ((json as IDictionary).Contains(key))
        {
            (json as IDictionary).Remove(key);
            return true;
        }
        return false;
    }

    public static bool Like(this string toSearch, string toFind)
    {
        return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
    }
    
    public static string VectorToString(this Vector3 vector)
    {
        return vector.x + "," + vector.y + "," + vector.z;
    }
    public static Vector3 ReadVector3Field(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return new Vector3(json[propertyName].ReadFloatField("x"), json[propertyName].ReadFloatField("y"), json[propertyName].ReadFloatField("z"));
    }
    public static Vector3 ReadVector2Field(this LitJson.JsonData json, string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return new Vector3(json[propertyName].ReadFloatField("x"), json[propertyName].ReadFloatField("y"));
    }
    public static Quaternion ReadQuaternionField(this LitJson.JsonData json,string propertyName)
    {
        propertyName = propertyName.Replace(".", "#");
        return new Quaternion(json[propertyName].ReadFloatField("x"), json[propertyName].ReadFloatField("y"), json[propertyName].ReadFloatField("z"), 0);
    }
    public static void ForEach(this LitJson.JsonData json ,Action<LitJson.JsonData>method)
    {
        for(var i=0;i<json.Count;i++)
        {
            LitJson.JsonData j = json[i];
            method(j);
        }
    }


	public static Vector2 ToVector2(this string str)
	{
		if(str == string.Empty) return Vector2.zero;

		float[] ia = str.Split(',').Select(n => Convert.ToSingle(n)).ToArray();
		if(ia.Length != 2)
		{
			Debug.LogWarning(string.Format("[String Extention]: wrong number of parameters for convert to Vector2 , {0}", str));
			return Vector2.zero;
		}
		else
		{
			return new Vector2(ia[0], ia[1]);
		}
	}

	public static Vector3 ToVector3(this string str)
	{
		if(str == string.Empty) return Vector3.zero;

		float[] ia = str.Split(',').Select(n => Convert.ToSingle(n)).ToArray();
		if(ia.Length != 3)
		{
			Debug.LogWarning("[String Extention]: wrong number of parameters for convert to Vector3");
			return Vector3.zero;
		}
		else
		{
			return new Vector3(ia[0], ia[1], ia[2]);
		}
	}

	public static int[] ToIntArray(this string str)
	{
		if(str == string.Empty) return new int[]{};
		return str.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
	}

	public static float[] ToFloatArray(this string str)
	{
		if(str == string.Empty) return new float[]{};
		return str.Split(',').Select(n => Convert.ToSingle(n)).ToArray();
	}

    public static bool In(this string value, IEnumerable<string> list)
    {
        return list.Any(l=>l==value);
    }

    public static string ToXmlString(this Rect rect)
    {
        return string.Join(",", new[] {rect.x, rect.y, rect.width, rect.height}.Select(v=>v.ToString()).ToArray());
    }


	public static int[] StringToIntArray(this string str)
	{
		if(string.IsNullOrEmpty(str)) return new int[0];

		string[] ss = str.Split(new char[]{','});
		return Array.ConvertAll(ss, (s)=>Convert.ToInt32(s));
	}

	public static string ResourceKeyExportEncode(this string key)
	{
		string str = "";
		if(string.IsNullOrEmpty(key)) return str;
		str = TakeTwoLowers.Any(key.StartsWith) ? key.Substring(0, 2).ToLower() + key.Substring(2, key.Length - 2) : key.Substring(0, 1).ToLower() + key.Substring(1, key.Length - 1);
		return str.Replace(".", "#");
	}

	public static string ResourceKeyImportEncode(this string key)
	{
		string str = "";
		if(string.IsNullOrEmpty(key)) return str;
		str = TakeTwoLowers.Any(key.ToUpper().StartsWith) ? key.Substring(0, 2).ToUpper() + key.Substring(2, key.Length - 2) : key.Substring(0, 1).ToUpper() + key.Substring(1, key.Length - 1);
		return str.Replace("#", ".");
	}



    public static string BuildAnimationMsgName(this string ani, bool enter)
    {
        return "On" + ani + (enter ? "Start" : "Completed");
    }

    public static string SplitAnimationMsgName(this string msg, bool enter)
    {
        int index = msg.IndexOf(enter ? "Start" : "Completed");
        return msg.Substring(2, index - 2);
    }


    //public static string GetString(this XElement element, string attributeName)
    //{
    //    return element.Attribute(attributeName).Value;
    //}
    //................................见了鬼
    //public static T ReadEnumField<T>(this LitJson.JsonData json, string propertyName) where T : struct, IConvertible
    //{
    //    if(!typeof(T).IsEnum)throw new ArgumentException();
    //   var value= json.ReadIntField(propertyName);
    //   var values = Enum.GetValues(typeof(T));
    //   var index = 0;
    //    T defaultValue;
    //    foreach(T v in values)
    //    {
    //        if (index == 0) defaultValue = v;
    //        if (index == value) return v;
    //        index++;
    //    }
    //    return defaultValue;
    //}


	public static string FirstCharToUpper(this string input)
	{
		if (String.IsNullOrEmpty(input))
			throw new ArgumentException("ARGH!");
		return input.First().ToString().ToUpper() + input.Substring(1);
	}


}
