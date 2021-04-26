using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speargun : Weapon
{
    [SerializeField]
    private Spear spearPrefab;

    private Spear SpearPrefab { get { return spearPrefab; } }

    protected override void FireWeapon()
    {
        Spear spear = Instantiate<Spear>(SpearPrefab, transform.position, Quaternion.identity);
        Vector2 spearDirection = new Vector2 { x = (isFlipped ? -1.0f : 1.0f), y = 0 };
        spear.GetComponent<Rigidbody2D>().AddForce(spearDirection.normalized * 500, ForceMode2D.Force);
        spear.GetComponent<SpriteRenderer>().flipX = !isFlipped;
        Destroy(spear.gameObject, 3.0f);
    }
}
