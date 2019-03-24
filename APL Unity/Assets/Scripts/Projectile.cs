using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb;

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
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
