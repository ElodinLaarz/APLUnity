using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb;

    public ParticleSystem enemyHitParticles;

    public int damage = 10;

	// Use this for initialization
	void Start () {
        rb.velocity = transform.right * speed;
	}

    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        Debug.Log("Here!!");
        Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
        if(enemy != null)
        {
            hitEnemy(enemy);
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
