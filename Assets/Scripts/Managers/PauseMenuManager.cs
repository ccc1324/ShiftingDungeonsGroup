using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public OptionsManager OptionsMenu;

    private CanvasGroup _canvasGroup;
    private Fadable _fadable;

    public void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _fadable = GetComponent<Fadable>();
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

    public void FadeInOptionsMenu()
    {
        _fadable.FadeOut(1);
        OptionsMenu.FadeInOptionsMenu(1, 1, _fadable);
    }

    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n); //0 is the index of main scene
        enabled = false;
        ShowHideMenu();
    }
}
