using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    int health = 100;

    public TextMeshProUGUI hpText;
    // Start is called before the first frame update
    void Start()
    {
        hpText.text = health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damage(int d)
    {

    }
}
