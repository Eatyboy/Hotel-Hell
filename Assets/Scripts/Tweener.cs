using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Easing
{
    public static float Linear(float t) => t;

    public static float EaseInExpo(float t) =>
        t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
    public static float EaseOutExpo(float t) =>
        t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    public static float SmoothStep(float t) => SmoothStep(t);
}

public class Tweener
{
    public static async UniTask Tween<T>(
        T startValue, 
        T endValue, 
        float duration, 
        Func<T, T, float, T> lerp,
        Action<T> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default)
    {
        easing ??= Easing.Linear;

        if (duration <= 0f)
        {
            applyTween(endValue);
            return;
        }

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            ct.ThrowIfCancellationRequested();

            float t = easing(Mathf.Clamp01(elapsedTime / duration));
            applyTween(lerp(startValue, endValue, t));

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        applyTween(endValue);
    }

    public static UniTask TweenFloat(
        float startValue,
        float endValue,
        float duration,
        Action<float> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default) =>
        Tween(startValue, endValue, duration, Mathf.LerpUnclamped, applyTween, easing);

    public static UniTask TweenVector2(
        Vector2 startValue,
        Vector2 endValue,
        float duration,
        Action<Vector2> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default) =>
        Tween(startValue, endValue, duration, Vector2.LerpUnclamped, applyTween, easing);

    public static UniTask TweenVector3(
        Vector3 startValue,
        Vector3 endValue,
        float duration,
        Action<Vector3> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default) =>
        Tween(startValue, endValue, duration, Vector3.LerpUnclamped, applyTween, easing);

    public static UniTask TweenQuaternion(
        Quaternion startValue,
        Quaternion endValue,
        float duration,
        Action<Quaternion> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default) =>
        Tween(startValue, endValue, duration, Quaternion.LerpUnclamped, applyTween, easing);

    public static UniTask TweenColor(
        Color startValue,
        Color endValue,
        float duration,
        Action<Color> applyTween,
        Func<float, float> easing = null,
        CancellationToken ct = default) =>
        Tween(startValue, endValue, duration, Color.LerpUnclamped, applyTween, easing);

    public static TweenGroup Group(float duration, Func<float, float> easing = null, CancellationToken ct = default)
        => new(duration, easing, ct);
}

public class TweenGroup
{
    private readonly float duration;
    private readonly Func<float, float> easing;
    private readonly CancellationToken ct;
    private readonly List<UniTask> tweens = new();

    public TweenGroup(float duration, Func<float, float> easing, CancellationToken ct)
    {
        this.duration = duration;
        this.easing = easing;
        this.ct = ct;
    }

    public TweenGroup Add<T>(T startValue, T endValue, Func<T, T, float, T> lerp, Action<T> applyTween)
    {
        tweens.Add(Tweener.Tween(startValue, endValue, duration, lerp, applyTween, easing, ct));
        return this;
    }

    public TweenGroup AddFloat(float startValue, float endValue, Action<float> applyTween)
        => Add(startValue, endValue, Mathf.LerpUnclamped, applyTween);

    public TweenGroup AddVector2(Vector2 startValue, Vector2 endValue, Action<Vector2> applyTween)
        => Add(startValue, endValue, Vector2.LerpUnclamped, applyTween);

    public TweenGroup AddVector3(Vector3 startValue, Vector3 endValue, Action<Vector3> applyTween)
        => Add(startValue, endValue, Vector3.LerpUnclamped, applyTween);

    public TweenGroup AddQuaternion(Quaternion startValue, Quaternion endValue, Action<Quaternion> applyTween)
        => Add(startValue, endValue, Quaternion.LerpUnclamped, applyTween);

    public TweenGroup AddColor(Color startValue, Color endValue, Action<Color> applyTween)
        => Add(startValue, endValue, Color.LerpUnclamped, applyTween);

    public UniTask Play() => UniTask.WhenAll(tweens);
}
