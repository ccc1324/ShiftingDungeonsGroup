using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public bool EnableWeaponHitbox;
    public int WeaponDamage;
    public bool SetToStun;

    private bool _hitbox_enabled;
    private Collider2D _weapon_hitbox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && EnableWeaponHitbox)
        {
            collision.GetComponent<IEnemy>().OnHit(WeaponDamage, SetToStun);
        }
    }

    private void Update()
    {
        if (EnableWeaponHitbox && !_hitbox_enabled)
        {
            _weapon_hitbox = gameObject.AddComponent<PolygonCollider2D>();
            _weapon_hitbox.isTrigger = true;
        }
        else if (!EnableWeaponHitbox && _hitbox_enabled)
        {
            Destroy(_weapon_hitbox);
        }
        _hitbox_enabled = EnableWeaponHitbox;
    }
}
