using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Sphere_Collision : MonoBehaviour {
    public Transform S1;
    public Transform S2;

    private void OnDrawGizmos()
    {
        Vector3 sp1 = S1.position;
        Vector3 sp2 = S2.position;

        float r1 = S1.localScale.x * 0.5f;
        float r2 = S2.localScale.x * 0.5f;

        Vector3 v = sp2 - sp1;
        if (v.sqrMagnitude < Mathf.Pow(r1 + r2, 2))
        {
            Gizmos.color = Color.blue;

            // 침범한 만큼 밀어내기
            // 침범한 거리(invasion distance)
            float idst = (r1 + r2) - v.magnitude;
            S1.position += -v.normalized * idst;
        } else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(sp1, sp2);
    }
}
