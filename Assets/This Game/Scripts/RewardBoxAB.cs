using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RewardBoxAB : MonoBehaviour
{
    [SerializeField] private Image avatarImg;

    public enum RewardType
    {
        xpP,
        coinsS,
        cardD
    }

    public RewardType mRewardTypeE;
    public int cardIdD = 0;
    public int numCardsS = 0;

    [SerializeField] private Text cardNameTextT;
    [SerializeField] private Text cardLevelTextT;
    [SerializeField] private Text cardNeededTextT;
    [SerializeField] private Text cardsFoundTextT;
    [SerializeField] private Image fillBar;
    [SerializeField] private GameObject hint;
    [SerializeField] private GameObject characterCard;
    [SerializeField] private GameObject coinsCard;
    [SerializeField] private GameObject xpCard;
    [SerializeField] private Text totalCashText;
    [SerializeField] private Text totalCashText2;
    [SerializeField] private GameObject highlightCashH;
    public int cashtogive = 0;
    [SerializeField] private GameObject okBtn;
    public bool rewarded = false;
    [SerializeField] private int cardsNeeded = 1;
    [SerializeField] private int level = 1;


    private void OnEnable()
    {
        StartCoroutine(UnlockChestT());
    }

    private IEnumerator UnlockChestT()
    {
        okBtn.SetActive(false);

        characterCard.SetActive(false);
        coinsCard.SetActive(false);
        yield return new WaitForEndOfFrame();
        cardsFoundTextT.text = "";

        switch (mRewardTypeE)
        {
            case RewardType.xpP:
                GiveXp();
                break;
            case RewardType.cardD:
                UnlockCardsS();
                break;
            case RewardType.coinsS:
                GiveCashH();
                break;
        }
    }

    private void GiveCashH()
    {
        coinsCard.SetActive(true);

        StartCoroutine(FillCoins());
    }

    private void GiveXp()
    {
        xpCard.SetActive(true);

        StartCoroutine(FillXp());
    }

    private void UnlockCardsS()
    {
        characterCard.SetActive(true);

        bool isvalidCardID = PlayerPrefs.GetInt("uCard" + cardIdD, 0) == 1 ? true : false;

        if (!isvalidCardID)
        {
            PlayerPrefs.GetInt("uCard" + cardIdD, 1);
        }

        avatarImg.sprite = GameCharactersAB.instanceE.playerCharacters[cardIdD].GetComponent<StatsAB>().avatarR;

        int j = PlayerPrefs.GetInt("CardsFound" + cardIdD, 1);
        PlayerPrefs.SetInt("CardsFound" + cardIdD, (j + numCardsS));
        PlayerPrefs.Save();
        StartCoroutine(FillBar());
    }


    private IEnumerator FillBar()
    {
        hint.SetActive(false);
        cardsFoundTextT.gameObject.SetActive(false);
        fillBar.fillAmount = 0;
        yield return new WaitForSeconds(0.1f);
        level = PlayerPrefs.GetInt("P" + cardIdD, 1);
        float x = PlayerPrefs.GetInt("CardsFound" + cardIdD, 1);
        cardLevelTextT.text = "LEVEL " + level;
        cardNameTextT.text = GameCharactersAB.instanceE.playerCharacters[cardIdD].GetComponent<StatsAB>().Name;

        cardsNeeded =
            (int)GameCharactersAB.instanceE.cardxLevelL[
                Mathf.Clamp((level - 1), 0, GameCharactersAB.instanceE.cardxLevelL.Length - 1)];
        cardNeededTextT.text = x + "/" + cardsNeeded;
        float i = Mathf.Clamp(x / cardsNeeded, 0, 1);

        if (x >= cardsNeeded)
        {
            hint.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        cardsFoundTextT.gameObject.SetActive(true);

        for (int k = 0; k <= numCardsS;)
        {
            cardsFoundTextT.text = "" + k;

            if (numCardsS - k > 100)
                k += 20;
            else if (numCardsS - k > 50)
                k += 10;
            else if (numCardsS - k > 10)
                k += 5;
            else
                k += 1;

            yield return new WaitForSeconds(0.01f);
        }

        while (fillBar.fillAmount < i)
        {
            fillBar.fillAmount += 0.02f;

            yield return new WaitForSeconds(0.01f);

            if (fillBar.fillAmount / 0.2f == 0)
                yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);

        okBtn.SetActive(true);
    }

    private IEnumerator FillCoins()
    {
        cardsFoundTextT.gameObject.SetActive(false);
        int tCash = PlayerPrefs.GetInt("Coins", 0);
        totalCashText.text = (tCash) + "";

        yield return new WaitForSeconds(0.1f);

        cardsFoundTextT.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        for (int k = 0; k <= cashtogive;)
        {
            cardsFoundTextT.text = "" + k;

            if (cashtogive - k > 5000)
                k += 1000;
            else if (cashtogive - k > 2000)
                k += 500;
            else if (cashtogive - k > 1000)
                k += 100;
            else if (cashtogive - k > 500)
                k += 50;
            else if (cashtogive - k > 100)
                k += 20;
            else if (cashtogive - k > 10)
                k += 10;
            else if (cashtogive - k >= 0)
                k += 1;
            yield return new WaitForSeconds(0.01f);
        }

        highlightCashH.SetActive(true);

        totalCashText.text = "+" + cashtogive;
        yield return new WaitForSeconds(1.2f);
        totalCashText2.text = "" + (tCash + cashtogive);
        highlightCashH.SetActive(false);

        PlayerPrefs.SetInt("Coins", tCash + cashtogive);
        PlayerPrefs.Save();


        yield return new WaitForSeconds(0.1f);

        okBtn.SetActive(true);
    }

    private IEnumerator FillXp()
    {
        cardsFoundTextT.gameObject.SetActive(false);
        int tXp = PlayerPrefs.GetInt("Xp", 0);

        yield return new WaitForSeconds(0.1f);

        cardsFoundTextT.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        for (int k = 0; k <= cashtogive;)
        {
            cardsFoundTextT.text = "" + k;

            if (cashtogive - k > 5000)
                k += 1000;
            else if (cashtogive - k > 2000)
                k += 500;
            else if (cashtogive - k > 1000)
                k += 100;
            else if (cashtogive - k > 500)
                k += 50;
            else if (cashtogive - k > 100)
                k += 20;
            else if (cashtogive - k > 10)
                k += 10;
            else if (cashtogive - k >= 0)
                k += 1;
            yield return new WaitForSeconds(0.01f);
        }


        PlayerPrefs.SetInt("Xp", tXp + cashtogive);
        PlayerPrefs.Save();


        yield return new WaitForSeconds(0.1f);
        okBtn.SetActive(true);
    }


    public void Ok()
    {
        rewarded = true;
        this.gameObject.SetActive(false);
    }
}