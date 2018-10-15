using UnityEngine;
using System.Collections;

public class TriangleArea : MonoBehaviour
{
    public float Angle = 30f;
    public float Distance = 5f;
    Vector3 leftPoint, rightPoint;
    private float distance = 5f;
    void Update()
    {
        Quaternion r = transform.rotation;
        Vector3 f0 = (transform.position + (r * Vector3.forward) * distance);
        Debug.DrawLine(transform.position, f0, Color.red);

        Quaternion r0 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 30f, transform.rotation.eulerAngles.z);
        Quaternion r1 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 30f, transform.rotation.eulerAngles.z);

        leftPoint = (transform.position + (r0 * Vector3.forward) * distance);
        rightPoint = (transform.position + (r1 * Vector3.forward) * distance);

        Debug.DrawLine(transform.position, leftPoint, Color.red);
        Debug.DrawLine(transform.position, rightPoint, Color.red);
        Debug.DrawLine(leftPoint, rightPoint, Color.red);
    }

    public bool CheckTarget(GameObject target)
    {
        if (!target)
            return false;
        return isINTriangle(target.transform.position, this.gameObject.transform.position, leftPoint, rightPoint);
    }

    private static float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }

    public static bool isINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
