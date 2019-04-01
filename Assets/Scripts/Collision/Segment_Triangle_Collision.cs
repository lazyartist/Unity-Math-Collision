using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment_Triangle_Collision : MonoBehaviour
{
    public Transform T1;
    public Transform T2;
    public Transform T3;
    public Transform SP1;
    public Transform SP2;

    private void OnDrawGizmos()
    {
        /*
        법선 벡터 : N(Nx, Ny, Nz)
        평면상의 한 점 : P(Px, Py, Pz)
        평면의 방정식 : Nx*x + Ny*y + Nz*z + d = 0
                      d = -N·Q = -(Nx*Px + Ny*Py + Nz*Pz)
        */
        // 삼각형 선분 벡터
        Vector3 tv12 = T2.position - T1.position;
        Vector3 tv23 = T3.position - T2.position;
        Vector3 tv31 = T1.position - T3.position;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(T1.position, T2.position);
        Gizmos.DrawLine(T2.position, T3.position);
        Gizmos.DrawLine(T3.position, T1.position);

        Vector3 tn = Vector3.Cross(tv12, tv23); // 삼각형의 법선 벡터(triangle normal)
        Vector3 tp = T1.position; // 삼각형 위의 한 점(triangle point)
        Vector3 sp = SP1.position; // 선분의 시작점(start or segment point)

        // d값은 변하지 않기 때문에 미리 계산하는게 좋다.
        float d = -(tn.x * tp.x + tn.y * tp.y + tn.z * tp.z);
        // 선분의 시작점과 평면의 거리
        float distance = tn.x * sp.x + tn.y * sp.y + tn.z * sp.z + d; // 평면의 방정식의 근(평면과 점의 거리)

        // 평면의 방정식의 근이 0보다 작을 경우 공간상의 점은 평면 법선 벡터가 있는 쪽의 반대쪽에 있다.
        if (distance < 0)
        {
            tn *= -1; // 선분의 시작점이 법선 벡터 반대쪽에 있으므로 법선 벡터를 뒤집어 준다.
            distance *= -1; // 거리에서 방향 정보를 제거한다.
        }

        // 선분 벡터(segment vector)
        Vector3 sv = SP2.position - SP1.position;
        // 평면의 법선 벡터와 선분의 각도
        float cos = Vector3.Dot(-tn, sv.normalized);

        // 선분과 평면의 각도가 0~90 사이일 경우만 계산한다.
        // 90~180 사이에서는 선분의 방향으로 절대 만나지 않는다.
        if (cos > 0)
        {
            // 선분의 시작에서 평면까지의 거리
            float distanceFromStartToPlane = distance / cos;
            // 선분의 시작에서 평면까지의 벡터(intersection point vector)
            Vector3 ipv = sv.normalized * distanceFromStartToPlane;

            // 제곱근 연산은 부하가 크기 때문에 길이의 제곱으로 비교한다.
            if (Mathf.Pow(distanceFromStartToPlane, 2) <= sv.sqrMagnitude)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(SP1.position + ipv, 1);

                // 각 변과 충돌 지점에서 각 꼭지점으로 이은 선을 외적하여 나온 벡터들을 
                // 서로 내적하여 모두 같은 값이면 삼각형 안에 있다고 판단
                Vector3 ip = SP1.position + ipv; // 충돌 지점(intersection point)
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
                if (Vector3.Dot(n1, n2) == 1f && Vector3.Dot(n2, n3) == 1f)
                {
                    Gizmos.color = Color.cyan; // 선분과 삼각형이 충돌함
                }
                else
                {
                    Gizmos.color = Color.red; // 선분과 삼각형이 충돌하지 않음
                }
            }
            else
            {
                Gizmos.color = Color.red; // 선분과 삼각형이 충돌하지 않음
            }
        }
        else
        {
            Gizmos.color = Color.red; // 선분과 삼각형이 충돌하지 않음
        }

        Gizmos.DrawLine(SP1.position, SP2.position);
    }
}
