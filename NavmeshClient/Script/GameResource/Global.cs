using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections;

static class Global
{
    public static readonly MapReferenceManager mapr_mgr = new MapReferenceManager();
    public static readonly GameConfig config = new GameConfig();
}

