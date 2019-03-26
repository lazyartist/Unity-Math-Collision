using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment_Segment_Collision : MonoBehaviour
{
    public Transform P1;
    public Transform P2;
    public Transform Q1;
    public Transform Q2;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(P1.position, P2.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Q1.position, Q2.position);

        Vector3 p1 = P1.position;
        Vector3 p2 = P2.position;
        Vector3 q1 = Q1.position;
        Vector3 q2 = Q2.position;

        Vector3 v = P2.position - P1.position;
        Vector3 w = Q2.position - Q1.position;

        float denominator = (q2.x - q1.x) * (p1.y - p2.y) - (p1.x - p2.x) * (q2.y - q1.y);

        if(denominator == 0)
        {
            Debug.Log("Overlap");
            return;
        }

        // t : P1->P2에서 교점의 비율, s : Q1->Q2에서 교점의 비율
        float t = ((q1.y - q2.y) * (p1.x - q1.x) + (q2.x - q1.x) * (p1.y - q1.y)) / denominator;
        float s = ((p1.y - p2.y) * (p1.x - q1.x) + (p2.x - p1.x) * (p1.y - q1.y)) / denominator;

        float x = v.x * t + p1.x;
        float y = v.y * t + p1.y;

        Debug.Log(t + ", " + s + ", " + denominator);

        if (t < 0.0f || t > 1.0f || s < 0.0f || s > 1.0f)
        {
            Debug.Log("No Collision");
        }
        else if (t == 0 && s == 0)
        {
            Debug.Log("Parallel");
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(x, y, 0), 1);
        }
    }
}
