using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StatsAB : MonoBehaviour
{
    public int level = 1;
    public string Name;
    public float bHealth = 10;
    public float bMovSpeed = 1;
    public float bRange = 1;
    public float bDmg = 1;

    [Space(5)] public Sprite avatarR;

    [HideInInspector] public float healthH = 1;

    [HideInInspector] public float dmgG = 1;

    [HideInInspector] public float rangeE = 1;

    [HideInInspector] public float movSpeedD;

    [Space(5)] public int price = 1;
    public int idD = 0;
    public bool isPlayerR = false;


    public void CopyStats(StatsAB b)
    {
        idD = b.idD;
        Name = b.Name;
        price = b.price;
        bHealth = b.bHealth;
        bDmg = b.bDmg;
        bRange = b.bRange;
        bMovSpeed = b.bMovSpeed;
        avatarR = b.avatarR;
    }


    private void Awake()
    {
        UpdateInfoO();
    }

    public void UpdateInfoO()
    {
        if (isPlayerR)
        {
            level = PlayerPrefs.GetInt("P" + idD, 1);
        }


        healthH = (int)(bHealth + (bHealth * ((level - 1) * (0.2f))));
        movSpeedD = (bMovSpeed + (bMovSpeed * ((level - 1) * (0.1f))));
        rangeE = (bRange + (bRange * ((level - 1) * (0.1f))));
        dmgG = (bDmg + (bDmg * ((level - 1) * (0.25f))));
    }
}