using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ext {
    
    public static Vector3 SetX(this Vector3 me, float x)
    {
        me.x = x;
        return me;
    }

    public static Vector3 SetY(this Vector3 me, float y)
    {
        me.y = y;
        return me;
    }

    public static Vector3 SetZ(this Vector3 me, float z)
    {
        me.z = z;
        return me;
    }

    public static Vector2 SetX(this Vector2 me, float x)
    {
        me.x = x;
        return me;
    }

    public static Vector2 SetY(this Vector2 me, float y)
    {
        me.y = y;
        return me;
    }

    public static Vector3 V3(this Vector4 v4)
    {
        return new Vector3(v4.x, v4.y, v4.z);
    }

    public static Vector4 V4(this Vector3 v3, float w)
    {
        return new Vector4(v3.x, v3.y, v3.z, w);
    }

    public static Vector2 V2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static Vector3 V3(this Vector2 v2, float z)
    {
        return new Vector3(v2.x, v2.y, z);
    }

    public static void LookAt(this Transform me, Vector3 dir, Vector3 up, Vector3 localForward, Vector3 localUp)
    {
        var look = Quaternion.LookRotation(dir, up);
        var localRot = Quaternion.Inverse(Quaternion.LookRotation(localForward, localUp));
        me.rotation = look * localRot;
    }



}
