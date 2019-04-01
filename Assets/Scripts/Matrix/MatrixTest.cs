using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    public float BoxSize; // Box Size
    public Transform Position;
    public Vector3 Scale = new Vector3(1f, 1f, 1f);
    [Range(0, 360)]
    public float RotationX = 0f;
    [Range(0, 360)]
    public float RotationY = 0f;
    [Range(0, 360)]
    public float RotationZ = 0f;


    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        float hSize = BoxSize / 2;
        Vector3[] boxVertecis = new Vector3[]
        {
            new Vector3(pos.x - hSize, pos.y- hSize, pos.z- hSize),
            new Vector3(pos.x + hSize, pos.y- hSize, pos.z- hSize),
            new Vector3(pos.x + hSize, pos.y + hSize, pos.z- hSize),
            new Vector3(pos.x- hSize, pos.y + hSize, pos.z- hSize),

            new Vector3(pos.x- hSize, pos.y- hSize, pos.z + hSize),
            new Vector3(pos.x + hSize, pos.y- hSize, pos.z + hSize),
            new Vector3(pos.x + hSize, pos.y+ hSize, pos.z + hSize),
            new Vector3(pos.x- hSize, pos.y + hSize, pos.z + hSize),
        };

        Matrix4 sm = Matrix4.CreateScale(Scale.x, Scale.y, Scale.z);
        Matrix4 rm = Matrix4.CreateRotation(RotationX * Mathf.Deg2Rad, RotationY * Mathf.Deg2Rad, RotationZ * Mathf.Deg2Rad);
        Matrix4 tm = Matrix4.CreateTranslation(Position.position.x, Position.position.y, Position.position.z);

        Matrix4 tsrm = tm * rm * sm;  // 변환 행렬의 적용 순서 반대로 곱해준다.
        Matrix4 tsrim = tsrm.Inverse();  // 역행렬


        // multifly matrix
        for (int i = 0; i < boxVertecis.Length; i++)
        {
            boxVertecis[i] = tsrm * boxVertecis[i];
        }

        // draw vertecis
        Color[] colors = new Color[] { Color.white, Color.yellow, Color.blue, Color.cyan };
        for (int i = 0; i < boxVertecis.Length; i++)
        {
            Color color = colors[(uint)i / 2];
            Gizmos.color = color;
            Gizmos.DrawWireSphere(boxVertecis[i], 0.1f);
        }

        // multifly matrix
        for (int i = 0; i < boxVertecis.Length; i++)
        {
            // 역행렬 곱해서 원래 좌표계로 돌림
            boxVertecis[i] = tsrim * boxVertecis[i];
        }

        // draw vertecis
        for (int i = 0; i < boxVertecis.Length; i++)
        {
            Color color = colors[(uint)i / 2];
            Gizmos.color = color;
            Gizmos.DrawWireSphere(boxVertecis[i], 0.1f);
        }
    }
}
