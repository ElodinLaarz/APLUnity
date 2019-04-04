using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemObject {

    [Header("Positioning")]
    public Transform pivotPosition;
    public Transform firePoint;

    [Header("Projectiles Specifics")]
    public ParticleSystem hitParticles;
    public GameObject bulletPrefab;

    public GameObject secondaryAttackObj;


    private float primaryCooldown = 0f;
    private float secondaryCooldown = 0f;
    private const float secondaryLifetime = 10f;

    private bool shootingPrimary = false;
    private bool shootingSecondary = false;

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

    void Update () {
        if (Input.GetButton("Fire1") && !shootingPrimary)
        {
            shootingPrimary = true;
            StartCoroutine(PrimaryAttack());
        }
        if (Input.GetButton("Fire2") && !shootingSecondary)
        {
            shootingSecondary = true;
            StartCoroutine(SecondaryAttack());
        }
    }

    void UpdateWeapon()
    {
        item = gm.playerWeapon;
        Debug.Log("We are updating the weapon to... " + item.name);
        gameObject.GetComponent<SpriteRenderer>().sprite = item.icon;
        bulletPrefab.GetComponent<Projectile>().damage = item.damage;

        //Secondary attack stuff...
        secondaryAttackObj.name = "Secondary Projectiles";
        secondaryAttackObj.GetComponent<Explosion>().bulletPrefab = bulletPrefab;
        secondaryAttackObj.GetComponent<Explosion>().numberOfProjectiles = item.numSecondaryProj;
        secondaryCooldown = item.secondaryCooldown;

        if (item.attackSpeed > 0)
        {
            primaryCooldown = 1 / item.attackSpeed;
        }
        else
        {
            Debug.LogError("You tried to divide by zero, you naughty boy!");
        }

    }

    IEnumerator PrimaryAttack()
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
            yield return new WaitForSeconds(primaryCooldown);
        }
        shootingPrimary = false;
    }

    IEnumerator SecondaryAttack()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        worldPoint.z = 0;

        GameObject secondAttack = Instantiate(secondaryAttackObj, worldPoint, Quaternion.identity);
        Destroy(secondAttack, secondaryLifetime);
        yield return new WaitForSeconds(secondaryCooldown);
        shootingSecondary = false;
    }

}
