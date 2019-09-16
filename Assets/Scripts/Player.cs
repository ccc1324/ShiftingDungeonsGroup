using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Manages player health, death, and spawning
 * Player movement, combat, and stun effects are handled seperately
 */

public class Player : MonoBehaviour
{
    public Image[] HealthBar;
    public Sprite FilledHeart;
    public Sprite EmptyHeart;
    public ParticleSystem ParticleEffects;

    public int MaxHealth;
    public Image FadeEffect;
    public GameObject CameraCutscene;
    private int _health;
    private DungeonManager _dungeon_manager;
    private Camera _camera;
    private CanvasGroup _inventory;
    private CanvasGroup _equipment;
    
    private Animator _animator;
    private PlayerCombat _player_combat;
    private PlayerMovement _player_movement;
    private PlayerInventory _player_inventory;

    void Start()
    {
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _camera = FindObjectOfType<Camera>();
        _inventory = FindObjectOfType<Inventory>().GetComponent<CanvasGroup>();
        _equipment = FindObjectOfType<Equipment>().GetComponent<CanvasGroup>();
        _player_combat = GetComponent<PlayerCombat>();
        _player_movement = GetComponent<PlayerMovement>();
        _player_inventory = GetComponent<PlayerInventory>();
        _animator = GetComponent<Animator>();
        _health = MaxHealth;
    }

    public void OnHit(int damage)
    {
        if (_health <= 0)
            return;

        _health -= damage;
        
        for (int i = _health; i < MaxHealth; i++)
        {
            if (HealthBar[i].color == new Color(0, 0, 0, 0))
                break;
            HealthBar[i].color = new Color(0, 0, 0, 0);
        }

        if (_health <= 0)
        {
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
        _player_inventory.Stunned = true;

        _animator.SetBool("Dead", true);
        StartCoroutine(FadeToBlack(4f));
        yield return new WaitForSeconds(4f);

        transform.position = new Vector2(0, -50);
        _dungeon_manager.DungeonState = "Respawn";
        _camera.transform.position = new Vector3(transform.position.x, 14.5f, -10);
        if (_inventory.alpha == 1)
        {
            _inventory.alpha = 0;
            _inventory.blocksRaycasts = false;
            _inventory.interactable = false;
        }
        if (_equipment.alpha == 1)
        {
            _equipment.alpha = 0;
            _equipment.blocksRaycasts = false;
            _equipment.interactable = false;
        }

        StartCoroutine(FadeToWhite(2f));
        yield return new WaitForSeconds(2f);

        StartCoroutine(Reset(1f));
    }

    //move player, restore health, change player animation, lock player, freefall, camera follow, unlock player
    IEnumerator Reset(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool("Dead", false);
        _health = MaxHealth;

        _animator.SetBool("Grounded", false);
        _animator.SetTrigger("Spawn");
        transform.position = new Vector2(0, 20);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0.2f;

        yield return new WaitForSeconds(0.5f);

        CameraCutscene.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(0.5f);

        CameraCutscene.SetActive(false);
        GetComponent<Rigidbody2D>().gravityScale = 1f;
        _player_inventory.Stunned = false;
        foreach (Image heart in HealthBar)
            heart.color = new Color(1, 1, 1, 1);
        _camera.GetComponent<CameraMovement>().CameraState = "Follow";
    }

    IEnumerator FadeToBlack(float time)
    {
        float startTime = Time.time;
        while(Time.time < startTime + time)
        {
            FadeEffect.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, (Time.time - startTime) / time);
            yield return null;
        }
        FadeEffect.color = Color.black;
    }

    IEnumerator FadeToWhite(float time)
    {
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            FadeEffect.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), (Time.time - startTime) / time);
            yield return null;
        }
        FadeEffect.color = new Color(0, 0, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room" || collision.tag == "Platform")
        {
            _animator.SetBool("Grounded", true);
        }
    }
}
