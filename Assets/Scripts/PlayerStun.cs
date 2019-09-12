using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStun : MonoBehaviour
{
    public float StunDuration;
    public bool Stunned;
    public bool ParalyzeHeal;
    public Image StunEffect;

    void Update()
    {
        if (Stunned)
        {
            if (!ParalyzeHeal)
            {
                ParalyzeHeal = true;
                StartCoroutine(OnStun(StunDuration));
            }
        }
    }

    IEnumerator OnStun(float time)
    {
        float startTime = Time.time;
        Color color = StunEffect.color;
        color.a = 0.5f;
        StunEffect.color = color;

        while (Time.time < startTime + time)
        {
            color.a = Mathf.Lerp(0.2f, 0, (Time.time - startTime) / (time + 0.5f));
            StunEffect.color = color;
            yield return null;
        }


        color.a = 0;
        StunEffect.color = color;
        ParalyzeHeal = false;
    }

    public void DisablePlayerCombat(float time)
    {
        StartCoroutine(DisableCombat(time));
    }

    IEnumerator DisableCombat(float time)
    {
        GetComponent<PlayerCombat>().enabled = false;
        yield return new WaitForSeconds(time);
        GetComponent<PlayerCombat>().enabled = true;
    }
}
