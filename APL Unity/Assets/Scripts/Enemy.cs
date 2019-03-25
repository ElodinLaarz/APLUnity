using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int curHP = 100;
    public int expReward = 10;
    
    //I would like to separate the stats from the enemy mechanics at some point...
    //public EnemyStats enemyStats;

    GameManager gmInstance;


    // I probably don't need this, but I am not sure how to access the sprite renderer off the top of my head...
    public SpriteRenderer sr;
    private Color normalColor;

    // flashing color
    public Color fc;

    // Time between flashes, total number, and total time to flash.
    private const float flashSpeed = 0.1f;
    private const int totalFlashes = 4;
    private const float flashTime = totalFlashes * flashSpeed;

    public int damage = 10;

    //public GameObject deathEffect;

    void Start()
    {
        gmInstance = GameManager.instance;
        normalColor = sr.color;
    }


    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashObject(sr, normalColor, fc));
        curHP -= damage;
        if (curHP <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        { 
            gmInstance.DamagePlayer(this, damage);
        }
    }

    // Using a coroutine to wait whilst the flashing happens.
    IEnumerator FlashObject(SpriteRenderer toFlash, Color originalColor, Color flashColor)
    {
        float flashingFor = 0;
        Color newColor = flashColor;
        while (flashingFor < flashTime)
        {
            toFlash.color = newColor;
            flashingFor += Time.deltaTime;
            yield return new WaitForSeconds(flashSpeed);
            flashingFor += flashSpeed;
            if (newColor == flashColor)
            {
                newColor = originalColor;
            }
            else
            {
                newColor = flashColor;
            }
        }
    }


    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        gmInstance.XpReward(expReward);
        Destroy(gameObject);
    }
}