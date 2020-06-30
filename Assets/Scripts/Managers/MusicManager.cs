using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip SpawnMusic;
    public float SpawnMusicVolume;
    public AudioPair[] DungeonMusic;

    private AudioSource _audio_source;
    private float _unmodified_volume;

    [System.Serializable]
    public struct AudioPair
    {
        public AudioClip StageMusic;
        public float StageVolume;
        public AudioClip BossMusic;
        public float BossVolume;
    }

    void Start()
    {
        _audio_source = GetComponent<AudioSource>();
        _audio_source.volume = SpawnMusicVolume * OptionsManager.GetMusicVolume();
        _unmodified_volume = SpawnMusicVolume;
    }

    public IEnumerator FadeOutMusic(float time)
    {
        float startTime = Time.time;
        float startVolume = _audio_source.volume;

        while (Time.time < startTime + time)
        {
            _audio_source.volume = Mathf.Lerp(startVolume, 0, (Time.time - startTime) / time);
            yield return null;
        }

        _audio_source.volume = 0;
    }

    public IEnumerator FadeInMusic(float time, AudioClip newClip, float newVolume)
    {
        float startTime = Time.time;
        _audio_source.clip = newClip;
        _audio_source.Play();
        _unmodified_volume = newVolume;
        newVolume *= OptionsManager.GetMusicVolume();

        while (Time.time < startTime + time)
        {
            _audio_source.volume = Mathf.Lerp(0, newVolume, (Time.time - startTime) / time);
            yield return null;
        }

        _audio_source.volume = newVolume;
    }

    public void PlayMusic(AudioClip newClip, float newVolume)
    {
        _unmodified_volume = newVolume;
        _audio_source.clip = newClip;
        _audio_source.volume = newVolume * OptionsManager.GetMusicVolume();
        _audio_source.Play();
    }

    public IEnumerator PlayMusicDelayed(float time, AudioClip newClip, float newVolume)
    {
        yield return new WaitForSeconds(time);
        PlayMusic(newClip, newVolume);
    }

    //Used by OptionsManager
    public void UpdateMusicVolume()
    {
        _audio_source.volume = _unmodified_volume * OptionsManager.GetMusicVolume();
    }
}
