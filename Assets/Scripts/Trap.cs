using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float trapResetTime = 1.0f;

    [SerializeField]
    private int trapDamage = 10;

    private bool isTriggered = false;

    private float lastTriggerTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!isTriggered && collision.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter character = collision.gameObject.GetComponent<PlayerCharacter>();
            character.Health -= trapDamage;

            isTriggered = true;
            lastTriggerTime = Time.unscaledTime;
            animator.SetBool("IsTriggered", isTriggered);
        }
    }

    private void Update()
    {
        if (isTriggered && lastTriggerTime + trapResetTime < Time.unscaledTime)
        {
            isTriggered = false;
            animator.SetBool("IsTriggered", isTriggered);
        }
    }
}
