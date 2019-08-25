using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public bool EnableWeaponHitbox;
    public int WeaponDamage;
    public bool SetToStun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && EnableWeaponHitbox)
        {
            collision.GetComponent<IEnemy>().OnHit(WeaponDamage, SetToStun);
        }
    }
}
