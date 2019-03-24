using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public int damage = 10;

    //public GameObject deathEffect;

    public void TakeDamage(int damage)
    {

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            playerStats.Damage(damage);
        }
    }


    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
