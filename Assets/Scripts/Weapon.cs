using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Fire rate in shots per second.")]
    protected float fireRate = 1.0f;

    protected SpriteRenderer rend;

    private float lastFireTime;

    protected bool isFlipped;

    protected virtual void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        lastFireTime = Time.unscaledTime;
    }

    public void Fire()
    {
        if (lastFireTime + (1.0f / fireRate) < Time.unscaledTime)
        {
            // We mogen weer aanvallen.
            lastFireTime = Time.unscaledTime;
            FireWeapon();
        }
    }

    public void FlipX(bool flipX)
    {
        isFlipped = flipX;
        rend.flipX = flipX;
        FlipXLocal(flipX);
    }

    protected virtual void FlipXLocal(bool flipX)
    {

    }

    protected virtual void FireWeapon()
    {

    }
}
