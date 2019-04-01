using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix4
{
    public float
            m00, m01, m02, m03,
            m10, m11, m12, m13,
            m20, m21, m22, m23,
            m30, m31, m32, m33;

    public Matrix4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30 = 0f, float m31 = 0f, float m32 = 0f, float m33 = 1f)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;

        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;

        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;

        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    public static Matrix4 operator /(Matrix4 m, float s)
    {
        return new Matrix4(
            m.m00 / s, m.m01 / s, m.m02 / s, m.m03 / s,
            m.m10 / s, m.m11 / s, m.m12 / s, m.m13 / s,
            m.m20 / s, m.m21 / s, m.m22 / s, m.m23 / s,
            m.m30 / s, m.m31 / s, m.m32 / s, m.m33 / s);
    }

    public static Matrix4 operator *(Matrix4 m, float s)
    {
        return new Matrix4(
            m.m00 * s, m.m01 * s, m.m02 * s, m.m03 * s,
            m.m10 * s, m.m11 * s, m.m12 * s, m.m13 * s,
            m.m20 * s, m.m21 * s, m.m22 * s, m.m23 * s,
            m.m30 * s, m.m31 * s, m.m32 * s, m.m33 * s);
    }

    public static Vector3 operator *(Matrix4 m, Vector3 v)
    {
        return new Vector3(
            m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * 1f,
            m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * 1f,
            m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * 1f);
    }

    public static Matrix4 operator *(Matrix4 m0, Matrix4 m1)
    {
        return new Matrix4(
            m0.m00 * m1.m00 + m0.m01 * m1.m10 + m0.m02 * m1.m20 + m0.m03 * m1.m30,
            m0.m00 * m1.m01 + m0.m01 * m1.m11 + m0.m02 * m1.m21 + m0.m03 * m1.m31,
            m0.m00 * m1.m02 + m0.m01 * m1.m12 + m0.m02 * m1.m22 + m0.m03 * m1.m32,
            m0.m00 * m1.m03 + m0.m01 * m1.m13 + m0.m02 * m1.m23 + m0.m03 * m1.m33,

            m0.m10 * m1.m00 + m0.m11 * m1.m10 + m0.m12 * m1.m20 + m0.m13 * m1.m30,
            m0.m10 * m1.m01 + m0.m11 * m1.m11 + m0.m12 * m1.m21 + m0.m13 * m1.m31,
            m0.m10 * m1.m02 + m0.m11 * m1.m12 + m0.m12 * m1.m22 + m0.m13 * m1.m32,
            m0.m10 * m1.m03 + m0.m11 * m1.m13 + m0.m12 * m1.m23 + m0.m13 * m1.m33,

            m0.m20 * m1.m00 + m0.m21 * m1.m10 + m0.m22 * m1.m20 + m0.m23 * m1.m30,
            m0.m20 * m1.m01 + m0.m21 * m1.m11 + m0.m22 * m1.m21 + m0.m23 * m1.m31,
            m0.m20 * m1.m02 + m0.m21 * m1.m12 + m0.m22 * m1.m22 + m0.m23 * m1.m32,
            m0.m20 * m1.m03 + m0.m21 * m1.m13 + m0.m22 * m1.m23 + m0.m23 * m1.m33,

            m0.m30 * m1.m00 + m0.m31 * m1.m10 + m0.m32 * m1.m20 + m0.m33 * m1.m30,
            m0.m30 * m1.m01 + m0.m31 * m1.m11 + m0.m32 * m1.m21 + m0.m33 * m1.m31,
            m0.m30 * m1.m02 + m0.m31 * m1.m12 + m0.m32 * m1.m22 + m0.m33 * m1.m32,
            m0.m30 * m1.m03 + m0.m31 * m1.m13 + m0.m32 * m1.m23 + m0.m33 * m1.m33
            );
    }

    public Matrix4 Inverse()
    {
        //m00, m01, m02, m03,
        //m10, m11, m12, m13,
        //m20, m21, m22, m23,
        //m30, m31, m32, m33)

        //   0 1 2 3
        // 0 a b c d
        // 1 e f g h
        // 2 i j k l
        // 3 m n o p

        float det = 
              (m00 * m11 * m22 * m33) // +afkp
            - (m00 * m11 * m23 * m32) // -aflo
            - (m00 * m12 * m21 * m33) // -agjp
            + (m00 * m12 * m23 * m31) // +agln
            + (m00 * m13 * m21 * m32) // +ahjo
            - (m00 * m13 * m22 * m31) // -ahkn
            - (m01 * m10 * m22 * m33) // -bekp
            + (m01 * m10 * m23 * m32) // +belo
            + (m01 * m12 * m20 * m33) // +bgip
            - (m01 * m12 * m23 * m30) // -bglm
            - (m01 * m13 * m20 * m32) // -bhio
            + (m01 * m13 * m22 * m30) // +bhkm
            + (m02 * m10 * m21 * m33) // +cejp
            - (m02 * m10 * m21 * m31) // -celn
            - (m02 * m11 * m20 * m33) // -cfip
            + (m02 * m11 * m23 * m30) // +cflm
            + (m02 * m13 * m20 * m31) // +chin
            - (m02 * m13 * m21 * m30) // -chjm
            - (m03 * m10 * m21 * m32) // -dejo
            + (m03 * m10 * m22 * m31) // +dekn
            + (m03 * m11 * m20 * m32) // +dfio
            - (m03 * m11 * m22 * m30) // -dfkm
            - (m03 * m12 * m20 * m31) // -dgin
            + (m03 * m12 * m21 * m30) // +dgjm
            ;

        Matrix4 m = new Matrix4(
            m12 * m23 * m31 - m13 * m22 * m31 + m13 * m21 * m32 - m11 * m23 * m32 - m12 * m21 * m33 + m11 * m22 * m33,
            m03 * m22 * m31 - m02 * m23 * m31 - m03 * m21 * m32 + m01 * m23 * m32 + m02 * m21 * m33 - m01 * m22 * m33,
            m02 * m13 * m31 - m03 * m12 * m31 + m03 * m11 * m32 - m01 * m13 * m32 - m02 * m11 * m33 + m01 * m12 * m33,
            m03 * m12 * m21 - m02 * m13 * m21 - m03 * m11 * m22 + m01 * m13 * m22 + m02 * m11 * m23 - m01 * m12 * m23,
            m13 * m22 * m30 - m12 * m23 * m30 - m13 * m20 * m32 + m10 * m23 * m32 + m12 * m20 * m33 - m10 * m22 * m33,
            m02 * m23 * m30 - m03 * m22 * m30 + m03 * m20 * m32 - m00 * m23 * m32 - m02 * m20 * m33 + m00 * m22 * m33,
            m03 * m12 * m30 - m02 * m13 * m30 - m03 * m10 * m32 + m00 * m13 * m32 + m02 * m10 * m33 - m00 * m12 * m33,
            m02 * m13 * m20 - m03 * m12 * m20 + m03 * m10 * m22 - m00 * m13 * m22 - m02 * m10 * m23 + m00 * m12 * m23,
            m11 * m23 * m30 - m13 * m21 * m30 + m13 * m20 * m31 - m10 * m23 * m31 - m11 * m20 * m33 + m10 * m21 * m33,
            m03 * m21 * m30 - m01 * m23 * m30 - m03 * m20 * m31 + m00 * m23 * m31 + m01 * m20 * m33 - m00 * m21 * m33,
            m01 * m13 * m30 - m03 * m11 * m30 + m03 * m10 * m31 - m00 * m13 * m31 - m01 * m10 * m33 + m00 * m11 * m33,
            m03 * m11 * m20 - m01 * m13 * m20 - m03 * m10 * m21 + m00 * m13 * m21 + m01 * m10 * m23 - m00 * m11 * m23,
            m12 * m21 * m30 - m11 * m22 * m30 - m12 * m20 * m31 + m10 * m22 * m31 + m11 * m20 * m32 - m10 * m21 * m32,
            m01 * m22 * m30 - m02 * m21 * m30 + m02 * m20 * m31 - m00 * m22 * m31 - m01 * m20 * m32 + m00 * m21 * m32,
            m02 * m11 * m30 - m01 * m12 * m30 - m02 * m10 * m31 + m00 * m12 * m31 + m01 * m10 * m32 - m00 * m11 * m32,
            m01 * m12 * m20 - m02 * m11 * m20 + m02 * m10 * m21 - m00 * m12 * m21 - m01 * m10 * m22 + m00 * m11 * m22
        );

        return m * (1f / det);
    }

    public static Matrix4 CreateScale(float x, float y, float z)
    {
        return new Matrix4(
        x, 0, 0, 0,
        0, y, 0, 0,
        0, 0, z, 0,
        0, 0, 0, 1);
    }

    public static Matrix4 CreateRotation(float x, float y, float z)
    {
        Matrix4 rx = new Matrix4(
            1, 0, 0, 0,
            0, Mathf.Cos(x), -Mathf.Sin(x), 0,
            0, Mathf.Sin(x), Mathf.Cos(x), 0,
            0, 0, 0, 1);

        Matrix4 ry = new Matrix4(
            Mathf.Cos(y), 0, Mathf.Sin(y), 0,
            0, 1, 0, 0,
            -Mathf.Sin(y), 0, Mathf.Cos(y), 0,
            0, 0, 0, 1);

        Matrix4 rz = new Matrix4(
            Mathf.Cos(z), -Mathf.Sin(z), 0, 0,
            Mathf.Sin(z), Mathf.Cos(z), 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

        return rx * ry * rz;
    }

    public static Matrix4 CreateTranslation(float x, float y, float z)
    {
        return new Matrix4(
        1, 0, 0, x,
        0, 1, 0, y,
        0, 0, 1, z,
        0, 0, 0, 1);
    }
}
