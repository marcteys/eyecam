using UnityEngine;
using System.Collections;


public static class ExtensionMethods
{


    public static Vector3 GetScreenPosition(Transform transform, Canvas canvas, Camera cam)
    {
        Vector3 pos;
        float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        float x = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width;
        float y = Camera.main.WorldToScreenPoint(transform.position).y / Screen.height;
        pos = new Vector3(width * x - width / 2, y * height - height / 2);
        return pos;
    }



    public static bool IsBetweenRange(this float thisValue, float value1, float value2)
    {
        return thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
    }

    public static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }


    public static float Map(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    public static int Map(this int from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return Mathf.RoundToInt(to);
    }

    

    public static float Clamp0360(this float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

    public static bool RoughlyEqual(float a, float b)
    {
        float treshold = 0.02f; //how much roughly
        return (Mathf.Abs(a - b) < treshold);
    }

    public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

}
