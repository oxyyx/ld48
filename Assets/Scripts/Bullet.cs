using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int playerDamage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter character = collision.gameObject.GetComponent<PlayerCharacter>();
            character.Health -= playerDamage;
        }
    }
}
