using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A black overlay that provides transition and stun effects
 * If a new effect is called while an old effect is still going, effect will be overwritten
 */
public class BlackOverlay : MonoBehaviour
{
    public float StunStartOpacity;
    public float StunEndOpacity;

    private Image _overlay;

    private void Start()
    {
        _overlay = GetComponent<Image>();

    }

    public void Stun(float time)
    {
        StopAllCoroutines();
        if (StunStartOpacity > StunEndOpacity)
            StartCoroutine(Fade(time, StunStartOpacity, StunEndOpacity));
        else if (StunEndOpacity >= StunStartOpacity)
            StartCoroutine(Fade(time, StunStartOpacity, StunEndOpacity));
    }

    public void FadeInFull(float time)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(time, 0, 1));
    }

    public void FadeOutFull(float time)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(time, 1, 0));
    }

    IEnumerator Fade(float time, float start_opacity, float end_opacity)
    {
        float startTime = Time.time;

        while (Time.time < startTime + time)
        {
            _overlay.color = new Color(0, 0, 0, Mathf.Lerp(start_opacity, end_opacity, (Time.time - startTime) / time));
            yield return null;
        }

        _overlay.color = Color.clear;
    }
}
