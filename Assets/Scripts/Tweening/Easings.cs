using System;
using UnityEngine;
/*
    Easing equations source: https://easings.net/

    Quad, Cubic, Quart, Quint easings are combined into polynomial easing which takes power as a parameter.
    eg. quad in is just polynomial in with power of 2.

    Transpiled from a Java code I have for this, removed most of the functions and added a couple
*/
public class Easings
{
    public enum Easing {s, si, so} // Easings
    /*
        s --> Straight / Linear, Defaults to this if not specified.
        si --> Sine In
        so --> Sine Out
    */

    public static float CalculateTimeFraction(float timeFraction, Easing ease) {
        switch(ease) {
            case Easing.s:
                return timeFraction;
            case Easing.si:
                return SineIn(timeFraction);
            case Easing.so:
                return SineOut(timeFraction);
            default:
                return timeFraction;
        }
    }

	public static float SineIn(float x)
	{
		return 1.0f - Mathf.Cos((x * Mathf.PI) / 2);
	}
	public static float SineOut(float x)
	{
		return Mathf.Sin((x * Mathf.PI) / 2);
	}
}