﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int curLVL = 1;
    public int curXP = 0;
    public int lvlUpXP = 100;


    public GameObject weaponObj;
    private Weapon weapon;

    private void Awake()
    {
        weapon = weaponObj.GetComponent<Weapon>();
    }

    public void ResetStats()
    {
        health = 100;
        curLVL = 1;
        curXP = 0;
        lvlUpXP = 100;
    }

    public Weapon CurWeapon()
    {
        return weapon;
    }

}
