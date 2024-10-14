using UnityEngine;

public class Bezier : System.Object
{
    public Vector3 initPos;
    public Vector3 targetPos;
    public Vector3 handlePos_0;
    public Vector3 handlePos_1;
    public float Lengh
    {
        get
        {
            return GetTriangleBezierLengh(initPos, handlePos_0, handlePos_1, targetPos, 60);
        }
    }

    /// <summary>
    /// 根据二次贝塞尔曲线的公式，计算各个时刻的坐标值
    /// </summary>
    /// <param name="begin">初始点</param>
    /// <param name="handlePos_0">初始点目标过点</param>
    /// <param name="handlePos_1">结束点目标过点</param>
    /// <param name="end">结束点</param>
    /// <param name="t">插值 (0~1) </param>
    /// <returns></returns>
    public static Vector3 GetTriangleBezierByT(Vector3 begin, Vector3 handlePos_0, Vector3 handlePos_1, Vector3 end, float t)
    {
        Vector3 p0 = Vector3.Lerp(begin, handlePos_0, t);
        Vector3 p1 = Vector3.Lerp(handlePos_1, end, t);
        return Vector3.Lerp(p0, p1, t);
    }
    public Bezier(Vector3 begin, Vector3 handle, Vector3 end)
    {
        this.initPos = begin;
        this.targetPos = end;
        this.handlePos_0 = handle;
        this.handlePos_1 = handle;
    }
    public Bezier(Vector3 begin, Vector3 handlePos_0, Vector3 handlePos_1, Vector3 end)
    {
        this.initPos = begin;
        this.targetPos = end;
        this.handlePos_0 = handlePos_0;
        this.handlePos_1 = handlePos_1;
    }
    
    public Vector3 Lerp(float t)
    {
        return GetTriangleBezierByT(initPos, handlePos_0, handlePos_1, targetPos, t);
    }
    public static float GetTriangleBezierLengh(Vector3 begin, Vector3 handlePos_0, Vector3 handlePos_1, Vector3 end, int count = 30)
    {
        float distance = 0;
        for (int i = 1; i < count; i++)
        {
            distance += (GetTriangleBezierByT(begin, handlePos_0, handlePos_1, end, (i - 1) / (float)count) - GetTriangleBezierByT(begin, handlePos_0, handlePos_1, end, (i) / (float)count)).magnitude;
        }
        return distance;
    }
}