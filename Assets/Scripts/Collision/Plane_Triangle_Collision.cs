using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Triangle_Collision : MonoBehaviour
{
    public Transform T0;
    public Transform T1;
    public Transform T2;
    public Transform P; // Plane

    private void OnDrawGizmos()
    {
        Vector3 p = P.position;
        Vector3 t0 = T0.position;
        Vector3 t1 = T1.position;
        Vector3 t2 = T2.position;

        // 평면의 법선
        Vector3 n = P.up;

        // 평면의 방정식의 d값
        float d = -1 * Vector3.Dot(n, p);

        float distance0 = n.x * t0.x + n.y * t0.y + n.z * t0.z + d;
        float distance1 = n.x * t1.x + n.y * t1.y + n.z * t1.z + d;
        float distance2 = n.x * t2.x + n.y * t2.y + n.z * t2.z + d;

        Debug.Log(distance0 + ", " + distance1 + ", " + distance2);

        Color color = Color.white;

        // 모두 같은 부호이면 같은 면에 있다. - 충돌 안함
        // 하나라도 부호가 다르면 다른 면에 있다. - 충돌함.
        if (distance0 * distance1 <= 0 || distance1 * distance2 <= 0)
        {
            // 충돌한 지점 찾기
            Vector3[] ts = { t0, t1, t2 }; // 꼭지점 위치
            Vector3[] tvs = { t1 - t0, t2 - t1, t0 - t2 }; // 변
            float[] distances = { distance0, distance1, distance2 }; // 꼭지점과 평면의 거리

            for (int i = 0; i < tvs.Length; i++)
            {
                Vector3 tv = tvs[i];
                Vector3 tvn = tv.normalized;
                float distance = distances[i];

                if (distance > 0 && Vector3.Dot(n, tvn) > 0) continue; // 변이 평면 위에 있고 평면을 향하고 있지 않다.
                if (distance < 0 && Vector3.Dot(n, tvn) < 0) continue; // 변이 평면 아래에 있고 평면을 향하고 있지 않다.

                distance = Mathf.Abs(distance); // 평면 위, 아래 판단 후 절대값으로 만들어 방향 정보를 제거한다.
                float cos = Mathf.Abs(Vector3.Dot(tvn, n)); // cos의 절대값은 예각이다.
                float l = distance / cos; // 꼭지점에서 변의 방향으로 평면에 닿을 때까지의 거리

                if (l >= 0f && Mathf.Pow(l, 2) <= tv.sqrMagnitude) // 이 변이 평면과 충돌
                {
                    Vector3 v = tvn * l; // 꼭지점에서 평면에 닿을 때까지의 벡터
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(ts[i] + v, 0.5f);
                }
            }

            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(T0.position, T1.position);
        Gizmos.DrawLine(T1.position, T2.position);
        Gizmos.DrawLine(T2.position, T0.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(P.position, P.position + P.up * 5f);
    }
}
