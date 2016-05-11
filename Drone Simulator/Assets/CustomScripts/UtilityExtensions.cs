using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class UtilityExtensions
{

    public static Vector3 infiniteVector3 = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

    public static Vector3 Sign(this Vector3 value)
    {
        return new Vector3(Mathf.Sign(value.x), Mathf.Sign(value.y), Mathf.Sign(value.z));
    }

    public static Vector3 Abs(this Vector3 value)
    {
        return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        float result = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        return Mathf.Clamp(result, Mathf.Min(from2, to2), Mathf.Max(to2, from2));
    }

    public static int Remap(this int value, int from1, int to1, int from2, int to2)
    {
        float result = ((float)value - from1) / (to1 - from1) * (to2 - from2) + from2;
        return (int)Mathf.Clamp(result, Mathf.Min(from2, to2), Mathf.Max(to2, from2));
    }


    public static bool IsWithinLimits(this float value, float min, float max)
    {
        return (value >= min) && (value <= max);
    }

    public static void DestroyAllChildren(this Transform target)
    {
        int childCount = target.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(target.GetChild(i).gameObject);
        }
    }

    /// <summary>compares two floats and returns true of their difference is less than floatDiff</summary>
    public static bool AlmostEquals(this float target, float second, float floatDiff)
    {
        return Mathf.Abs(target - second) < floatDiff;
    }

    public static GameObject[] FindGameObjectsWithTag(this Transform transform, string tag, bool includeInactive = false)
    {

        List<GameObject> results = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf && !includeInactive) continue;
            if (child.CompareTag(tag)) results.Add(child.gameObject);
        }

        return results.ToArray();
    }

    public static GameObject FindGameObjectByName(this Transform transform, string name, bool includeInactive = false)
    {

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf && !includeInactive) continue;
            if (child.name == name) return child.gameObject;
        }

        return null;
    }

    public static GameObject[] FindGameObjectsByName(this Transform transform, string name, bool includeInactive = false)
    {

        List<GameObject> results = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf && !includeInactive) continue;
            if (child.name == name) results.Add(child.gameObject);
        }

        return results.ToArray();
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 dir = point - pivot;
        return rotation * dir + pivot;
    }


    #region VertexColor Utilities

    public static Color RemapToColor(this float current, float fromF, float toF, Color fromC, Color toC)
    {
        float amt = current.Remap(fromF, toF, 0, 1);
        return Color.Lerp(fromC, toC, amt);
    }


    public static void ChangeVertexColor(this GameObject go, Color newColor)
    {
        MeshFilter mF = go.GetComponentInChildren<MeshFilter>();
        if (mF == null) return;
        Mesh mesh = mF.mesh;

        int vertCount = mesh.vertexCount;
        Color32[] colors = new Color32[vertCount];
        int i = 0;
        if (vertCount == 0) return;
        while (i < vertCount)
        {
            colors[i] = newColor;
            i++;
        }
        mesh.colors32 = colors;
    }

    public static Color GetVertexColor(this GameObject go)
    {
        MeshFilter mF = go.GetComponentInChildren<MeshFilter>();
        if (mF == null) return Color.clear;
        Color[] colors = mF.mesh.colors;
        if (colors.Length == 0)
        {
            Debug.Log("no vertex color: " + go.name);
            return Color.clear;
        }
        else return colors[0];
    }

    #endregion


    #region Color Utilities

    public static Color ConvertHexColor(float r, float g, float b, float a = 255)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

    public static Vector3 ConvertRGBtoHSV(this Color color)
    {
        Vector3 hsv = new Vector3();
        float min, max, delta;
        min = Mathf.Min(color.r, color.g, color.b);
        max = Mathf.Max(color.r, color.g, color.b);
        hsv.z = max;               // v
        delta = max - min;

        if (max != 0) hsv.y = delta / max;       // s
        else
        {
            // r = g = b = 0		// s = 0, v is undefined
            hsv.y = 0;
            hsv.x = -1;
            return new Vector3(hsv.x, hsv.y, hsv.z);
        }
        if (color.r == max)
            hsv.x = (color.g - color.b) / delta;       // between yellow & magenta
        else if (color.g == max)
            hsv.x = 2 + (color.b - color.r) / delta;   // between cyan & yellow
        else
            hsv.x = 4 + (color.r - color.g) / delta;   // between magenta & cyan

        hsv.x *= 60;               // degrees
        if (hsv.x < 0) hsv.x += 360;

        return new Vector3(hsv.x, hsv.y, hsv.z);
    }

    public static Color ConvertHSVtoRGB(float h, float s, float v)
    {
        int i;
        float f, p, q, t;
        if (s == 0)
        {
            // achromatic (grey)
            return new Color(v, v, v);
        }
        h /= 60;            // sector 0 to 5
        i = Mathf.FloorToInt(h);
        f = h - i;          // factorial part of h
        p = v * (1 - s);
        q = v * (1 - s * f);
        t = v * (1 - s * (1 - f));
        Color rgb = Color.black;
        switch (i)
        {
            case 0:
                rgb.r = v;
                rgb.g = t;
                rgb.b = p;
                break;
            case 1:
                rgb.r = q;
                rgb.g = v;
                rgb.b = p;
                break;
            case 2:
                rgb.r = p;
                rgb.g = v;
                rgb.b = t;
                break;
            case 3:
                rgb.r = p;
                rgb.g = q;
                rgb.b = v;
                break;
            case 4:
                rgb.r = t;
                rgb.g = p;
                rgb.b = v;
                break;
            default:        // case 5:
                rgb.r = v;
                rgb.g = p;
                rgb.b = q;
                break;
        }

        return rgb;

        //if (S == 0f)
        //    return new Color(V, V, V);
        ////else if (V == 0f)
        ////	return new Color.black;
        //else
        //{
        //    Color col = Color.black;
        //    float Hval = H * 6f;
        //    int sel = Mathf.FloorToInt(Hval);
        //    float mod = Hval - sel;
        //    float v1 = V * (1f - S);
        //    float v2 = V * (1f - S * mod);
        //    float v3 = V * (1f - S * (1f - mod));
        //    switch (sel + 1)
        //    {
        //        case 0:
        //            col.r = V;
        //            col.g = v1;
        //            col.b = v2;
        //            break;
        //        case 1:
        //            col.r = V;
        //            col.g = v3;
        //            col.b = v1;
        //            break;
        //        case 2:
        //            col.r = v2;
        //            col.g = V;
        //            col.b = v1;
        //            break;
        //        case 3:
        //            col.r = v1;
        //            col.g = V;
        //            col.b = v3;
        //            break;
        //        case 4:
        //            col.r = v1;
        //            col.g = v2;
        //            col.b = V;
        //            break;
        //        case 5:
        //            col.r = v3;
        //            col.g = v1;
        //            col.b = V;
        //            break;
        //        case 6:
        //            col.r = V;
        //            col.g = v1;
        //            col.b = v2;
        //            break;
        //        case 7:
        //            col.r = V;
        //            col.g = v3;
        //            col.b = v1;
        //            break;
        //    }
        //    col.r = Mathf.Clamp(col.r, 0f, 1f);
        //    col.g = Mathf.Clamp(col.g, 0f, 1f);
        //    col.b = Mathf.Clamp(col.b, 0f, 1f);
        //    return col;
        //}
    }

    public static Color GetDifferentColor(this Color original)
    {
        Vector3 newColor_hsv = new Vector3(Random.Range(0, 360f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        Vector3 oldColor_hsv = original.ConvertRGBtoHSV();
        //Debug.Log("original color hsv: " + oldColor_hsv + " old color: " + original);
        float nextStepColor = 60f;
        newColor_hsv.x = (Mathf.Abs(newColor_hsv.x - oldColor_hsv.x) > nextStepColor) ? newColor_hsv.x : Mathf.Repeat(newColor_hsv.x + nextStepColor, 360);
        //		Debug.Log("new color hsv: " + newColor_hsv);
        return ConvertHSVtoRGB(newColor_hsv.x, newColor_hsv.y, newColor_hsv.z);
    }


    #endregion

    public static GameObject RetrieveRandomPref(GameObject[] prefPool)
    {
        int randPrefNdx = UnityEngine.Random.Range(0, prefPool.Length);
        return prefPool[randPrefNdx];

    }

    public static Color GetOppositeColor(this Color target)
    {
        return new Color(1 - target.r, 1 - target.g, 1 - target.b);
    }

    public static Vector2 ClampBoundsWithin2DPlane(Vector2 targetCenter, Vector2 targetDimensions, Vector2 planeCenter, Vector2 planeDimensions)
    {
        Vector2 targetExtents = targetDimensions * 0.5f;
        Vector2 planeExtents = planeDimensions * 0.5f;
        Vector2 clampedPos = Vector2.zero;
        Vector2 posLimits = planeExtents - targetExtents - new Vector2(0.1f, 0.1f);
        clampedPos.x = Mathf.Clamp(targetCenter.x, planeCenter.x - posLimits.x, planeCenter.x + posLimits.x);
        clampedPos.y = Mathf.Clamp(targetCenter.y, planeCenter.y - posLimits.y, planeCenter.y + posLimits.y);
        return clampedPos;
    }

    public static Vector3 ClampBoundsWithinBox(Vector3 targetCenter, Vector3 targetDimensions, Vector3 boxCenter, Vector3 boxDimensions)
    {
        Vector3 targetExtents = targetDimensions * 0.5f;
        Vector3 planeExtents = boxDimensions * 0.5f;
        Vector3 clampedPos = Vector2.zero;
        Vector3 posLimits = planeExtents - targetExtents - new Vector3(0.1f, 0.1f, 0.1f);
        clampedPos.x = Mathf.Clamp(targetCenter.x, boxCenter.x - posLimits.x, boxCenter.x + posLimits.x);
        clampedPos.y = Mathf.Clamp(targetCenter.y, boxCenter.y - posLimits.y, boxCenter.y + posLimits.y);
        clampedPos.z = Mathf.Clamp(targetCenter.z, boxCenter.z - posLimits.z, boxCenter.z + posLimits.z);
        return clampedPos;
    }

    public static bool CheckObjectInBox(Vector3 targetCenter, Vector3 targetDimensions, Vector3 boxCenter, Vector3 boxDimensions)
    {
        Vector3 targetExtents = targetDimensions * 0.5f;
        Vector3 boxExtents = boxDimensions * 0.5f;
        if ((targetCenter.x + targetExtents.x) > (boxCenter.x + boxExtents.x)) return false;
        if ((targetCenter.x - targetExtents.x) < (boxCenter.x - boxExtents.x)) return false;
        if ((targetCenter.y + targetExtents.y) > (boxCenter.y + boxExtents.y)) return false;
        if ((targetCenter.y - targetExtents.y) < (boxCenter.y - boxExtents.y)) return false;
        if ((targetCenter.z + targetExtents.z) > (boxCenter.z + boxExtents.z)) return false;
        if ((targetCenter.z - targetExtents.z) < (boxCenter.z - boxExtents.z)) return false;

        return true;
    }

    public static bool CheckValueInLimits(float value, float min, float max)
    {
        return value > min && value < max;
    }

    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < (start + time))
        {
            yield return null;
        }
    }

    public static void Shuffle<T>(ref T[] source)
    {
        for (int i = 0; i < source.Length; ++i)
        {
            int r = Random.Range(0, i);
            T tmp = source[i];
            source[i] = source[r];
            source[r] = tmp;
        }
    }

    public static void ResetEnable(this MonoBehaviour target, bool enable)
    {
        target.enabled = !enable;
        target.enabled = enable;
    }

    public static void ResetActive(this GameObject target, bool active)
    {
        target.SetActive(!active);
        target.SetActive(active);
    }

    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }

}