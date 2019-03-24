using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;

    //public GameObject player;
    public SpriteRenderer sr;
    private Color normalColor;

    // flashing color
    public Color fc;

    // Time between flashes, total number, and total time to flash.
    private const float flashSpeed = 0.1f;
    private const int totalFlashes = 4;
    private const float flashTime = totalFlashes * flashSpeed;


    //public GameObject player;


    //public TextMeshProUGUI hpText;

    void Start()
    {
        //    hpText.text = health.ToString();
        normalColor = sr.color;
    }

    void Update()
    {
    // Just testing to see if the HP was being updated in the GUI
    //    Damage(1);
    }

    public void Damage(int d)
    {
        StartCoroutine(FlashObject(sr, normalColor, fc));
        health -= d;
        if(health <= 0)
        {
            Destroy(gameObject);
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
    }
}
