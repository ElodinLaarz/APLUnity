using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("HP/XP")]
    public int health = 0;
    public int curLVL = 1;
    public int curXP = 0;
    public int lvlUpXP = 100;

    [Header("Player Stats")]
    public int strength = 1;
    public int dexterity = 1;
    public int constitution = 1;
    public int wisdom = 1;
    public int intelligence = 1;
    public int charisma = 1;

    [Header("Base Stats")]
    public int baseHealth = 100;
    public float baseSpeed = 10f;


    //public WeaponObject weaponObj;

    private void Awake()
    {
        // weapon = weaponObj.GetComponent<Weapon>();
    }

    public void ResetStats()
    {
        health = 100;
        curLVL = 1;
        curXP = 0;
        lvlUpXP = 100;
    }

}
