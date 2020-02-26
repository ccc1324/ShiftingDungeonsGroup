using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public SpriteRenderer Weapon;
    public bool Stunned;

    private int _attackNumber; //Used to allow smooth transition between different weapons
    private int _equipNumber; //Used to correctly display current weapon

    public float _time_of_last_attack;
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

        //Update Weapon
        switch (_equipNumber)
        {
            case 1:
                Weapon.sprite = _equipment.PrimaryWeapon.Item == null ? null : _equipment.PrimaryWeapon.Item.GameObjectSprite;
                _weapon_cooldown = _equipment.PrimaryWeapon.Item == null ? 0 : GetWeaponCooldown(_equipment.PrimaryWeapon.Item);
                break;
            case 2:
                Weapon.sprite = _equipment.SecondaryWeapon.Item == null ? null : _equipment.SecondaryWeapon.Item.GameObjectSprite;
                _weapon_cooldown = _equipment.SecondaryWeapon.Item == null ? 0 : GetWeaponCooldown(_equipment.SecondaryWeapon.Item);
                break;
        }

        if (_time_of_last_attack + _weapon_cooldown > Time.time)
            return;

        if ((Input.GetKeyDown("k") || Input.GetKeyDown("l")) && _time_of_last_attack + _weapon_cooldown < Time.time)
            _time_of_last_attack = Time.time;

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
                    _time_of_last_attack = -1; //ignores weapon cooldown when transitioning
                    _animator.SetBool("Attacking", false);
                    _animator.SetInteger("Weapon", _equipment.PrimaryWeapon.Item.GetWeaponAnimationKey());
                    //Destroy(_weaponHitbox);
                }

                else
                {
                    _playerMovement.InCombat = true;
                    _animator.SetBool("Attacking", true);
                    _animator.SetInteger("Weapon", _equipment.PrimaryWeapon.Item.GetWeaponAnimationKey());

                    _weapon.WeaponDamage = _equipment.PrimaryWeapon.Item.WeaponDamage;
                    if (Weapon.GetComponent<PolygonCollider2D>() == null && _attackNumber != 0) //attack number check to fix a bug
                    {
                        
                        //_weaponHitbox = Weapon.gameObject.AddComponent<PolygonCollider2D>();
                        //_weaponHitbox.isTrigger = true;
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
                if (_playerMovement.Grounded)
                {
                    _playerMovement.Stop();
                    _playerMovement.InCombat = true;
                }

                if (_attackNumber == 1)
                {
                    _time_of_last_attack = -1; //ignores weapon cooldown when transitioning
                    _animator.SetBool("Attacking", false);
                    _animator.SetInteger("Weapon", _equipment.SecondaryWeapon.Item.GetWeaponAnimationKey());
                   // Destroy(_weaponHitbox);
                }

                else
                {
                    _playerMovement.InCombat = true;
                    _animator.SetBool("Attacking", true);
                    _animator.SetInteger("Weapon", _equipment.SecondaryWeapon.Item.GetWeaponAnimationKey());

                    _weapon.WeaponDamage = _equipment.SecondaryWeapon.Item.WeaponDamage;
                    if (Weapon.GetComponent<PolygonCollider2D>() == null && _attackNumber != 0) //attack number check to fix a bug
                    {
                        
                        //_weaponHitbox = Weapon.gameObject.AddComponent<PolygonCollider2D>();
                        //_weaponHitbox.isTrigger = true;
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
            _playerMovement.InCombat = false;
            _attackNumber = 0;
            //Destroy(_weaponHitbox);
        }
        
    }

    private float GetWeaponCooldown(Item item)
    {
        switch (item.GetWeaponAnimationKey())
        {
            case 1: //Sword
                return 25/60f;
            case 2: //Axe
                return 0f;
            case 3: //Dagger
                return 17/60f;
            case 4: //Spear
                return 22/60f;
            case 5: //Staff
                return 26/60f;
            case 6: //Hammer
                return 0f;
            default:
                return 0f;
        }
    }
}
