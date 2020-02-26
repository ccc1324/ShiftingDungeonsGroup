using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/* Manages the start menu portion of the game
 * Loads a previous save or starts a new game
 * Player and managers shoudl already be in the scene
 * Player starts off screen, frozen
 */
public class StartMenuManager : MonoBehaviour
{
    public GameObject Player;
    public CanvasGroup StartMenu;
    public CanvasGroup DifficultyMenu;
    public LoadManager LoadManager;
    public GameObject TutorialObjects;
    public GameObject PostTutorialObjects;
    public DungeonManager DungeonManager;
    public AudioClip ClickSFX;
    public TextMeshProUGUI StoryCanvas;
    public TextMeshProUGUI CreditsCanvas;
    public string[] StoryText;
    public UnityEvent EnablePauseMenuEvent;

    private Player _player;
    private PlayerCombat _player_combat;
    private PlayerMovement _player_movement;
    private PlayerInventory _player_inventory;
    private Animator _player_animator;
    private AudioSource _audio_source;
    private MonsterSpawner _monster_spawner;

    void Start()
    {
        _player = Player.GetComponent<Player>();
        _player_combat = Player.GetComponent<PlayerCombat>();
        _player_movement = Player.GetComponent<PlayerMovement>();
        _player_inventory = Player.GetComponent<PlayerInventory>();
        _player_animator = Player.GetComponent<Animator>();
        _audio_source = GetComponent<AudioSource>();
        _monster_spawner = FindObjectOfType<MonsterSpawner>();

        _player_inventory.Stunned = true;
        _player_animator.SetBool("Dead", true);
        _player.GetComponent<Rigidbody2D>().gravityScale = 0;

        StoryText = new string[]{ //Bug doesn't allow escape characters in inspector
            "Dungeon - noun \nA dark usually underground prison or vault \nMay contain monsters and treasure",
            "All known dungeons were vanquished by the Hero \nand no new dungeons have appeared since"
        };
    }

    public void SelectDifficulty(int difficulty) //int because the editer can't serialize enums for button events
    {
        MonsterSpawner.DifficultyEnum difficultyEnum;
        switch (difficulty)
        {
            case 0: difficultyEnum = MonsterSpawner.DifficultyEnum.Easy;
                break;
            case 1: difficultyEnum = MonsterSpawner.DifficultyEnum.Normal;
                break;
            case 2: difficultyEnum = MonsterSpawner.DifficultyEnum.Hard;
                break;
            default: difficultyEnum = MonsterSpawner.DifficultyEnum.Hard;
                break;
        }

        if (_monster_spawner != null)
            _monster_spawner.Difficulty = difficultyEnum;
        else
            Debug.Log("MonsterSpawner not found (StartMenuManager)");

        StartCoroutine(EnableTutorial(21f)); //21 = length of tutorial transition
        StartCoroutine(FadeOutDifficultyMenu(2f));
        _audio_source.PlayOneShot(ClickSFX);
        StartCoroutine(DisableTutorial()); //Destroys tutorial objects once the game starts
        StartCoroutine(DisplayStory(2, 2)); //Displays story text
        StartCoroutine(EnablePauseMenu(21f));
    }

    public void NewGame()
    {
        _audio_source.PlayOneShot(ClickSFX);
        StartCoroutine(FadeOutStartMenu(2f));
        StartCoroutine(FadeInDifficultyMenu(2f, 2f));
    }

    public void Continue()
    {
        try
        {
            LoadManager.LoadGame();
        }
        catch
        {
            return;
        }
        _player.ResetPlayer(2.5f);
        StartCoroutine(FadeOutStartMenu(2f));
        _audio_source.PlayOneShot(ClickSFX);
        PostTutorialObjects.SetActive(true);
        StartCoroutine(EnablePauseMenu(2f));
    }

    public void Credits()
    {
        StartCoroutine(RollCredits(2));
    }

    IEnumerator RollCredits(float time)
    {
        CanvasGroup canvas = CreditsCanvas.GetComponent<CanvasGroup>();
        StartCoroutine(FadeOutStartMenu(2f));
        yield return new WaitForSeconds(2f);

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            canvas.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / time);
            yield return null;
        }
        canvas.alpha = 1;

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        startTime = Time.time;
        while (Time.time < startTime + time)
        {
            canvas.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / time);
            yield return null;
        }
        canvas.alpha = 0;

        StartMenu.interactable = true;
        StartMenu.blocksRaycasts = true;

        startTime = Time.time;
        while (Time.time < startTime + time)
        {
            StartMenu.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / time);
            yield return null;
        }
        StartMenu.alpha = 1;
    }

    IEnumerator FadeOutStartMenu(float time)
    {
        StartMenu.interactable = false;
        StartMenu.blocksRaycasts = false;

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            StartMenu.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / time);
            yield return null;
        }
        StartMenu.alpha = 0;
    }

    IEnumerator FadeInDifficultyMenu(float buffer, float time)
    {
        yield return new WaitForSeconds(buffer);

        DifficultyMenu.interactable = true;
        DifficultyMenu.blocksRaycasts = true;

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            DifficultyMenu.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / time);
            yield return null;
        }
        DifficultyMenu.alpha = 1;
    }

    IEnumerator FadeOutDifficultyMenu(float time)
    {
        DifficultyMenu.interactable = false;
        DifficultyMenu.blocksRaycasts = false;

        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            DifficultyMenu.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / time);
            yield return null;
        }
        DifficultyMenu.alpha = 0;
    }

    IEnumerator EnableTutorial(float time)
    {
        yield return new WaitForSeconds(time);
        TutorialObjects.SetActive(true);
    }

    //Destroys tutorial objects once the game starts
    IEnumerator DisableTutorial()
    {
        while (true)
        {
            if (DungeonManager.DungeonState == "Stage")
            {
                Destroy(TutorialObjects);
                break;
            }
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator DisplayStory(float delayTime, float time)
    {
        CanvasGroup canvas = StoryCanvas.GetComponent<CanvasGroup>();
        float startTime;
        yield return new WaitForSeconds(delayTime);
        
        foreach (string line in StoryText)
        {
            startTime = Time.time;
            StoryCanvas.text = line;

            while (Time.time < startTime + time)
            {
                canvas.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / time);
                yield return null;
            }

            canvas.alpha = 1;
            yield return new WaitForSeconds(3f);
            startTime = Time.time;

            while (Time.time < startTime + time)
            {
                canvas.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / time);
                yield return null;
            }

            canvas.alpha = 0;
            yield return new WaitForSeconds(0.5f);
        }

        _player.ResetPlayer(1.5f);
    }

    IEnumerator EnablePauseMenu(float time)
    {
        yield return new WaitForSeconds(time);
        if (EnablePauseMenuEvent != null)
            EnablePauseMenuEvent.Invoke();
    }
}
