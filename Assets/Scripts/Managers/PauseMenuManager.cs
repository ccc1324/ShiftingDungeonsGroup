using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] Slider _music_slider;
    [SerializeField] Slider _sound_slider;

    public void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if(_music_slider && _sound_slider)
        {
            _music_slider.value = PlayerPrefsController.GetMusicVolume();
            _sound_slider.value = PlayerPrefsController.GetSoundVolume();
        }
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ShowHideMenu();

        
    }

    public void ShowHideMenu()
    {
        _canvasGroup.alpha = _canvasGroup.alpha == 0 ? 1 : 0;
        _canvasGroup.interactable = _canvasGroup.interactable ? false : true;
        _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts ? false: true;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0); //0 is the index of main scene
        enabled = false;
        ShowHideMenu();
    }

    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n); //0 is the index of main scene
        enabled = false;
        ShowHideMenu();
    }

    public void SaveMusicVolume()
    {
        PlayerPrefsController.SetMusicVolume(_music_slider.value);
        var _musicManager = FindObjectOfType<MusicManager>();
        if (_musicManager)
        {
            _musicManager.GetComponent<AudioSource>().volume = _music_slider.value;
        }
    }

    public void SaveSoundVolume()
    {
        PlayerPrefsController.SetSoundVolume(_sound_slider.value);
    }
}
