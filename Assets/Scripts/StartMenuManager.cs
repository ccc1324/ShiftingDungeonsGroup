using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Manages the start menu portion of the game
 * Loads a previous save or starts a new game
 * Player and managers shoudl already be in the scene
 * Player starts off screen, frozen
 */
public class StartMenuManager : MonoBehaviour
{
    public GameObject Player;
    public CanvasGroup StartMenu;
    public LoadManager LoadManager;

    private Player _player;
    private PlayerCombat _player_combat;
    private PlayerMovement _player_movement;
    private PlayerInventory _player_inventory;
    private Animator _player_animator;

    void Start()
    {

        _player = Player.GetComponent<Player>();
        _player_combat = Player.GetComponent<PlayerCombat>();
        _player_movement = Player.GetComponent<PlayerMovement>();
        _player_inventory = Player.GetComponent<PlayerInventory>();
        _player_animator = Player.GetComponent<Animator>();

        _player_inventory.Stunned = true;
        _player_animator.SetBool("Dead", true);
    }

    public void NewGame()
    {
        _player.ResetPlayer(2.5f);
        StartCoroutine(FadeUI(2f));
    }

    public void Continue()
    {
        LoadManager.LoadGame();
        _player.ResetPlayer(2.5f);
        StartCoroutine(FadeUI(2f));
    }

    IEnumerator FadeUI(float time)
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
}
