using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject enemyProjectile;

    public float enemyProjSpeed = 0.1f;

    public bool playerIsClose = false;

    public float attackRate = 2f;
    public float attackCountdown = 1f;

    GameManager gmInstance;

    private float angle = 0;

    private void Start()
    {
        enemyProjectile.GetComponent<Projectile>().speed = enemyProjSpeed;
        gmInstance = GameManager.instance;
    }
    void Update()
    {
        //Keeping track of when the enemy can attack
        attackCountdown -= Time.deltaTime;
        // Doesn't continue if the player is too far to attack, the player doesn't exist, or it has no projectiles
        // If the creature has a melee attack, we might have to update this (since the projectile might be null)
        if (!playerIsClose|| gmInstance.player == null || enemyProjectile == null || attackCountdown > 0f)
            return;
        //The direction and rotation we want to look in
        Vector2 dir = new Vector2(gmInstance.player.transform.position.x - transform.position.x, gmInstance.player.transform.position.y - transform.position.y);

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        AttackPlayer();
        attackCountdown = 1f / attackRate;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerIsClose = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerIsClose = false;
    }

    void AttackPlayer()
    {
        Instantiate(enemyProjectile, firePoint.transform.position, firePoint.transform.rotation);
    }
}
