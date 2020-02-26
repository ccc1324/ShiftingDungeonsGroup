using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/* Manages player health, death, and spawning/respawning
 * Player movement, combat, and stun effects are handled seperately
 */

public class Player : MonoBehaviour
{
    public int MaxHealth;
    public GameObject CameraCutscene;

    public AudioClip DeathSFX;
    public float DeathSFXVolume;
    public AudioClip SpawnMusic;
    public float SpawnMusicVolume;

    private int _health;
    private HealthbarSegmented _healthbar;
    private DungeonManager _dungeon_manager;
    private Camera _camera;
    private CanvasGroup _inventory;
    private CanvasGroup _equipment;
    private AudioSource _audio_source;
    private BlackOverlay _fade_effect;
    
    private Animator _animator;
    private PlayerCombat _player_combat;
    private PlayerMovement _player_movement;
    private PlayerInventory _player_inventory;

    void Start()
    {
        _healthbar = FindObjectOfType<HealthbarSegmented>();
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _camera = FindObjectOfType<Camera>();
        _inventory = FindObjectOfType<Inventory>().GetComponent<CanvasGroup>();
        _equipment = FindObjectOfType<Equipment>().GetComponent<CanvasGroup>();
        _fade_effect = FindObjectOfType<BlackOverlay>();

        _player_combat = GetComponent<PlayerCombat>();
        _player_movement = GetComponent<PlayerMovement>();
        _player_inventory = GetComponent<PlayerInventory>();
        _animator = GetComponent<Animator>();
        _audio_source = GetComponent<AudioSource>();
        _health = MaxHealth;
    }

    public void OnHit(int damage)
    {
        if (_health <= 0)
            return;

        _health -= damage;
        _healthbar.DecreaseHealth(damage);

        if (_health <= 0)
        {
            _audio_source.PlayOneShot(DeathSFX);
            _audio_source.volume = DeathSFXVolume * OptionsManager.GetSoundVolume();
            StartCoroutine(Death());
        }
    }

    //moves player to original start position and "resets" player
    public void ResetPlayer(float time)
    {
        StartCoroutine(Reset(time));
    }

    //fade screen, reset dungeonmanager, move camera, hide inventory/equipment, hide player, unfade, spawn
    IEnumerator Death()
    {
        if (_player_inventory != null)
            _player_inventory.Stunned = true;

        _animator.SetBool("Dead", true);
        if (_fade_effect != null)
            _fade_effect.FadeInFull(4);
        StartCoroutine(SlowTime(4));
        yield return new WaitForSeconds(4f);

        transform.position = new Vector2(0, -50);
        if (_dungeon_manager != null)
            _dungeon_manager.DungeonState = "Respawn";
        _camera.transform.position = new Vector3(transform.position.x, 14.5f, -10);
        if (_inventory != null && _inventory.alpha == 1)
        {
            _inventory.alpha = 0;
            _inventory.blocksRaycasts = false;
            _inventory.interactable = false;
        }
        if (_equipment != null && _equipment.alpha == 1)
        {
            _equipment.alpha = 0;
            _equipment.blocksRaycasts = false;
            _equipment.interactable = false;
        }

        if (_fade_effect != null)
            _fade_effect.FadeOutFull(2);
        yield return new WaitForSeconds(2f);

        StartCoroutine(Reset(1f));
    }

    //move player, restore health, change player animation, lock player, freefall, camera follow, unlock player
    IEnumerator Reset(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool("Grounded", false);
        _player_movement.Grounded = false;
        _animator.SetTrigger("Spawn");
        transform.position = new Vector2(0, 20);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0.2f;

        _animator.ResetTrigger("Stun"); //fixes a bug where player gets stunned twice
        _animator.SetBool("Dead", false);
        _health = MaxHealth;

        yield return new WaitForSeconds(0.5f);

        CameraCutscene.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(0.5f);

        CameraCutscene.SetActive(false);
        GetComponent<Rigidbody2D>().gravityScale = 1f;
        if (_player_inventory != null)
            _player_inventory.Stunned = false;
        _healthbar.SetHealth(MaxHealth);
        _camera.GetComponent<CameraMovement>().CameraState = "Follow";
    }

    IEnumerator SlowTime(float time)
    {
        float startTime = Time.time;
        Time.timeScale = 0.5f;

        while (Time.time < startTime + time)
        {
            Time.timeScale = Mathf.Lerp(0.5f, 1, (Time.time - startTime) / (time));
            yield return null;
        }

        Time.timeScale = 1;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room" || collision.tag == "Platform")
        {
            _animator.SetBool("Grounded", true);
        }
    }
}
