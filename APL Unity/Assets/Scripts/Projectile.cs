﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 1f;

    public ParticleSystem enemyHitParticles;

    public int damage = 10;
    public float projectileLifeTime = 20f;

    // This flash is implemented via animation vs discrete as is the case with the player. I might
    // The player/enemies to flash via animation at some point, but I think I like the discrete flashes.
    // This is more like a glow
    private const float flashSpeed = 0.5f;
       
    // I want to use this to make generic particles... maybe I won't use it... we'll see.
    public static Projectile CreateComponent(GameObject where, int d)
    {
        Projectile myC = where.AddComponent<Projectile>();
        myC.damage = d;
        return myC;
    }

    // Use this for initialization
    void Start()
    {
        //float angle = TrajectoryAngle();
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        StartCoroutine(FlashObject());
        Destroy(gameObject, projectileLifeTime);
    }

    //float TrajectoryAngle()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    Vector3 screenPoint;

    //    mousePos.z = Camera.main.transform.position.z;
    //    screenPoint = Camera.main.WorldToScreenPoint(transform.position);

    //    mousePos.x -= screenPoint.x;
    //    mousePos.y -= screenPoint.y;

    //    float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    //    return angle * Mathf.Rad2Deg;
    //}

    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        if (hitInfo.collider.tag == "Enemy")
        {
            hitEnemy(hitInfo.collider.GetComponent<Enemy>());
        }else if(hitInfo.collider.tag == "Player" && GameManager.instance.IsInvincible() != true)
        {
            GameManager.instance.DamagePlayer(gameObject, damage);
        }
        Destroy(gameObject);
    }

    void hitEnemy(Enemy enemy)
    {
        // We keep track of the particle to destroy it after 2 seconds.
        ParticleSystem effectIns = Instantiate(enemyHitParticles, transform.position, transform.rotation);
        Destroy(effectIns.gameObject, 2f);

        Damage(enemy);
    }

    void Damage(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }


    // Using a coroutine to wait whilst the flashing happens.
    IEnumerator FlashObject()
    {
        Animator ani = gameObject.GetComponent<Animator>();
        // This *should* stop when it's destroyed, but be careful~
        while (ani != null)
        {
            if (ani.GetBool("flashOn") == true)
            {
                ani.SetBool("flashOn", false);
            }
            else
            {
                ani.SetBool("flashOn", true);
            }
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
