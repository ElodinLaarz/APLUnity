using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 20f;
    //public Rigidbody2D rb;

    public ParticleSystem enemyHitParticles;

    public int damage = 10;

    public static Projectile CreateComponent(GameObject where, int d)
    {
        Projectile myC = where.AddComponent<Projectile>();
        myC.damage = d;
        return myC;
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        if (hitInfo.collider.tag == "Enemy")
        {
            hitEnemy(hitInfo.collider.GetComponent<Enemy>());
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
}
