using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    public Transform Target {get;}
    public Vector3 StartPos {get;}
    public Vector3 EndPos {get;}
    public float StartTime {get;}
    public float Duration {get;}
    public Easings.Easing Ease {get;}

    public Tween(Transform target, Vector3 startPos, Vector3 endPos, float startTime, float duration)
    {
        Target = target;
        StartPos = startPos;
        EndPos = endPos;
        StartTime = startTime;
        Duration = duration;
        Ease = Easings.Easing.s;
    }

    // NEW!!! NOW WITH EASINGS!!!!!
    public Tween(Transform target, Vector3 startPos, Vector3 endPos, float startTime, float duration, Easings.Easing ease)
    {
        Target = target;
        StartPos = startPos;
        EndPos = endPos;
        StartTime = startTime;
        Duration = duration;
        Ease = ease;
    }
}
