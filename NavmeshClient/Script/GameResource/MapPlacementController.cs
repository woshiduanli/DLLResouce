using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using G9ZUnityGUI;
using System.IO;
using System;
using System.Linq;
using System.Text;
using System.Reflection;
public class MapPlacementController : MonoBehaviour 
{
    public static MapPlacementController instance = null;
    public static MapReference curMap = null;
    public static string textResourcesPath = Directory.GetCurrentDirectory() + "/resources/";
   
}
