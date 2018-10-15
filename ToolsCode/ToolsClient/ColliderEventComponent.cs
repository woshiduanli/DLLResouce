using System;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class ColliderEventComponent : MonoBehaviour
{
    public BoxCollider boxcollider;
    public Action<GameObject, Collider> onTriggerEnter = null;
    public Action<GameObject, Collider> onTriggerExit = null;
    public Action<GameObject, Collider> onTriggerStay = null;

    public Action<GameObject, Collision> onCollisionEnter = null;
    public Action<GameObject, Collision> onCollisionExit = null;
    public Action<GameObject, Collision> onCollisionStay = null;


    void Start()
    {
        boxcollider = this.GetComponentInChildren<BoxCollider>();
    }

    public bool isTrigger
    {
        get
        {
            return this.boxcollider ? this.boxcollider.isTrigger : false;
        }
        set
        {
            if (boxcollider)
                this.boxcollider.isTrigger = value;
        }
    }

    void Destory()
    {
        onTriggerEnter = null;
        onTriggerExit = null;
        onTriggerStay = null;
        onCollisionEnter = null;
        onCollisionExit = null;
        onCollisionStay = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (onCollisionEnter != null)
            onCollisionEnter(gameObject, collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (onCollisionExit != null)
            onCollisionExit(gameObject, collision);
    }

    void OnCollisionStay(Collision collision)
    {
        if (onCollisionStay != null)
            onCollisionStay(gameObject, collision);
    }

    void OnTriggerEnter(Collider other)
    {
        if (onTriggerEnter != null)
            onTriggerEnter(gameObject, other);
    }

    void OnTriggerExit(Collider other)
    {
        if (onTriggerExit != null)
            onTriggerExit(gameObject, other);
    }

    void OnTriggerStay(Collider other)
    {
        if (onTriggerStay != null)
            onTriggerStay(gameObject, other);
    }
}