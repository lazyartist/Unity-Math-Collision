using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEquation : MonoBehaviour {
    public Transform Plane;
    public Transform SpacePoint;

    // 공간상의 한 점에서 평면에 수직으로 선분 그리기
    private void OnDrawGizmos()
    {
        /*
        법선 벡터 : N(Nx, Ny, Nz)
        평면상의 한 점 : P(Px, Py, Pz)
        평면의 방정식 : Nx*x + Ny*y + Nz*z + d = 0
                      d = -N·P = -(Nx*Px + Ny*Py + Nz*Pz)
         */
        Vector3 n = Plane.up.normalized; // 평면의 법선 벡터
        Vector3 p = Plane.position; // 평면상의 한 점
        Vector3 s = SpacePoint.position; // 공간상의 한 점

        // d값은 변하지 않기 때문에 미리 계산하는게 좋다.
        float d = -(n.x * p.x + n.y * p.y + n.z * p.z);
        // 공간 상의 점과 평면의 거리
        float distance = n.x * s.x + n.y * s.y + n.z * s.z + d;
        // 공간 상의 점과 평면을 연결하는 가장 짧은 벡터
        Vector3 shortestVector = -n * distance;

        Debug.Log(distance);

        if (distance > 0)
        {
            Gizmos.color = Color.red;
        } else if (distance < 0)
        {
            Gizmos.color = Color.blue;
        } else
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawWireSphere(SpacePoint.position, SpacePoint.localScale.x);
        Gizmos.DrawLine(SpacePoint.position, SpacePoint.position + shortestVector);

        //Gizmos.
    }
}
