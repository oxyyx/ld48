using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speargun : Weapon
{
    [SerializeField]
    private Bullet bulletPrefab;

    private Bullet BulletPrefab { get { return bulletPrefab; } }

    protected override void FireWeapon()
    {
        Debug.Log("Fire Speargun!");
    }
}
