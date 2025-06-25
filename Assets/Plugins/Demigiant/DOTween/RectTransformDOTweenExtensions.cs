using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public static class RectTransformDOTweenExtensions
{
    public static Tweener DOOffsetMin(this RectTransform target, Vector2 endValue, float duration)
    {
        return DOTween.To(() => target.offsetMin, x => target.offsetMin = x, endValue, duration)
                      .SetTarget(target);
    }

    public static Tweener DOOffsetMax(this RectTransform target, Vector2 endValue, float duration)
    {
        return DOTween.To(() => target.offsetMax, x => target.offsetMax = x, endValue, duration)
                      .SetTarget(target);
    }
}

