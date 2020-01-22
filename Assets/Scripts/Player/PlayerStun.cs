using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerStun : MonoBehaviour
{
    public float StunDuration;
    public bool Stunned;
    [Tooltip("Indicates that the player currently cannot be stunned")]
    public bool ParalyzeHeal;
    public AudioClip StunSFX;
    public float StunSFXVolume;

    private BlackOverlay _stun_effect;

    private void Start()
    {
        _stun_effect = FindObjectOfType<BlackOverlay>();
    }

    void Update()
    {
        if (Stunned)
        {
            if (!ParalyzeHeal)
            {
                ParalyzeHeal = true;
                StartCoroutine(Stun(StunDuration));
            }
        }
    }

    IEnumerator Stun(float time)
    {
        float startTime = Time.time;
        GetComponent<AudioSource>().PlayOneShot(StunSFX);
        GetComponent<AudioSource>().volume = StunSFXVolume;
        if (_stun_effect != null)
            _stun_effect.Stun(StunDuration);
        else
            Debug.Log("No Stun Effect Found");

        yield return new WaitForSeconds(time);

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
