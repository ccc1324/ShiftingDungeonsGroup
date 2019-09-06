using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public SpriteRenderer Weapon;
    public bool Stunned;

    private int _attackNumber; //Used to allow smooth transition between different weapons
    private int _equipNumber; //Used to correctly display current weapon

    private float _time_of_last_attack;
    private float _weapon_cooldown;

    private Animator _animator;
    private PlayerMovement _playerMovement;
    private Equipment _equipment;
    private PolygonCollider2D _weaponHitbox;
    private PlayerWeapon _weapon;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _equipment = FindObjectOfType<Equipment>();
        _weapon = Weapon.GetComponent<PlayerWeapon>();

        _time_of_last_attack = -1;
    }

    void Update()
    {
        if (Stunned)
            return;

        if (Input.GetKeyDown("k") || Input.GetKeyDown("l") && _time_of_last_attack + 0.5f < Time.time)
            _time_of_last_attack = Time.time;

        //Update Weapon
        switch (_equipNumber)
        {
            case 1:
                Weapon.sprite = _equipment.PrimaryWeapon.Item == null ? null : _equipment.PrimaryWeapon.Item.GameObjectSprite;
                break;
            case 2:
                Weapon.sprite = _equipment.SecondaryWeapon.Item == null ? null : _equipment.SecondaryWeapon.Item.GameObjectSprite;
                break;
        }

        //Attack
        if (Input.GetKey("k"))
        {
            if (_equipment.PrimaryWeapon.Item == null)
            {
                _animator.SetBool("Attacking", false);
            }

            else if (_equipment.PrimaryWeapon.Item != null)
            {
                if (_attackNumber == 2)
                {
                    _time_of_last_attack = Time.time;
                    _animator.SetBool("Attacking", false);
                    _animator.SetInteger("Weapon", _equipment.PrimaryWeapon.Item.GetWeaponAnimationKey());
                    Destroy(_weaponHitbox);
                }

                else
                {
                    if (_time_of_last_attack + 0.5f < Time.time)
                    {
                        _playerMovement.Stop();
                        _animator.SetBool("Attacking", true);
                        _animator.SetInteger("Weapon", _equipment.PrimaryWeapon.Item.GetWeaponAnimationKey());
                        _playerMovement.enabled = false;
                        if (Weapon.GetComponent<PolygonCollider2D>() == null && _attackNumber != 0) //attack number check to fix a bug
                        {
                            _weapon.WeaponDamage = _equipment.PrimaryWeapon.Item.WeaponDamage;
                            _weaponHitbox = Weapon.gameObject.AddComponent<PolygonCollider2D>();
                            _weaponHitbox.isTrigger = true;
                        }
                    }
                }
            }

            _attackNumber = 1;
            _equipNumber = 1;
        }

        else if (Input.GetKey("l"))
        {
            if (_equipment.SecondaryWeapon.Item == null)
            {
                _animator.SetBool("Attacking", false);
            }

            else if (_equipment.SecondaryWeapon.Item != null)
            {
                if (_attackNumber == 1)
                {
                    _time_of_last_attack = Time.time;
                    _animator.SetBool("Attacking", false);
                    _animator.SetInteger("Weapon", _equipment.SecondaryWeapon.Item.GetWeaponAnimationKey());
                    Destroy(_weaponHitbox);
                }

                else
                {
                    if (_time_of_last_attack + 0.5f < Time.time)
                    {
                        _playerMovement.Stop();
                        _animator.SetBool("Attacking", true);
                        _animator.SetInteger("Weapon", _equipment.SecondaryWeapon.Item.GetWeaponAnimationKey());
                        _playerMovement.enabled = false;
                        if (Weapon.GetComponent<PolygonCollider2D>() == null && _attackNumber != 0) //attack number check to fix a bug
                        {
                            _weapon.WeaponDamage = _equipment.SecondaryWeapon.Item.WeaponDamage;
                            _weaponHitbox = Weapon.gameObject.AddComponent<PolygonCollider2D>();
                            _weaponHitbox.isTrigger = true;
                        }
                    }
                }
            }

            _attackNumber = 2;
            _equipNumber = 2;
        }

        else
        {
            _animator.SetBool("Attacking", false);
            _animator.SetInteger("Weapon", 0);
            _playerMovement.enabled = true;
            _attackNumber = 0;
            Destroy(_weaponHitbox);
        }
        
    }

    private float GetWeaponCooldown(Item item)
    {
        switch (item.GetWeaponAnimationKey())
        {
            case 1: //Sword
                return 30/60f;
            case 2: //Axe
                return 30/60f;
            case 3: //Dagger
                return 20/60f;
            case 4: //Spear
                return 30/60f;
            case 5: //Staff
                return 30/60f;
            case 6: //Hammer
                return 30/60f;
            default:
                return 0.5f;
        }
    }
}
