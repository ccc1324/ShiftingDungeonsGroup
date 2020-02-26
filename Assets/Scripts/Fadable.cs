using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach to an object with a Canvas Group to allow if to be faded in/out
 */
public class Fadable : MonoBehaviour
{
    private CanvasGroup _canvas_group;

    void Start()
    {
        _canvas_group = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float time, float buffer = 0)
    {
        if (_canvas_group != null)
        {
            StartCoroutine(FadeInCoroutine(time, buffer));
        }
    }

    IEnumerator FadeInCoroutine(float time, float buffer)
    {
        yield return new WaitForSeconds(buffer);

        _canvas_group.interactable = true;
        _canvas_group.blocksRaycasts = true;

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            _canvas_group.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / time);
            yield return null;
        }
        _canvas_group.alpha = 1;
    }

    public void FadeOut(float time, float buffer = 0)
    {
        if (_canvas_group != null)
        {
            StartCoroutine(FadeOutCoroutine(time, buffer));
        }
    }

    IEnumerator FadeOutCoroutine(float time, float buffer)
    {
        yield return new WaitForSeconds(buffer);

        _canvas_group.interactable = false;
        _canvas_group.blocksRaycasts = false;

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            _canvas_group.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / time);
            yield return null;
        }
        _canvas_group.alpha = 0;
    }
}
