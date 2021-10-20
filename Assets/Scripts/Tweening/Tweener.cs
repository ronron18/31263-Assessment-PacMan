using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour {
    private List<Tween> activeTweens;

    void Awake() {
        activeTweens = new List<Tween>();
    }

    void Update() {
        Tween activeTween;
        for (int i = activeTweens.Count-1; i >=0; i--) 
        {
            activeTween = activeTweens[i];
            if(activeTween.Target != null) {
                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f) {
                    float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                    timeFraction = Easings.CalculateTimeFraction(timeFraction, activeTween.Ease);
                    activeTween.Target.position = Vector3.Lerp(activeTween.StartPos,
                                                              activeTween.EndPos,
                                                               timeFraction);                
                } else {
                    activeTween.Target.position = activeTween.EndPos;
                    activeTweens.RemoveAt(i);
                }
            }
            else {
                activeTweens.RemoveAt(i);
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        return false;
    }

    // NOW WITH MORE P O W E R ! ! !
    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration, Easings.Easing ease) {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration, ease));
            return true;
        }
        return false;
    }

    public bool TweenExists(Transform target) {
        foreach (Tween activeTween in activeTweens) {
            if (activeTween.Target.transform == target)
                return true;
        }
        return false;
    }
}
