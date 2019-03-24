using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUpdate : MonoBehaviour
{
    public GameObject player;

    public TextMeshProUGUI tmpgHealth;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        tmpgHealth.text = stats.health.ToString();
    }
}
