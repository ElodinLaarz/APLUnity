using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Used to restart scene
using TMPro;
using UnityEngine.UI;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

       
    public GameObject player;
    public Weapon playerWeapon;
    public Sprite playerWeaponSprite;

    public TextMeshProUGUI tmpgHealth;
    public TextMeshProUGUI tmpgXP;
    public TextMeshProUGUI tmpgLVL;

    public Image weaponImage;
    public TextMeshProUGUI tmpgWeapon;

    private PlayerStats playerStats;

    private SpriteRenderer sr;
    private Color normalColor;

    // flashing color
    public Color fc;

    // Time between flashes, total number, and total time to flash.
    private const float flashSpeed = 0.1f;
    private const int totalFlashes = 4;
    private const float flashTime = totalFlashes * flashSpeed;

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

        //Still have to implement Board Manager
        //Get a component reference to the attached BoardManager script
        //boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        //InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {

        //Still have to implement Board Manager
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        //boardScript.SetupScene(level);

    }

    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        sr = player.GetComponent<SpriteRenderer>();
        RefreshStats();
        RefreshItem(playerStats.CurWeapon());

        normalColor = sr.color;
    }

    public void DamagePlayer(int d)
    {
        StartCoroutine(FlashObject(sr, normalColor, fc));
        playerStats.health -= d;
        RefreshStats();
        if (playerStats.health <= 0)
        {
            Destroy(player);
            RestartCurLevel();
        }
    }

    // Using a coroutine to wait whilst the flashing happens.
    IEnumerator FlashObject(SpriteRenderer toFlash, Color originalColor, Color flashColor)
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
                newColor = originalColor;
            }
            else
            {
                newColor = flashColor;
            }
        }
        toFlash.color = normalColor;
    }


    public void XpReward(int xp)
    {
        playerStats.curXP += xp;
        if(playerStats.curXP >= playerStats.lvlUpXP)
        {
            playerStats.curXP -= playerStats.lvlUpXP;
            playerStats.lvlUpXP *= 2;
            playerStats.curLVL += 1;
        }
        RefreshStats();
    }

    void RefreshStats()
    {
        tmpgHealth.text = playerStats.health.ToString();
        tmpgXP.text = playerStats.curXP.ToString() + "/" + playerStats.lvlUpXP.ToString();
        tmpgLVL.text = playerStats.curLVL.ToString();
    }

    void RefreshItem(Weapon weapon)
    {
        tmpgWeapon.text = weapon.itemName + " -- Damage: " + weapon.weaponDamage;
        weaponImage.sprite = weapon.weaponSprite;
    }

    public void RestartCurLevel()
    {
        playerStats.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}