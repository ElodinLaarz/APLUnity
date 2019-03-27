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
    private const float flashTime = totalFlashes * flashSpeed;


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

        //normalColor = sr.color;
    }

    private void Update()
    {
        if (player != null && isInvincible)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
        }
        else if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
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

            float ricochetMag = 10;
            Vector2 ricochetDir = new Vector2(e.transform.position.x - player.transform.position.x, e.transform.position.y - player.transform.position.y);
            ricochetDir.Normalize();
            player.GetComponent<Rigidbody2D>().velocity = -ricochetMag * ricochetDir;
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
        float flashingFor = 0;
        Color newColor = flashColor;
        while (flashingFor < flashTime)
        {
            toFlash.color = newColor;
            flashingFor += Time.deltaTime;
            yield return new WaitForSeconds(flashSpeed);
            flashingFor += flashSpeed;
            if (newColor == flashColor)
            {
                newColor = normalColor;
            }
            else
            {
                newColor = flashColor;
            }
        }
        toFlash.color = normalColor;
        isInvincible = false;
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
}