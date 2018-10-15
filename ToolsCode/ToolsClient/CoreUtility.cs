using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public static class CoreUtility {
    //---------------------------------------------------------------------
    public static Action quitImpl = null;

    //---------------------------------------------------------------------
    public static void Quit() {
        if (quitImpl != null) {
            quitImpl();
        } else if (!Application.isEditor) {
            Application.Quit();
        } else {
            Debug.LogWarning("Can not quit in the editor.");
        }
    }

    //---------------------------------------------------------------------
    public static void SetActive(Transform trans, bool active) {
        if (trans != null && trans.gameObject.activeSelf != active) {
            trans.gameObject.SetActive(active);
        }
    }

    //---------------------------------------------------------------------
    public static void SetActive(GameObject go, bool active) {
        if (go != null && go.activeSelf != active) {
            go.SetActive(active);
        }
    }

    //---------------------------------------------------------------------
    public static void SetActive(Component comp, bool active) {
        if (comp != null && comp.gameObject.activeSelf != active) {
            comp.gameObject.SetActive(active);
        }
    }

    //---------------------------------------------------------------------
    public static bool CheckIsUrl(string url) {
        return RegexMatch(url, @"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$");
    }

    //---------------------------------------------------------------------
    public static bool RegexMatch(string input, string pattern) {
        if (input != null && input.Trim() != "") {
            return Regex.IsMatch(input, pattern);
        }

        return false;
    }

    //---------------------------------------------------------------------
    public static bool IsAsset(Transform transform) {
        return IsAsset(transform.gameObject);
    }

    //---------------------------------------------------------------------
    public static bool IsAsset(GameObject gameObject) {
        if (gameObject.activeSelf) {
            return !gameObject.activeInHierarchy;
        } else {
            bool lastActived = gameObject.activeSelf;
            gameObject.SetActive(true);
            bool result = !gameObject.activeInHierarchy;
            gameObject.SetActive(lastActived);
            return result;
        }
    }


    //---------------------------------------------------------------------
    public static bool ForEach(Transform transform,
        Func<Transform, bool> functor) {
        if (transform == null) {
            return false;
        }

        if (functor(transform)) {
            return true;
        }

        for (int i = 0; i < transform.childCount; ++i) {
            if (ForEach(transform.GetChild(i), functor)) {
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------
    public static bool ForEach(Transform transform,
        Func<Transform, Transform, bool> functor) {
        return ForEach(transform, transform, functor);
    }

    //---------------------------------------------------------------------
    private static bool ForEach(Transform rootTrans, Transform child,
        Func<Transform, Transform, bool> functor) {
        if (child == null) {
            return false;
        }

        if (functor(rootTrans, child)) {
            return true;
        }

        for (int i = 0; i < child.childCount; ++i) {
            if (ForEach(rootTrans, child.GetChild(i), functor)) {
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------
    public static void AttachChild(Transform parent, Transform child) {
        AttachChild(parent, child, false);
    }

    //---------------------------------------------------------------------
    public static void AttachChild(Transform parent,
        Transform child, bool inheritLayer) {
        if (child == null) {
            return;
        }

        child.parent = parent;

        if (parent == null) {
            return;
        }

        if (inheritLayer && parent.gameObject.layer != child.gameObject.layer) {
            ForEach(parent, HanleModifyLayer);
        }
    }

    //---------------------------------------------------------------------
    public static void SaveLocalTransform(Transform transform) {
        ms_KeepLocalTransform = true;
        ms_KeepLocalPosition = transform.localPosition;
        ms_KeepLocalRotation = transform.localRotation;
        ms_KeepLocalScale = transform.localScale;
    }

    //---------------------------------------------------------------------
    public static void RevertLocalTransform(Transform transform) {
        if (!ms_KeepLocalTransform) {
            Debug.LogWarning("You must call BeginLocalTransform first.");
        }

        transform.localPosition = ms_KeepLocalPosition;
        transform.localRotation = ms_KeepLocalRotation;
        transform.localScale = ms_KeepLocalScale;
    }

    //---------------------------------------------------------------------
    public static void SaveTransform(Transform transform) {
        ms_KeepTransform = true;
        ms_KeepPosition = transform.position;
        ms_KeepRotation = transform.rotation;
    }

    //---------------------------------------------------------------------
    public static void RevertTransform(Transform transform) {
        if (!ms_KeepTransform) {
            Debug.LogWarning("You must call BeginTransform first.");
        }

        transform.position = ms_KeepPosition;
        transform.rotation = ms_KeepRotation;
    }

    //---------------------------------------------------------------------
    public static void NormalizeTransform(GameObject gameObject) {
        NormalizeTransform(gameObject.transform);
    }

    //---------------------------------------------------------------------
    public static void NormalizeTransform(Transform transform) {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    //---------------------------------------------------------------------
    public static void Destroy(UnityEngine.Object unityObject) {
        if (unityObject == null) {
            return;
        }

        if (!Application.isPlaying) {
            UnityEngine.Object.DestroyImmediate(unityObject);
        }

        if (unityObject is GameObject) {
            GameObject go = unityObject as GameObject;
            if (go != null) {
                go.transform.parent = null;
                unityObject = go;

                // go.BroadcastMessage("OnWillDestroy", 
                //     SendMessageOptions.DontRequireReceiver);
            } else {
                unityObject = null;
            }
        } else if (unityObject is Transform) {
            Transform trans = unityObject as Transform;
            if (trans != null) {
                trans.parent = null;
                unityObject = trans.gameObject;
            } else {
                unityObject = null;
            }
        }

        if (unityObject != null) {
            UnityEngine.Object.Destroy(unityObject);
        }
    }

    //---------------------------------------------------------------------
    public static void DestroyImmediate(UnityEngine.Object unityObject) {
        DestroyImmediate(unityObject, false);
    }

    //---------------------------------------------------------------------
    public static void DestroyImmediate(UnityEngine.Object unityObject, bool allowAssets) {
        if (unityObject == null) {
            return;
        }

        if (!Application.isEditor) {
            Destroy(unityObject);
        } else {
            UnityEngine.Object.DestroyImmediate(unityObject, allowAssets);
        }
    }

    //---------------------------------------------------------------------
    public static void GCCollect(bool deepMode) {
        if (deepMode) {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        } else {
            System.GC.Collect(0, System.GCCollectionMode.Optimized);
        }
    }

    //---------------------------------------------------------------------
    public static bool Equals(UnityEngine.Object[] a, UnityEngine.Object[] b) {
        if ((a == null && b == null)) {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length) {
            return false;
        }

        for (int i = 0; i < a.Length; ++i) {
            if (a[i] != b[i]) {
                return false;
            }
        }

        return true;
    }

    #region Internal Method
    //---------------------------------------------------------------------
    private static bool HanleModifyLayer(Transform root, Transform child) {
        child.gameObject.layer = root.gameObject.layer;
        return false;
    }
    #endregion

    #region Internal Member
    //---------------------------------------------------------------------
    private static Vector3 ms_KeepPosition = Vector3.zero;
    private static Quaternion ms_KeepRotation = Quaternion.identity;
    private static bool ms_KeepTransform = false;

    //---------------------------------------------------------------------
    private static Vector3 ms_KeepLocalPosition = Vector3.zero;
    private static Quaternion ms_KeepLocalRotation = Quaternion.identity;
    private static Vector3 ms_KeepLocalScale = Vector3.one;
    private static bool ms_KeepLocalTransform = false;
    #endregion
}
