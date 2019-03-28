using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject enemyProjectile;

    public float enemyProjSpeed = 1f;

    private bool playerIsClose = false;
    private bool isAttacking = false;

    public float attackRateMin = 0.5f;
    public float attackRateMax = 1f;

    GameManager gmInstance;

    private void Start()
    {
        enemyProjectile.GetComponent<Projectile>().speed = enemyProjSpeed;
        gmInstance = GameManager.instance;
    }
    void Update()
    {
        // Doesn't continue if the player is too far to attack, the player doesn't exist, or it has no projectiles
        // If the creature has a melee attack, we might have to update this (since the projectile might be null)
        if (!playerIsClose|| gmInstance.player == null || enemyProjectile == null)
            return;
        //The direction and rotation we want to look in
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackPlayer());
        }
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

    IEnumerator AttackPlayer()
    {
        Vector2 dir = new Vector2(gmInstance.player.transform.position.x - transform.position.x, gmInstance.player.transform.position.y - transform.position.y);
        float angle = 0;

        if(dir == Vector2.zero)
        {
            Debug.LogError("Player and Enemy have same location.");
            yield break;
        }
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float waitTime = Random.Range(1 / attackRateMax, 1 / attackRateMin);
        Instantiate(enemyProjectile, firePoint.transform.position, firePoint.transform.rotation);
        yield return new WaitForSeconds(waitTime);
        isAttacking = false;
    }
}
