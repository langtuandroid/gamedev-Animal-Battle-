using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuAB : MonoBehaviour
{
    [SerializeField] private Text coinTextT;
    [SerializeField] private GameObject upgradeCardsPanelL;
    [SerializeField] private GameObject loadingG;
    [SerializeField] private Image sliderBarR;
    [SerializeField] private GameObject gDailyRewards;
    [SerializeField] private GameObject gChangeName;
    [SerializeField] private GameObject rateusDialogue;

    public void Start()
    {
        UpdateCoinsS();

        if (PlayerPrefs.GetInt("firstTimeReward", 0) == 0)
        {
            StartCoroutine(ShowRewardD());
        }
    }

    private IEnumerator ShowRewardD()
    {
        bool done = false;
        while (!done)
        {
            yield return new WaitForSeconds(0.5f);
            if (!gChangeName.activeSelf)
            {
                gDailyRewards.SetActive(true);
                done = true;
                PlayerPrefs.SetInt("firstTimeReward", 1);
                PlayerPrefs.Save();
            }
        }
    }

    public void StartBattleE()
    {
        loadingG.SetActive(true);
        BGMAB.instanceE.BtnClickSfxX();
        if (PlayerPrefs.GetInt("menuInter", 0) > 0)
        {
            // FBAdsManager.instance.ShowInterstitial();
        }
        else
        {
            PlayerPrefs.SetInt("menuInter", 1);
            PlayerPrefs.Save();
        }

        StartCoroutine(LoadNewSceneE(2));
    }


    private IEnumerator LoadNewSceneE(int sceneToLoad)
    {
        loadingG.SetActive(true);
        sliderBarR.fillAmount = 0;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        yield return new WaitForSeconds(0.2f);

        while (!async.isDone)
        {
            float progress = async.progress / 10000f;
            Debug.Log(progress);
            sliderBarR.fillAmount = progress;


            yield return null;
        }
    }

    public void UpdateCoinsS()
    {
        coinTextT.text = "" + PlayerPrefs.GetInt("Coins", 0);
    }

    public void UpgradeCardsS()
    {
        int r = UnityEngine.Random.Range(0, 2);
        if (r % 2 == 0)
        {
            
        }


        BGMAB.instanceE.BtnClickSfxX();
        upgradeCardsPanelL.SetActive(true);
    }

    public void AllCharactersS()
    {
        int r = UnityEngine.Random.Range(0, 2);
        if (r % 2 == 0)
        {
            AdsManagerAB.instanceE.ShowInterR();
        }
        else
            AdsManagerAB.instanceE.ShowCbB();


        loadingG.SetActive(true);
        BGMAB.instanceE.BtnClickSfxX();
        StartCoroutine(LoadNewSceneE(4));
    }

    public void RateUsS()
    {
        rateusDialogue.SetActive(true);
        BGMAB.instanceE.BtnClickSfxX();
    }

    public void MoreGamesS()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=300+Battle+Games");
        BGMAB.instanceE.BtnClickSfxX();
    }
}