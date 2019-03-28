using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Used to restart scene
using TMPro;
using UnityEngine.UI;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    DialogueManager dm;
    Inventory inventory;

    [Header("Player")]
    public GameObject player;
    public Item playerWeapon;

    private bool isInvincible = false;

    [Header("GUI Text Boxes")]
    public TextMeshProUGUI tmpgHealth;
    public TextMeshProUGUI tmpgXP;
    public TextMeshProUGUI tmpgLVL;
    public TextMeshProUGUI tmpgWeapon;
    public TextMeshProUGUI tmpgHpPot;
    public TextMeshProUGUI tmpgManaPot;

    [Header("GUI Images")]
    public Image weaponImage;
    //public Image hpImage;
    //public Image manaImage;

    private PlayerStats playerStats;

    private SpriteRenderer sr;
    private Color normalColor = new Color(255, 255, 255, 255);

    [Header("Flashing Color")]
    // flashing color
    public Color fc;

    // Time between flashes, total number, and total time to flash.
    private const float flashSpeed = 0.1f;
    private const int totalFlashes = 4;
    
    // Time after flashing that player has control
    public float iFramesLength = 0.5f * flashSpeed * totalFlashes;


    #region Singleton
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one gameManager in scene!");
            return;
        }
        else
        {
            instance = this;
        }

        //At some point we'll want to keep the manager around between levels/scenes
        //DontDestroyOnLoad(gameObject);

    }
    #endregion


    void Start()
    {
        dm = DialogueManager.instance;
        inventory = Inventory.instance;
        inventory.currentWeapon = playerWeapon;
        playerStats = player.GetComponent<PlayerStats>();
        sr = player.GetComponent<SpriteRenderer>();
        RefreshItem(inventory.currentWeapon);
        RefreshStats();

        // Stops ignoring collisions with enemy projectiles whilst invincible
        //Physics2D.IgnoreLayerCollision(16, 14, false);

        //normalColor = sr.color;
    }

    private void Update()
    {
        if (isInvincible)
        {
            Physics2D.IgnoreLayerCollision(16, 8, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(16, 8, false);
        }
        if (Input.GetButtonDown("Continue"))
        {
            dm.DisplayNextSentence();
        }

    }

    public void DamagePlayer(GameObject e, int d)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            // Ignore collisions with enemy projectiles whilst invincible
            // Physics2D.IgnoreLayerCollision(16, 14, true);
            player.GetComponent<PlayerMovement>().enabled = false;

            float ricochetMag = 0f;
            //Need a non-zero vector... the numbers aren't important.
            Vector2 ricochetDir = new Vector2(player.transform.position.x - e.transform.position.x, player.transform.position.y - e.transform.position.y);

            // Bounce off enemies more strongly than projectiles
            if (e.tag == "Enemy")
            {
                ricochetMag = 10f;
                
            }else if( e.tag == "Projectile")
            {
                ricochetMag = 1f;
                ricochetDir = ricochetDir.normalized + e.GetComponent<Rigidbody2D>().velocity.normalized;
            }

            ricochetDir.Normalize();
            player.GetComponent<Rigidbody2D>().velocity = ricochetMag * ricochetDir;

            StartCoroutine(FlashPlayer(sr, fc));
            playerStats.health -= d;
            RefreshStats();
            if (playerStats.health <= 0)
            {
                Destroy(player);
                RestartCurLevel();
            }
        }

    }

    // Using a coroutine to wait whilst the flashing happens.
    IEnumerator FlashPlayer(SpriteRenderer toFlash, Color flashColor)
    {
        float curFlashes = 0;
        Color newColor = flashColor;
        while (curFlashes < totalFlashes)
        {
            toFlash.color = newColor;
            yield return new WaitForSeconds(flashSpeed);
            if (newColor == flashColor)
            {
                newColor = normalColor;
            }
            else
            {
                newColor = flashColor;
            }
            curFlashes += 1;
        }
        toFlash.color = normalColor;

        player.GetComponent<PlayerMovement>().enabled = true;
        yield return new WaitForSeconds(iFramesLength);

        isInvincible = false;
        // Stops ignoring collisions with enemy projectiles whilst invincible
        //Physics2D.IgnoreLayerCollision(16, 14, false);
    }

    public void XpReward(int xp)
    {
        playerStats.curXP += xp;
        if (playerStats.curXP >= playerStats.lvlUpXP)
        {
            playerStats.curXP -= playerStats.lvlUpXP;
            playerStats.lvlUpXP *= 2;
            playerStats.curLVL += 1;
        }
        RefreshStats();
    }

    void RefreshStats()
    {
        //stats
        tmpgHealth.text = playerStats.health.ToString();
        tmpgXP.text = playerStats.curXP.ToString() + "/" + playerStats.lvlUpXP.ToString();
        tmpgLVL.text = playerStats.curLVL.ToString();

        //potions
        Vector2 curPots = inventory.CurPots();
            tmpgHpPot.text = curPots.x.ToString() + "/" + inventory.maxPotions.ToString();
            tmpgManaPot.text = curPots.y.ToString() + "/" + inventory.maxPotions.ToString();
}

    public void RefreshItem(Item weapon)
    {
        tmpgWeapon.text = weapon.name + " -- Damage: " + weapon.damage;
        weaponImage.sprite = weapon.icon;

    }

    public void RestartCurLevel()
    {
        playerStats.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}