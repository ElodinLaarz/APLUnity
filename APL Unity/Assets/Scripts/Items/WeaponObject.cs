using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemObject {

    public Transform firePoint;
    public GameObject bulletPrefab;

    public Transform pivotPosition;

    //public Sprite weaponSprite;

    //public int weaponDamage;
    public ParticleSystem hitParticles;

    public float weaponCooldown = 1f;

    private bool shooting = false;
    //public int weaponLevel = 1;

    GameManager gm;
    Inventory inventory;

    void Start()
    {
        gm = GameManager.instance;
        inventory = Inventory.instance;
        UpdateWeapon();
        inventory.onItemChangedCallback += UpdateWeapon;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton("Fire1") && !shooting)
        {
            shooting = true;
            StartCoroutine(Shoot());
        }
    }

    void UpdateWeapon()
    {
        Debug.Log("We are updating the weapon to... " + gm.playerWeapon.name);
        gameObject.GetComponent<SpriteRenderer>().sprite = gm.playerWeapon.icon;
        bulletPrefab.GetComponent<Projectile>().damage = gm.playerWeapon.damage;
        if(gm.playerWeapon.attackSpeed > 0)
        {
            weaponCooldown = 1 / gm.playerWeapon.attackSpeed;
        }
        else
        {
            Debug.LogError("You tried to divide by zero, you naughty boy!");
        }

    }

    IEnumerator Shoot()
    {
        while (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint;

            screenPoint = Camera.main.WorldToScreenPoint(firePoint.position);

            mousePos.x -= screenPoint.x;
            mousePos.y -= screenPoint.y;

            //This always fires at the mouse, but it gets weird when the mouse is over the Sprite.
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            angle += pivotPosition.transform.rotation.z;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0,0,angle));
            yield return new WaitForSeconds(weaponCooldown);
        }
        shooting = false;
    }
}
