using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;

    //public GameObject player;


     //public TextMeshProUGUI hpText;
    // Start is called before the first frame update
    void Start()
    {
    //    hpText.text = health.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
    // Just testing to see if the HP was being updated in the GUI
    //    Damage(1);
    }

    public void Damage(int d)
    {
        health -= d;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
