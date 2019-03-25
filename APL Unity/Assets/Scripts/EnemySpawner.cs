using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public static bool existence = true;
    public float spawnRate = 1f;

    public Enemy enemyToSpawn;

    void Awake()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        float velMag = 10f;
        // Just to not have them all on top of one another.
        float randomXVel = Random.Range(-velMag, velMag);
        float randomYVel = Random.Range(-velMag, velMag);

        Enemy curEnemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        curEnemy.GetComponent<Rigidbody2D>().velocity = new Vector2(randomXVel, randomYVel);
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(Spawning());
    }
    private void OnDestroy()
    {
        //existence = false;
        Destroy(gameObject);
    }
}
