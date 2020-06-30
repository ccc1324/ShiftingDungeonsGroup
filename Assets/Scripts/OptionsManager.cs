using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider _music_slider;
    [SerializeField] Slider _sound_slider;
    private Fadable _fadable;
    private Fadable _back_menu;
    private MusicManager _music_manager;

    const string MUSIC_VOLUME_KEY = "music volume";
    const string SOUND_VOLUME_KEY = "sound volume";

    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    private void Start()
    {
        if (_music_slider && _sound_slider)
        {
            _music_slider.value = GetMusicVolume();
            _sound_slider.value = GetSoundVolume();
        }
        _fadable = GetComponent<Fadable>();
        _music_manager = FindObjectOfType<MusicManager>();
    }

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME){
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetSoundVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, volume);
        }
    }

    public static float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
    }


    public void SaveMusicVolume()
    {
        SetMusicVolume(_music_slider.value);
        if (_music_manager)
            _music_manager.UpdateMusicVolume();
    }

    public void SaveSoundVolume()
    {
        SetSoundVolume(_sound_slider.value);
    }

    public void FadeInOptionsMenu(float time, float buffer, Fadable BackMenu)
    {
        _back_menu = BackMenu;
        _fadable.FadeIn(time, buffer);
    }

    public void FadeOutOptionsMenu(float time)
    {
        _fadable.FadeOut(time);
        _back_menu.FadeIn(time, time);
    }
}
