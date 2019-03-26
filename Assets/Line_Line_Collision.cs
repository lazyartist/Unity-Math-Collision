using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Line_Collision : MonoBehaviour {
    public Transform LineStart1;
    public Transform LineEnd1;
    public Transform LineStart2;
    public Transform LineEnd2;

private void OnDrawGizmos()
{
    /*
        이 방법으로는 수직인 직선의 기울기가 0 또는 무한대가 되므로 교점을 찾을 수 없다.
    */

    // 직선의 방정식 : y = ax + b, y = cx + d
    // x` = (d - b) / (a - c)
    // y` = ax` + b
    float a = (LineEnd1.position.y - LineStart1.position.y) / (LineEnd1.position.x - LineStart1.position.x);
    float c = (LineEnd2.position.y - LineStart2.position.y) / (LineEnd2.position.x - LineStart2.position.x);
    float b = LineEnd1.position.y - a * LineEnd1.position.x;
    float d = LineEnd2.position.y - c * LineEnd2.position.x;

    if(a == c && b == d)
    {
        Debug.Log("Overlap");
        return;
    } else if (a == c)
    {
        Debug.Log("Parallel");
        return;
    } else
    {
        Debug.Log("Collision");
    }

    float x = (d - b) / (a - c);
    float y = a * x + b;

    Gizmos.color = Color.red;
    Gizmos.DrawLine(LineStart1.position, LineEnd1.position);
    Gizmos.color = Color.blue;
    Gizmos.DrawLine(LineStart2.position, LineEnd2.position);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(new Vector3(x, y, 0), 1);

    Debug.Log(x + ", " + y);
}
}
