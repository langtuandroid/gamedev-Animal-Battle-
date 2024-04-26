using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AchievementAB : MonoBehaviour
{
    [SerializeField] private int achievmentidD;

    private enum AchievementType
    {
        Kill,
        Play,
        Win
    }

    [SerializeField] private AchievementType mAchievementType;
    [SerializeField] private string achievmentNameE = "Achievement";

    [HideInInspector] [SerializeField] private int currentAchievmentT = 0;
    [SerializeField] private int maxAchievements = 1;
    [SerializeField] private int targetId;


    [Space(5)] [SerializeField] private Text rewardnameTextT;
    [SerializeField] private Text missionObjectiveText;
    [SerializeField] private Text remainingText;
    [SerializeField] private Text rewardText;

    [SerializeField] private Image targetImg;

    [SerializeField] private Text targetText;
    [SerializeField] private Image rewardImageE;
    [SerializeField] private GameObject claimBtn;
    [SerializeField] private GameObject tickK;

    [HideInInspector] [SerializeField] private bool completedD = false;


    [HideInInspector] [SerializeField] private bool availableE = false;

    [Space(5)] [SerializeField] private Sprite xpImgG;

    [Space(5)] [SerializeField] private Sprite moneyImgG;

    [Space(5)] [SerializeField] private Sprite winImg;

    [SerializeField]
    private enum RewardType
    {
        Xp,
        Coins,
        Card
    }

    [SerializeField] private RewardType mRewardTypeE;
    [SerializeField] private int rewardIdD;
    [SerializeField] private int rewardCountT;

    [SerializeField] private string achievementVariable = "Achievment1";
    [SerializeField] private Color normalColorR;
    [SerializeField] private Color highlightColorR;
    [SerializeField] private Image fillBar;
    [SerializeField] private Text missionNumberText;

    public RewardBoxAB rewardboxX;

    
    private void OnEnable()
    {
        UpdateAchievementT();
    }


    private void UpdateAchievementT()
    {
        missionNumberText.text = "Mission # " + (achievmentidD + 1);

        if (PlayerPrefs.GetInt("Achievement" + achievmentidD, 0) > 0)
        {
            completedD = true;
        }

        switch (mAchievementType)
        {
            case AchievementType.Kill:
                achievmentNameE = "Kill " + maxAchievements + " " +
                                  GameCharactersAB.instanceE.playerCharacters[targetId].GetComponent<StatsAB>().Name +
                                  "s in Battle";
                targetImg.sprite = GameCharactersAB.instanceE.playerCharacters[targetId].GetComponent<StatsAB>().avatarR;
                achievementVariable = "Killed" + targetId;
                targetText.text = "KILL";
                break;
            case AchievementType.Play:

                achievmentNameE = "Spawn " + maxAchievements + " " +
                                  GameCharactersAB.instanceE.playerCharacters[targetId].GetComponent<StatsAB>().Name +
                                  "s in Battle";
                targetImg.sprite = GameCharactersAB.instanceE.playerCharacters[targetId].GetComponent<StatsAB>().avatarR;
                achievementVariable = "Spawned" + targetId;
                targetText.text = "PLAY";
                break;
            case AchievementType.Win:
                targetImg.sprite = winImg;
                achievementVariable = "Victories";
                achievmentNameE = "Win " + maxAchievements + " Battles";
                targetText.text = "" + maxAchievements;
                break;
        }

        missionObjectiveText.text = achievmentNameE;
        switch (mRewardTypeE)
        {
            case RewardType.Xp:
                rewardnameTextT.text = "XP";
                rewardImageE.sprite = xpImgG;
                break;
            case RewardType.Card:
                rewardnameTextT.text =
                    "" + GameCharactersAB.instanceE.playerCharacters[rewardIdD].GetComponent<StatsAB>().Name;
                rewardImageE.sprite = GameCharactersAB.instanceE.playerCharacters[rewardIdD].GetComponent<StatsAB>().avatarR;
                break;
            case RewardType.Coins:
                rewardnameTextT.text = "CASH";
                rewardImageE.sprite = moneyImgG;
                break;
        }


        currentAchievmentT = Mathf.Clamp(PlayerPrefs.GetInt(achievementVariable, 0), 0, maxAchievements);
        rewardText.text = "+" + rewardCountT + "";


        remainingText.text = currentAchievmentT + " / " + maxAchievements;
        StartCoroutine(FillprogressBar());

        if (!completedD)
        {
            if (currentAchievmentT >= maxAchievements)
            {
                AvailableE();
            }
            else
                Normal();
        }
        else
        {
            Unlocked();
        }
    }

    private IEnumerator FillprogressBar()
    {
        fillBar.fillAmount = 0;
        float i = (float)currentAchievmentT / (float)maxAchievements;
        
        Debug.Log("" + achievmentidD + " : " + i);
        while (fillBar.fillAmount < i)
        {
            fillBar.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    private void Normal()
    {
        this.GetComponent<Outline>().effectColor = normalColorR;
        availableE = false;
        claimBtn.SetActive(true);
        claimBtn.GetComponent<Button>().interactable = false;
    }

    private void Unlocked()
    {
        this.GetComponent<Outline>().effectColor = normalColorR;
        claimBtn.SetActive(false);
        tickK.SetActive(true);
        availableE = false;
    }

    private void AvailableE()
    {
        this.GetComponent<Outline>().effectColor = highlightColorR;
        claimBtn.SetActive(true);
        availableE = true;
        claimBtn.GetComponent<Button>().interactable = true;
    }

    public void ClaimM()
    {
        switch (mRewardTypeE)
        {
            case RewardType.Xp:
                rewardboxX.mRewardTypeE = RewardBoxAB.RewardType.xpP;
                rewardboxX.cashtogive = rewardCountT;

                break;
            case RewardType.Card:
                rewardboxX.mRewardTypeE = RewardBoxAB.RewardType.cardD;
                rewardboxX.numCardsS = rewardCountT;
                rewardboxX.cardIdD = rewardIdD;
                break;
            case RewardType.Coins:
                rewardboxX.mRewardTypeE = RewardBoxAB.RewardType.coinsS;
                rewardboxX.cashtogive = rewardCountT;
                break;
        }

        rewardboxX.gameObject.SetActive(true);


        PlayerPrefs.SetInt("Achievement" + achievmentidD, 1);
        PlayerPrefs.Save();
        UpdateAchievementT();
        completedD = true;
        availableE = false;
    }
}