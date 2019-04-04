using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject bulletPrefab = null;
    public int numberOfProjectiles = 3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * 360f / numberOfProjectiles;
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        }
    }
}
