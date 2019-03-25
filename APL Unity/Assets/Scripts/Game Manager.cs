using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Used to restart scene
using TMPro;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

       
    public GameObject player;
    private PlayerStats playerStats;

    public TextMeshProUGUI tmpgHealth;
    public TextMeshProUGUI tmpgXP;

    public PlayerStats stats;

    GameManager gameManager;

    public SpriteRenderer sr;
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
        instance = this;
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

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
        player.GetComponent<PlayerStats>();
        sr = player.GetComponent<SpriteRenderer>();
        tmpgHealth.text = stats.health.ToString();
        tmpgXP.text = stats.curXP.ToString() + "/" + stats.lvlUpXP.ToString();
        
        normalColor = sr.color;
    }

    public void DamagePlayer(int d)
    {
        StartCoroutine(FlashObject(sr, normalColor, fc));
        stats.health -= d;
        RefreshStats();
        if (stats.health <= 0)
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


    void RefreshStats()
    {
        tmpgHealth.text = stats.health.ToString();
        tmpgXP.text = stats.curXP.ToString() + "/" + stats.lvlUpXP.ToString();
    }

    public void RestartCurLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}