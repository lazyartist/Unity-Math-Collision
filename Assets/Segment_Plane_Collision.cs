using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment_Plane_Collision : MonoBehaviour
{
    public Transform Plane;
    public Transform SegmentStart;
    public Transform SegmentEnd;

    private void OnDrawGizmos()
    {
        /*
        법선 벡터 : N(Nx, Ny, Nz)
        평면상의 한 점 : P(Px, Py, Pz)
        평면의 방정식 : Nx*x + Ny*y + Nz*z + d = 0
                      d = -N·Q = -(Nx*Px + Ny*Py + Nz*Pz)
        */
        Vector3 n = Plane.up.normalized; // 평면의 법선 벡터
        Vector3 p = Plane.position; // 평면상의 한 점
        Vector3 s = SegmentStart.position; // 선분의 시작점

        // d값은 변하지 않기 때문에 미리 계산하는게 좋다.
        float d = -(n.x * p.x + n.y * p.y + n.z * p.z);
        // 선분의 시작점과 평면의 거리
        float distance = n.x * s.x + n.y * s.y + n.z * s.z + d; // 평면의 방정식의 근

        // 평면의 방정식의 근이 0보다 작을 경우 공간상의 점은 평면 법선 벡터가 있는 쪽의 반대쪽에 있다.
        if(distance < 0)
        {
            n *= -1; // 선분의 시작점이 법선 벡터 반대쪽에 있으므로 법선 벡터를 뒤집어 준다.
            distance *= -1; // 거리에서 방향 정보를 제거한다.
        }

        // 공간 상의 점과 평면을 연결하는 가장 짧은 벡터
        Vector3 shortestVector = n * distance;
        // 선분 벡터
        Vector3 segment = SegmentEnd.position - SegmentStart.position;
        // 평면의 법선 벡터와 선분의 각도
        float cos = Vector3.Dot(-n, segment.normalized);

        // 선분과 평면의 각도가 0~90 사이일 경우만 계산한다.
        // 90~180 사이에서는 선분의 방향으로 절대 만나지 않는다.
        if (cos > 0)
        {
            // 선분의 시작에서 평면까지의 거리
            float distanceFromStartToPlane = distance / cos;
            // 선분의 시작에서 평면까지의 벡터
            Vector3 toPlane = segment.normalized * distanceFromStartToPlane;

            Debug.Log(distanceFromStartToPlane + ", " + segment.magnitude);

            // 제곱근 연산은 부하가 크기 때문에 길이의 제곱으로 비교한다.
            //if (distanceFromStartToPlane <= segment.magnitude)
            if (Mathf.Pow(distanceFromStartToPlane, 2) <= segment.sqrMagnitude)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(SegmentStart.position + toPlane, 1);

                Gizmos.color = Color.cyan; // 선분과 평면이 충돌함
            }
            else
            {
                Gizmos.color = Color.red; // 선분과 평면이 충돌하지 않음
            }
        } else
        {
            Gizmos.color = Color.red; // 선분과 평면이 충돌하지 않음
        }
        
        Gizmos.DrawLine(SegmentStart.position, SegmentEnd.position);
    }
}
