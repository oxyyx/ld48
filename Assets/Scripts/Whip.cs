using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapon
{
    private Animator anim;
    private PolygonCollider2D col;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        col = GetComponent<PolygonCollider2D>();
    }

    protected override void FireWeapon()
    {
        if (anim != null)
        {
            anim.Play("Whip_Attack");
        }
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }

    protected override void FlipXLocal(bool flipX)
    {
        if (flipX)
        {
            transform.localPosition = new Vector2 { x = -1, y = 0 };
        } else
        {
            transform.localPosition = new Vector2 { x = 1, y = 0 };
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
