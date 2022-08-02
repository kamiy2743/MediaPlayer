using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public static class RectTransformExt
    {
        public static void SetPivot(this RectTransform rt, Vector2 pivot)
        {
            var centerPos = (Vector2)rt.localPosition - rt.CalcDelta();
            rt.pivot = pivot;
            rt.localPosition = centerPos + rt.CalcDelta(pivot);
        }

        public static Vector2 CalcDelta(this RectTransform rt)
        {
            return rt.CalcDelta(rt.pivot);
        }

        public static Vector2 CalcDelta(this RectTransform rt, Vector2 pivot)
        {
            return new Vector2(rt.sizeDelta.x * (pivot.x - 0.5f), rt.sizeDelta.y * (pivot.y - 0.5f));
        }
    }
}
