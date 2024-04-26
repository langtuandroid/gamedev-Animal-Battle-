using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterBtnAB : MonoBehaviour
{
    public int levelL = 1;
    public int priceE = 10;
    [SerializeField] private Text levelText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text priceText2;
    [SerializeField] private Image avatarR;
    public Sprite spP;
    [SerializeField] private Text nameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text rangeText;
    [SerializeField] private Text dmgText;
    [SerializeField] private Text speedText;
    public StatsAB mstatT;
    public GameObject statsObjj;

    private void Start()
    {
        mstatT = this.GetComponent<StatsAB>();
        UpdateInfo();
    }

    
    public void UpdateInfo()
    {
        levelText.text = "" + levelL;
        priceText.text = priceText2.text = "" + priceE;
        avatarR.sprite = spP;
        nameText.text = "" + mstatT.Name;
        healthText.text = "" + mstatT.healthH;
        rangeText.text = "" + mstatT.rangeE;
        speedText.text = "" + mstatT.movSpeedD;
        dmgText.text = "" + mstatT.dmgG;
    }
}