using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

    public Transform firePoint;
    public GameObject bulletPrefab;

    public SpriteRenderer sr;
    public Sprite weaponSprite;

    public int weaponDamage;
    public ParticleSystem hitParticles;
    //public int weaponLevel = 1;

    void Start()
    {
        bulletPrefab.GetComponent<Projectile>().damage = weaponDamage;

        sr = gameObject.GetComponent<SpriteRenderer>();
        weaponSprite = sr.sprite;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
