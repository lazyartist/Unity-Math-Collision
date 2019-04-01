using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 삼각형 클래스 정의 --- 시작
[System.Serializable]
public class Triangle
{
    public Transform[] Vertices; // 꼭짓점
    public Vector3[] Segments; // 선분
    public Vector3 N; // 법선 벡터
    public Color[] SegmentColors = new Color[3]; // 각 선분의 색상, 충돌한 선분은 다른 색으로 칠해준다.

    private float _d; // 평면의 방정식의 d

    // 삼각형을 초기화한다
    public void Init()
    {
        if (Segments == null || Segments.Length < Vertices.Length) Segments = new Vector3[Vertices.Length];

        Segments[0] = Vertices[1].position - Vertices[0].position; // 0->1로 향하는 벡터
        Segments[1] = Vertices[2].position - Vertices[1].position; // 1->2로 향하는 벡터
        Segments[2] = Vertices[0].position - Vertices[2].position; // 2->0로 향하는 벡터

        N = Vector3.Cross(Segments[0], Segments[1]).normalized; // 평면의 법선(노멀) 벡터는 자주사용하므로 미리 계산

        _d = -(Vector3.Dot(N, Vertices[0].position)); // 평면의 방정식의 d값은 자주 사용하므로 미리 계산

        for (int i = 0; i < SegmentColors.Length; i++)
        {
            SegmentColors[i] = Color.black; // 충돌하지 않은 선분 기본 색상 초기화
        }
    }

    // 현재 삼각형을 평면으로 하여 매개변수로 전달된 선분과 충돌했는지 테스트한다.
    // 선분에서 충돌한 지점을 선분 길이 대비 비율로 반환한다.
    // 0~1 사이 : 충돌 지점이 선분 안이다. 충돌했다.
    // 0~1 바깥 : 충돌 지점이 선분 밖이다. 충돌 안했다.
    public float HitTestWithSegment(Vector3 position, Vector3 vector)
    {
        // 평면과 선분 시작점의 거리
        float distance = N.x * position.x + N.y * position.y + N.z * position.z + _d;
        // 선분의 방향을 나타내는 단위벡터
        Vector3 vn = vector.normalized;
        // 반환할 충돌지점 비율값
        float t = -1f;
        // 평면의 법선과 선분의 내적값, 두 벡터 사이각의 cos값과 같다.
        float cos = Vector3.Dot(N, vn);

        // 평면과 시작점의 거리가 양수이면 시작점은 평면의 법선벡터 방향(평면의 위)에 있고,
        // 음수이면 평면의 법선벡터 반대 방향(평면 아래)에 있다.
        if (distance > 0 && cos > 0) return t; // 평면 위에 있고 평면을 향하지 않는다. 충돌 가능성 없음.
        if (distance < 0 && cos < 0) return t; // 평면 아래에 있고 평면을 향하지 않는다. 충돌 가능성 없음.

        distance = Mathf.Abs(distance); // 거리에서 방향성을 제거하기 위해 부호를 제거한다.
        cos = Mathf.Abs(cos); // 예각을 구하기 위해 절대값으로 변경(cos값에서 부호를 제거하면 언제나 0~90사이를 가리키는 예각이 된다.)

        // 두 벡터의 내각, 즉 cos값이 0이라는 것은 두 벡터가 90도로 직교한다는 것이고 이는 
        // 평면의 법선과 선분이 90도라는 것이다.
        // 즉, 평면과 선분은 평행하므로 충돌 가능성이 없다.
        if (Mathf.Approximately(cos, 0f)) return t;

        // 시작점에서 평면에 내린 수선(평면에 직교하는 선 = 인접변)의 길이와 
        // 두 벡터의 cos값으로 빗변의 길이를 알아낸다.
        float hypotenuse = distance / cos;
        // 빗변의 길이와 선분의 길이의 비율을 계산한다.
        t = hypotenuse / vector.magnitude;

        return t;
    }

    // 선분을 모두 그린다.
    public void DrawSegments()
    {
        for (int i = 0; i < Segments.Length; i++)
        {
            Gizmos.color = SegmentColors[i];
            Gizmos.DrawLine(Vertices[i].position, Vertices[(i + 1) % Segments.Length].position);
        }
    }
}
/// 삼각형 클래스 정의 --- 끝

public class Triangle_Triangle_Collision : MonoBehaviour {
    public Triangle t0; // 삼각형1
    public Triangle t1; // 삼각형2

    private Color[] _crossVectorColors = { Color.red, Color.blue, Color.green };

    private void OnDrawGizmos()
    {
        // 삼각형을 초기화한다.
        t0.Init();
        t1.Init();

        // t0을 평면으로 하여 충돌 테스트를 한다.
        bool hit = HitTestTriangle(t0, t1);
        // 충돌하지 않았으면 t1을 평면으로 하여 충돌 테스트를 한 번 더 한다.
        if (!hit) HitTestTriangle(t1, t0);

        // 삼각형의 외곽선을 그린다.
        t0.DrawSegments();
        t1.DrawSegments();
    }

    // 삼각형을 평면과 선분 역할로 구분하여 충돌 테스트를 한다.
    private bool HitTestTriangle(Triangle triangleForPlane, Triangle triangle)
    {
        bool hit = false;

        // 각각의 선분이 삼각형과 걸치는지 확인
        for (int i = 0; i < triangle.Segments.Length; i++)
        {
            Color segmentColor = Color.black;

            // 현재 선분과 삼각형의 평면이 충돌하는지 확인, 선분 길이 대비 충돌지점의 비율을 반환 받는다.
            float t = triangleForPlane.HitTestWithSegment(triangle.Vertices[i].position, triangle.Segments[i]);
            if (t >= 0f && t <= 1f) // 삼각형의 평면과 선분이 충돌했다.
            {
                // 삼각형의 평면과 선분의 히트 포인트(충돌 지점)
                Vector3 hitPoint = triangle.Vertices[i].position + triangle.Segments[i] * t;

                // 히트 포인트가 삼각형 안에 있는지 확인
                Vector3[] crossVectors = new Vector3[triangleForPlane.Segments.Length];
                for (int jj = 0; jj < triangleForPlane.Segments.Length; jj++)
                {
                    // 각 꼭지점에서 히트 포인트까지 벡터를 구하고 인접변과 외적한다.
                    Vector3 toHitPoint = hitPoint - (triangleForPlane.Vertices[jj].position + triangleForPlane.Segments[jj]);
                    crossVectors[jj] = Vector3.Cross(triangleForPlane.Segments[jj], toHitPoint);

                    // 외적 벡터 그리기
                    Gizmos.color = _crossVectorColors[i];
                    Gizmos.DrawLine(triangleForPlane.Vertices[jj].position, triangleForPlane.Vertices[jj].position + crossVectors[jj].normalized * 3f);
                }

                // 외적한 모든 벡터가 같은 방향이면 히트 포인트는 삼각형 안에 있다.
                // 같은 방향의 벡터끼리 내적하면 1~0(0~90도) 양수, 다른 방향의 벡터끼리 내적하면 0~-1(90~180도) 음수
                if (Vector3.Dot(crossVectors[0], crossVectors[1]) > 0 && Vector3.Dot(crossVectors[1], crossVectors[2]) > 0)
                {
                    hit = true;

                    segmentColor = Color.white; // 충돌했으므로 선분색을 변경한다.

                    Gizmos.color = _crossVectorColors[i]; // 충돌지점마다 색을 다르게 한다.
                    Gizmos.DrawWireSphere(hitPoint, 1f); // 충돌지점을 그린다.

                    for (int jj = 0; jj < triangleForPlane.Segments.Length; jj++)
                    {
                        Gizmos.DrawLine(triangleForPlane.Vertices[jj].position, hitPoint); // 꼭짓점과 충돌지점까지 선을 그린다.
                    }
                }
            }

            triangle.SegmentColors[i] = segmentColor;
        }

        return hit;
    }
}
