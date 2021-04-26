using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Fire rate in shots per second.")]
    protected float fireRate = 1.0f;

    private float lastFireTime;

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

    protected virtual void FireWeapon()
    {

    }
}
