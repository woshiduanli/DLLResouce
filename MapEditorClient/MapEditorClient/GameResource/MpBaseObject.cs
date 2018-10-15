using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpBaseObject {

    public RoleReference roleRef;

    GameObject gameobject;
    Transform transform {
        get
        {
            return gameobject.transform;
        }
    }
}
