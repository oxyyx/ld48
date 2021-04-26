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

    private GameObject activeTarget;
    private GameObject[] possibleTargets;

    // Start is called before the first frame update
    void Start()
    {
        possibleTargets = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTarget != null)
        {
            // Check if target is still in range.
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
        }
    }

    private void FindNewTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject go in possibleTargets)
        {
            float targetDistance = Vector3.Distance(go.transform.position, transform.position);

            if (targetDistance < closestDistance && targetDistance <= maximumFollowDistance)
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

        transform.position = (Vector2) transform.position + (movementMultiplier * movementDirection);


        var rayDirection = activeTarget.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection);

        if (hit && hit.collider.gameObject == activeTarget) {
            Debug.Log(String.Format("CLICK CLACK, TARGET FINNA GET SHOT"));
        }
    }
}
