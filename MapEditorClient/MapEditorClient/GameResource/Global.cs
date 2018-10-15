using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections;

static class Global
{
    public static ReferenceManager<MapObjectConfigReference> mapobj_config_mgr = new ReferenceManager<MapObjectConfigReference>();
    public static readonly ItemReferenceManager item_mgr = new ItemReferenceManager();
    public static readonly ReferenceManager<RoleReference> role_mgr = new ReferenceManager<RoleReference>();
    public static readonly ReferenceManager<SkillReference> sr_mgr = new ReferenceManager<SkillReference>();
    public static readonly ReferenceManager<BuffReference> buff_r_mgr = new ReferenceManager<BuffReference>();
    public static readonly ReferenceManager<ProductReference> product_r_mgr = new ReferenceManager<ProductReference>();
    public static readonly MapObjectReferenceManager mapobj_r_mgr = new MapObjectReferenceManager();
    public static readonly MapReferenceManager mapr_mgr = new MapReferenceManager();
    public static readonly GameConfig config = new GameConfig();
}

