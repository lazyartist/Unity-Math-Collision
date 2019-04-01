using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment_AABB_Collision : MonoBehaviour
{
    public Transform B0; // Box0
    public Transform B1; // Box1
    public Transform S0; // Segment0
    public Transform S1; // Segment1

    private void OnDrawGizmos()
    {
        Vector3 b0 = B0.position;
        Vector3 b1 = B1.position;
        Vector3 s0 = S0.position;
        Vector3 s1 = S1.position;
        Vector3 sv = s1 - s0;

        // 선분과 박스의 충돌하는 곳의 최대, 최소값.(직선의 매개변수 방정식의 t값)
        // x, y, z축의 충돌을 검사하며 점점 좁혀나간다.
        float t_min = 0f;
        float t_max = 1f;
        bool hit = false;

        // do while 반복문 영역은 반복을 위한 것이 아니라 조건에 안맞았을 시 바로 빠져나오기 위한 영역. 
        // while (false)이기 때문에 단 한번만 실행된다.
        do
        {
            // x
            float tx_min = 0f;
            float tx_max = 1f;
            if (sv.x == 0f)
            {
                // 선분이 x좌표로 이동하지 않는다는 뜻이므로 선분의 위치가 박스영역 안인지만 확인하면 된다.
                // 그리고 0으로 나누지 않기 위해서도 따로 처리한다.
                if (s0.x < b0.x || s0.x > b1.x) break;
            }
            else
            {
                // 선분과 박스 영역의 x값의 매개변수를 구한다.
                float tx0 = (b0.x - s0.x) / sv.x;
                float tx1 = (b1.x - s0.x) / sv.x;
                // x축의 최소값 최대값을 결정
                tx_min = Mathf.Min(tx0, tx1);
                tx_max = Mathf.Max(tx0, tx1);
                // 최소, 최대값이 영역을 벗어났는지 확인
                if (tx_max < 0f || tx_min > 1) break;

                //Gizmos.color = Color.red;
                //Gizmos.DrawWireSphere(s0 + (sv * tx0), 0.3f);
                //Gizmos.DrawWireCube(s0 + (sv * tx1), Vector3.one * 0.55f);
            }
            // t_min이 x축의 최대(tx_max)보다 크다는 것은 다른축과 겹치는 영역이 없다는 것이기 때문에 종료한다.
            // t_max가 x축의 최소(tx_min)보다 작다는 것은 다른축과 겹치는 영역이 없다는 것이기 때문에 종료한다.
            if (t_min > tx_max || t_max < tx_min) break;

            // t_min, t_max 값을 tx_min, tx_max 값으로 갱신한다.
            // 현재 최소값보다 x축의 최소값이 크면 x축의 최소값으로 갱신한다.(겹치는 영역을 좁혀나간다.)
            t_min = Mathf.Max(t_min, tx_min);
            // 현재 최대값보다 x축의 최대값이 크면 x축의 최소값으로 갱신한다.(겹치는 영역을 좁혀나간다.)
            t_max = Mathf.Min(t_max, tx_max);
            // 여기서 t_min, t_max 값은 초기값이기 때문에 tx_min, tx_max 을 바로 넣어도 되지만 일관성을 위해 y, z와 똑같이 처리한다.

            // y
            // x와 똑같이 처리한다.
            float ty_min = 0f;
            float ty_max = 1f;
            if (sv.y == 0f)
            {
                if (s0.y < b0.y || s0.y > b1.y) break;
            }
            else
            {
                float ty0 = (b0.y - s0.y) / sv.y;
                float ty1 = (b1.y - s0.y) / sv.y;
                ty_min = Mathf.Min(ty0, ty1);
                ty_max = Mathf.Max(ty0, ty1);

                if (ty_max < 0f || ty_min > 1) break;

                //Gizmos.color = Color.red;
                //Gizmos.DrawWireSphere(s0 + (sv * ty0), 0.3f);
                //Gizmos.DrawWireCube(s0 + (sv * ty1), Vector3.one * 0.55f);
            }
            if (t_max < ty_min || t_min > ty_max) break;
            t_min = Mathf.Max(t_min, ty_min);
            t_max = Mathf.Min(t_max, ty_max);

            // z
            // x와 똑같이 처리한다.
            float tz_min = 0f;
            float tz_max = 1f;
            if (sv.z == 0f)
            {
                if (s0.z < b0.z || s0.z > b1.z) break;
            }
            else
            {
                float tz0 = (b0.z - s0.z) / sv.z;
                float tz1 = (b1.z - s0.z) / sv.z;
                tz_min = Mathf.Min(tz0, tz1);
                tz_max = Mathf.Max(tz0, tz1);

                if (tz_max < 0f || tz_min > 1) break;

                //Gizmos.color = Color.red;
                //Gizmos.DrawWireSphere(s0 + (sv * tz0), 0.3f);
                //Gizmos.DrawWireCube(s0 + (sv * tz1), Vector3.one * 0.55f);
            }
            if (t_max < tz_min || t_min > tz_max) break;
            t_min = Mathf.Max(t_min, tz_min);
            t_max = Mathf.Min(t_max, tz_max);

            // 최종 t값이 0~1 사이에 있는지 확인
            //hit = !(t_min > 1f || t_max < 0);
            hit = (t_min <= 1f && t_max >= 0);
        } while (false);

        // 선분 그리기
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(s0, s1);
        Gizmos.DrawLine(s0, s0 + Vector3.up);
        Gizmos.DrawLine(s1, s1 + Vector3.up);

        // 충돌 영역을 선분에 겹쳐 그린다.
        if (hit)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(s0 + (sv * t_min), s0 + (sv * t_max));
            Gizmos.DrawLine(s0 + (sv * t_min), s0 + (sv * t_min) + Vector3.up);
            Gizmos.DrawLine(s0 + (sv * t_max), s0 + (sv * t_max) + Vector3.up);
        }

        // 상자 그리기
        // 각 축별로 앞위왼 -> 뒤위오 순으로 그린다.
        // x축
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(b0.x, b1.y, b0.z), new Vector3(b1.x, b1.y, b0.z));
        Gizmos.DrawLine(b0, new Vector3(b1.x, b0.y, b0.z));
        Gizmos.DrawLine(new Vector3(b0.x, b1.y, b1.z), b1);
        Gizmos.DrawLine(new Vector3(b0.x, b0.y, b1.z), new Vector3(b1.x, b0.y, b1.z));

        // y축
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(b0.x, b1.y, b0.z), b0);
        Gizmos.DrawLine(new Vector3(b1.x, b1.y, b0.z), new Vector3(b1.x, b0.y, b0.z));
        Gizmos.DrawLine(new Vector3(b0.x, b1.y, b1.z), new Vector3(b0.x, b0.y, b1.z));
        Gizmos.DrawLine(b1, new Vector3(b1.x, b0.y, b1.z));

        // z축
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(b0.x, b1.y, b0.z), new Vector3(b0.x, b1.y, b1.z));
        Gizmos.DrawLine(b0, new Vector3(b0.x, b0.y, b1.z));
        Gizmos.DrawLine(new Vector3(b1.x, b1.y, b0.z), b1);
        Gizmos.DrawLine(new Vector3(b1.x, b0.y, b0.z), new Vector3(b1.x, b0.y, b1.z));
    }
}
