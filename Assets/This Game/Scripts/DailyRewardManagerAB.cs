using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyRewardManagerAB : MonoBehaviour
{
    [SerializeField] private int lastRewardD = -1;
    private DateTime _lastRewardTimeE;
    [SerializeField] private int availableRewardD = -1;
    [SerializeField] private Text timeRemainingText;
    [SerializeField] private GameObject reaminigTimeE;
    [SerializeField] private GameObject claimBtnN;
    [SerializeField] private DailyRewardAB[] dailyRewards;
    [SerializeField] private RewardBoxAB rewardbox;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("lastReward"))
        {
            PlayerPrefs.SetInt("lastReward", -1);
            PlayerPrefs.Save();
        }

        UpdateInfoO();
        if (IsRewardAvailable())
        {
            claimBtnN.SetActive(true);
            reaminigTimeE.SetActive(false);
        }
        else
        {
            claimBtnN.SetActive(false);
            reaminigTimeE.SetActive(true);
        }
    }

    private void UpdateInfoO()
    {
        foreach (DailyRewardAB d in dailyRewards)
            d.UpdateInfoO(availableRewardD);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateInfoO();
        if (IsRewardAvailable())
        {
            claimBtnN.SetActive(true);
            reaminigTimeE.SetActive(false);
        }
        else
        {
            claimBtnN.SetActive(false);
            reaminigTimeE.SetActive(true);
        }
    }

    private bool IsRewardAvailable()
    {
        availableRewardD = 0;

        if (PlayerPrefs.HasKey("lastRewardTime"))
        {
            string lastTime = PlayerPrefs.GetString("lastRewardTime");
            _lastRewardTimeE = Convert.ToDateTime(lastTime);
            TimeSpan ts = DateTime.Now - _lastRewardTimeE;
            double hours = ts.TotalHours;

            double day = hours / 24;

            lastRewardD = PlayerPrefs.GetInt("lastReward", 0);
            availableRewardD = lastRewardD + 1;

            if (day >= 1 && day < 2)
            {
                if (availableRewardD > 6)
                    availableRewardD = 0;

                return true;
            }
            else if (day < 1)
            {
                double s = ts.TotalSeconds;

                s = 86400 - Mathf.Abs((float)s);
                int h = (int)s / 3600;
                s = s - (h * 3600);
                int m = (int)(s / 60);
                s = s - (m * 60);
                int seconds = (int)(s % 60);
                timeRemainingText.text = "" + h.ToString("00") + " H " + m.ToString("00") + " m " +
                                         seconds.ToString("00") + " s ";
                //TimeRemainingText.text = "" + seconds.ToString("00") + " s ";
                return false;
            }
            else
            {
                availableRewardD = 0;
                PlayerPrefs.SetInt("lastReward", -1);
                PlayerPrefs.Save();
                return true;
            }
        }

        return true;
    }

    public void ClaimM()
    {
        rewardbox.gameObject.SetActive(true);

        if (dailyRewards[availableRewardD].rewardtypeE == DailyRewardAB.RewardType.cardD)
        {
            rewardbox.mRewardTypeE = RewardBoxAB.RewardType.cardD;
            rewardbox.numCardsS = dailyRewards[availableRewardD].rewardCountT;
            rewardbox.cardIdD = dailyRewards[availableRewardD].rewardIdD;
        }
        else
        {
            rewardbox.mRewardTypeE = RewardBoxAB.RewardType.coinsS;
            rewardbox.cashtogive = dailyRewards[availableRewardD].rewardCountT;
        }

        PlayerPrefs.SetInt("lastReward", availableRewardD);
        PlayerPrefs.SetString("lastRewardTime", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        PlayerPrefs.Save();
    }
}