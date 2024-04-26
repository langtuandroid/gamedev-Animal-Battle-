using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyRewardAB : MonoBehaviour
{
    [SerializeField] private Text rewardTextT;
    [SerializeField] private Text dayTextT;
    [SerializeField] private Image avatarImgG;
    public int rewardIdD = 0;
    public int rewardCountT = 1;
    [SerializeField] private int idD;
    [Space(10)] [SerializeField] private GameObject xlockK;

    [Space(10)] [SerializeField] private GameObject hintT;

    [Space(10)] [SerializeField] private GameObject tickmarkK;

    private Sprite _spP;
    [SerializeField] private Sprite spCashH;

    public enum RewardType
    {
        cardD,
        cashH
    };

    public RewardType rewardtypeE;


    private void Start()
    {
        dayTextT.text = "Day " + (idD + 1);
        rewardTextT.text = "+" + rewardCountT;

        if (rewardtypeE == RewardType.cardD)
        {
            avatarImgG.sprite = GameCharactersAB.instanceE.playerCharacters[rewardIdD].GetComponent<StatsAB>().avatarR;
        }
        else
        {
            avatarImgG.sprite = spCashH;
        }
    }

    public void UpdateInfoO(int availableReward)
    {
        this.GetComponent<Outline>().effectColor = Color.white;
        this.GetComponent<Outline>().effectDistance = new Vector2(-2, -2);

        if (idD == availableReward)
            AvailableE();
        else if (idD < availableReward)
        {
            ClaimedD();
        }
        else
            LockedD();
    }

    private void AvailableE()
    {
        this.GetComponent<Outline>().effectColor = Color.yellow;
        this.GetComponent<Outline>().effectDistance = new Vector2(-3, -3);
        xlockK.SetActive(false);
        hintT.SetActive(true);
        tickmarkK.SetActive(false);
    }

    private void LockedD()
    {
        xlockK.SetActive(true);
        hintT.SetActive(false);
        tickmarkK.SetActive(false);
    }

    private void ClaimedD()
    {
        xlockK.SetActive(false);
        hintT.SetActive(false);
        tickmarkK.SetActive(true);
    }
}