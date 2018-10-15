using UnityEngine;
using System.Collections;

public class CurvePoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Aperture_CurvePoint.tiff");
    }
}


