using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentPlaneCollision : MonoBehaviour {
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
        Vector3 s = SegmentStart.position; // 공간상의 한 점

        // d값은 변하지 않기 때문에 미리 계산하는게 좋다.
        float d = -(n.x * p.x + n.y * p.y + n.z * p.z);
        // 공간 상의 점과 평면의 거리
        float distance = n.x * s.x + n.y * s.y + n.z * s.z + d;
        // 공간 상의 점과 평면을 연결하는 가장 짧은 벡터
        Vector3 shortestVector = -n * distance;

        // 선분 벡터
        Vector3 segment = SegmentEnd.position - SegmentStart.position;
        // 평면의 법선 벡터와 선분의 각도
        float angle = Vector3.Dot(-n, segment.normalized);
        // 선분의 시작에서 평면까지의 거리
        float distanceFromStartToPlane = distance / angle;
        // 선분의 시작에서 평면까지의 벡터
        Vector3 toPlane = segment.normalized * distanceFromStartToPlane; 

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(SegmentStart.position, SegmentEnd.position);

        if(distanceFromStartToPlane <= segment.magnitude)
        {
            Gizmos.color = Color.cyan; // 선분과 평면이 충돌함
        } else
        {
            Gizmos.color = Color.red; // 선분과 평면이 충돌하지 않음
        }
        Gizmos.DrawLine(SegmentStart.position, SegmentStart.position + toPlane);
    }
}
