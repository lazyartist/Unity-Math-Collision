using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Triangle_Collision : MonoBehaviour
{
    public Transform T1;
    public Transform T2;
    public Transform T3;
    public Transform SP;

    private void OnDrawGizmos()
    {
        // 삼각형 선분 벡터(triangle vector)
        Vector3 tv12 = T2.position - T1.position;
        Vector3 tv23 = T3.position - T2.position;
        Vector3 tv31 = T1.position - T3.position;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(T1.position, T2.position);
        Gizmos.DrawLine(T2.position, T3.position);
        Gizmos.DrawLine(T3.position, T1.position);

        Vector3 tn = Vector3.Cross(tv12, tv23).normalized; // 삼각형의 법선 벡터(triangle normal)
        Vector3 pp = T1.position; // 평면 위의 한 점(plane point)
        Vector3 sp = SP.position; // 구의 중심(sphere point)
        float r = SP.localScale.x * 0.5f; // 구의 반지름(radius)

        // d값은 변하지 않기 때문에 미리 계산하는게 좋다.
        float d = -(tn.x * pp.x + tn.y * pp.y + tn.z * pp.z); // 평면의 방정식에서 d값
        // 구의 중심과 평면의 거리
        float distance = tn.x * sp.x + tn.y * sp.y + tn.z * sp.z + d; // 평면의 방정식의 근(평면과 점의 거리)

        // 평면의 방정식의 근이 0보다 작을 경우 공간상의 점은 평면 법선 벡터가 있는 쪽의 반대쪽에 있다.
        if (distance < 0)
        {
            tn *= -1; // 선분의 시작점이 법선 벡터 반대쪽에 있으므로 법선 벡터를 뒤집어 준다.
            distance *= -1; // 거리에서 방향 정보를 제거한다.
        }

        // 1. 구와 삼각형이 있는 평면과 충돌 검사 //
        {
            if (distance > r)
            {
                return;
            }
        }

        // 2. 구와 삼각형 각 변의 충돌 검사 //
        {
            bool isCollided = false;

            Vector3[] tps = { T1.position, T2.position, T3.position };
            Vector3[] tvs = { tv12, tv23, tv31 };
            for (int i = 0; i < 3; i++)
            {
                Vector3 tp = tps[i];
                Vector3 tv = tvs[i];
                Vector3 k = new Vector3(tp.x - sp.x, tp.y - sp.y, tp.z - sp.z);

                // ax^2 + 2bx + c = 0
                float a = Mathf.Pow(tv.x, 2) + Mathf.Pow(tv.y, 2) + Mathf.Pow(tv.z, 2);
                float b = k.x * tv.x + k.y * tv.y + k.z * tv.z;
                float c = Mathf.Pow(k.x, 2) + Mathf.Pow(k.y, 2) + Mathf.Pow(k.z, 2) - Mathf.Pow(r, 2);

                // 판별식(discriminant)
                float dis = Mathf.Pow(b, 2) - (a * c);
                if (dis >= 0) // 근이 있다.
                {
                    isCollided = true;

                    // 근의 공식
                    float t1 = (-b + Mathf.Sqrt(dis)) / a;
                    float t2 = (-b - Mathf.Sqrt(dis)) / a;

                    Debug.Log(t1 + ", " + t2);

                    if (t1 >= 0f && t1 <= 1f)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(new Vector3(tv.x * t1 + tp.x, tv.y * t1 + tp.y, tv.z * t1 + tp.z), 0.2f);
                    }

                    if (t2 >= 0f && t2 <= 1f)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireSphere(new Vector3(tv.x * t2 + tp.x, tv.y * t2 + tp.y, tv.z * t2 + tp.z), 0.2f);
                    }
                }

                if (isCollided) break;
            }

            if (isCollided)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(sp, r * 1.2f);
                return;
            }
        }

        // 3. 구가 삼각형 안에 들어갔는지 검사 //
        {
            // 구의 중심에서 삼각형에 수직으로 내린 직선 벡터(vertical vector)
            Vector3 vv = tn * distance;

            Vector3 ip = SP.position - vv; // 충돌 지점(intersection point)
            Vector3 n1 = Vector3.Cross(tv31, T1.position - ip).normalized;
            Vector3 n2 = Vector3.Cross(tv12, T2.position - ip).normalized;
            Vector3 n3 = Vector3.Cross(tv23, T3.position - ip).normalized;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(T1.position, T1.position + n1);
            Gizmos.DrawLine(T2.position, T2.position + n2);
            Gizmos.DrawLine(T3.position, T3.position + n3);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(T1.position, ip);
            Gizmos.DrawLine(T2.position, ip);
            Gizmos.DrawLine(T3.position, ip);

            // 내적이 1이면 같은 방향
            Debug.Log(Vector3.Dot(n1, n2) + ", " + Vector3.Dot(n2, n3));
            if (Vector3.Dot(n1, n2) >= 0.0f && Vector3.Dot(n2, n3) >= 0.0f)
            {
                Gizmos.color = Color.cyan; // 구가 삼각형 안에 있음
            }
            else
            {
                Gizmos.color = Color.red; // 구가 삼각형 안에 없음
            }

            Gizmos.DrawLine(SP.position, SP.position + vv);
            Gizmos.DrawWireSphere(SP.position, r*1.2f);
        }
    }
}
