using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Plane_Collision : MonoBehaviour {
    public Transform P0;
    public Transform P1;

    private void OnDrawGizmos()
    {
        Vector3 p0 = P0.position;
        Vector3 p1 = P1.position;

        // 평면의 법선
        Vector3 n0 = P0.up.normalized;
        Vector3 n1 = P1.up.normalized;

        // 평면의 방정식의 d값
        float d0 = -1 * Vector3.Dot(n0, p0);
        float d1 = -1 * Vector3.Dot(n1, p1);

        // 교선 벡터
        Vector3 v = Vector3.Cross(P0.up, P1.up);
        Debug.Log(v);

        // 시작점
        float x = 0;
        float y = 0;
        float z = 0;
        if (v.z != 0.0f)
        {
            x = (n0.y * d1 - n1.y * d0) / v.z;
            y = (n1.x * d0 - n0.x * d1) / v.z;
            z = 0f;
        }
        else if (v.y != 0.0f)
        {
            x = (n1.z * d0 - n0.z * d1) / v.y;
            y = 0f;
            z = (n1.y * d0 - n0.y * d1) / v.z;
        }
        else if (v.x != 0.0f)
        {
            x = 0f;
            y = (n0.z * d1 - n1.z * d0) / v.x;
            z = (n1.y * d0 - n0.y * d1) / v.x;
        }
        Vector3 sp = new Vector3(x, y, z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(p0, p0 + P0.up);
        Gizmos.DrawLine(p1, p1 + P1.up);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireMesh(P0.gameObject.GetComponent<MeshFilter>().mesh, P0.position, P0.rotation, P0.localScale);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(Vector3.zero, v*100000f);
        Gizmos.DrawLine(Vector3.zero, -v*100000f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(sp, sp + v * 100000f);
        Gizmos.DrawLine(sp, sp + -v * 100000f);
    }
}
