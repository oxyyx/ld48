using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{ 
    [SerializeField]
    private float movementSpeed = 1.5f;

    [SerializeField]
    private float maximumFollowDistance = 5.0f;

    [SerializeField]
    private float maximumAttackDistance = 2.0f;

    [SerializeField]
    private float attackDelay = 2.0f;

    [SerializeField]
    private Bullet bulletPrefab;

    private Bullet BulletPrefab { get { return bulletPrefab; } }

    private GameObject activeTarget;
    private GameObject[] possibleTargets;

    private Animator animator;
    private SpriteRenderer renderer;

    private float lastFireTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        possibleTargets = GameObject.FindGameObjectsWithTag("Player");
        lastFireTime = Time.unscaledTime;
    }
    void Update()
    {
        // If target is known, check if it is still in range.
        if (activeTarget != null)
        {
            float targetDistance = Vector3.Distance(activeTarget.transform.position, transform.position);

            if (targetDistance > maximumFollowDistance)
            {
                activeTarget = null;
            }
        }

        if (activeTarget == null)
        {
            FindNewTarget();
        } else
        {
            MoveTowardsTarget();

            float targetDistance = Vector3.Distance(activeTarget.transform.position, transform.position);

            if (targetDistance <= maximumAttackDistance && TargetInSight())
            {
                if (lastFireTime + attackDelay <= Time.unscaledTime)
                {
                    // Last Frame of the attack animation triggers the FireAtTarget procedure which fires a bullet.
                    animator.Play("Slime_Attack");
                    lastFireTime = Time.unscaledTime;
                }
            }
        }
    }

    private void FireAtTarget()
    {
        Bullet bullet = Instantiate<Bullet>(BulletPrefab, transform.position, Quaternion.identity);
        Vector3 bulletDirection = activeTarget.transform.position - transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection.normalized * 500, ForceMode2D.Force);
        Destroy(bullet.gameObject, 3.0f);
    }

    private void FindNewTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject go in possibleTargets)
        {
            float targetDistance = Vector3.Distance(go.transform.position, transform.position);

            if (targetDistance < closestDistance && targetDistance <= maximumFollowDistance && GameObjectInSight(go))
            {
                closestDistance = targetDistance;
                closestTarget = go;
            }
        }

        if (closestTarget != null)
        {
            activeTarget = closestTarget;
        }
    }

    private bool GameObjectInSight(GameObject obj)
    {
        Vector3 rayDirection = obj.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection);

        if (hit && hit.collider.gameObject == obj)
        {
            return true;
        }

        return false;
    }

    private bool TargetInSight()
    {
        return GameObjectInSight(activeTarget);
    }

    private void MoveTowardsTarget()
    {
        if (activeTarget == null)
        {
            return;
        }

        Vector2 targetPosition = activeTarget.transform.position;
        float movementMultiplier = Time.deltaTime * movementSpeed;

        Vector2 movementDirection = new Vector2 {
            x = Vector3.Normalize(targetPosition - (Vector2)transform.position).x,
            y = 0
        };

        if (movementDirection.x > 0 && renderer.flipX == true)
        {
            renderer.flipX = false;
        }
        else if (movementDirection.x < 0 && renderer.flipX == false)
        {
            renderer.flipX = true;
        }

        transform.position = (Vector2) transform.position + (movementMultiplier * movementDirection);
    }
}
