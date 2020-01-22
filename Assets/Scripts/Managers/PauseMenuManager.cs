using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    public void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
