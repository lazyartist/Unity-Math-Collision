using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment_Sphere_Collision : MonoBehaviour
{
    public Transform P1;
    public Transform P2;
    public Transform Sphere;

    private void OnDrawGizmos()
    {
        Vector3 s = Sphere.position;
        float r = Sphere.localScale.x * 0.5f;
        Vector3 p = P1.position;
        Vector3 v = P2.position - P1.position;

        Vector3 k = new Vector3(p.x - s.x, p.y - s.y, p.z - s.z);

        // ax^2 + 2bx + c = 0
        float a = Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2);
        float b = k.x * v.x + k.y * v.y + k.z * v.z;
        float c = Mathf.Pow(k.x, 2) + Mathf.Pow(k.y, 2) + Mathf.Pow(k.z, 2) - Mathf.Pow(r, 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p, p + v);

        // 판별식
        float d = Mathf.Pow(b, 2) - (a * c);
        if(d < 0) // 근이 없다
        {
            return;
        }

        // 근의 공식
        float t1 = (-b + Mathf.Sqrt(d)) / a;
        float t2 = (-b - Mathf.Sqrt(d)) / a;

        Debug.Log(t1 + ", " + t2);

        if (t1 >= 0f && t1 <= 1f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(v.x * t1 + p.x, v.y * t1 + p.y, v.z * t1 + p.z), 1);
        }

        if (t2 >= 0f && t2 <= 1f)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(v.x * t2 + p.x, v.y * t2 + p.y, v.z * t2 + p.z), 1);
        }
    }
}
