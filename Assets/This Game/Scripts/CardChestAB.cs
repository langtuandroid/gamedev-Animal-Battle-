using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardChestAB : MonoBehaviour
{
    [SerializeField] private Image avatarImg;
    [SerializeField] private int chesttype = 0;
    [SerializeField] private int cardIdD = 0;
    [SerializeField] private int numCardsS = 0;

    [SerializeField] private Text cardNameTextT;
    [SerializeField] private Text reaminigRewardsTextT;
    [SerializeField] private Text cardLevelTextT;
    [SerializeField] private Text cardNeededTextT;
    [SerializeField] private Text cardsFoundTextT;
    [SerializeField] private Image fillBar;
    [SerializeField] private GameObject hint;
    [SerializeField] private GameObject characterCard;
    [SerializeField] private GameObject coinsCard;
    [SerializeField] private Text totalCashText;
    [SerializeField] private Text totalCashText2;
    [SerializeField] private GameObject highlightCashH;

    private int _cashtogiveE = 0;
    [SerializeField] private GameObject okBtn;
    [SerializeField] private Text okBtnTextT;
    public int count = 1;
    [SerializeField] private int cardsNeeded = 1;

    [SerializeField] private int level = 1;

    private void OnEnable()
    {
        count = chesttype + 1;
        StartCoroutine(UnlockChestT());
    }

    private IEnumerator UnlockChestT()
    {
        okBtn.SetActive(false);

        characterCard.SetActive(false);
        coinsCard.SetActive(false);
        yield return new WaitForEndOfFrame();
        reaminigRewardsTextT.text = ((chesttype + 1) - (count - 1)) + " OF " + ((chesttype + 1));
        cardsFoundTextT.text = "";

        if (count > 1)
        {
            UnlockCardsS();
            okBtnTextT.text = "MORE";
        }
        else
        {
            okBtnTextT.text = "GREAT";
            GiveCash();
        }

        count--;
    }

    private void GiveCash()
    {
        coinsCard.SetActive(true);

        switch (chesttype)
        {
            case 0:
                _cashtogiveE = Random.Range(50, 250);
                break;
            case 1:
                _cashtogiveE = Random.Range(100, 500);

                break;
            case 2:
                _cashtogiveE = Random.Range(100, 500);
                break;
        }

        float xxx = _cashtogiveE / 10;

        _cashtogiveE = (int)xxx * 10;
        StartCoroutine(FillCoins());
    }

    private void UnlockCardsS()
    {
        characterCard.SetActive(true);

        Xpoint:
        cardIdD = Random.Range(0, GameCharactersAB.instanceE.playerCharacters.Length);
        bool isvalidCardID = PlayerPrefs.GetInt("uCard" + cardIdD, 0) == 1 ? true : false;

        if (!isvalidCardID && cardIdD > 7)
            goto Xpoint;

        avatarImg.sprite = GameCharactersAB.instanceE.playerCharacters[cardIdD].GetComponent<StatsAB>().avatarR;

        switch (count)
        {
            case 1:
                numCardsS = Random.Range(10, 80);
                break;
            case 2:
                numCardsS = Random.Range(20, 100);
                break;
            case 3:
                numCardsS = Random.Range(30, 500);
                break;
            default:
                numCardsS = Random.Range(1, 50);

                break;
        }

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
            fillBar.fillAmount += 0.01f;

            yield return new WaitForSeconds(0.02f);

            if (fillBar.fillAmount / 0.2f == 0)
                yield return new WaitForSeconds(0.02f);
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

        for (int k = 0; k <= _cashtogiveE;)
        {
            cardsFoundTextT.text = "" + k;
            if (_cashtogiveE - k > 5000)
                k += 1000;
            else if (_cashtogiveE - k > 2000)
                k += 500;
            else if (_cashtogiveE - k > 1000)
                k += 100;
            else if (_cashtogiveE - k > 500)
                k += 50;
            else if (_cashtogiveE - k > 100)
                k += 20;
            else if (_cashtogiveE - k > 10)
                k += 10;
            else if (_cashtogiveE - k >= 0)
                k += 1;
            yield return new WaitForSeconds(0.02f);
        }

        highlightCashH.SetActive(true);

        totalCashText.text = "+" + _cashtogiveE;
        yield return new WaitForSeconds(1.2f);
        totalCashText2.text = "" + (tCash + _cashtogiveE);
        highlightCashH.SetActive(false);
        PlayerPrefs.SetInt("Coins", tCash + _cashtogiveE);
        PlayerPrefs.Save();


        yield return new WaitForSeconds(0.1f);
        okBtn.SetActive(true);
    }


    public void Okk()
    {
        if (count < 1)
        {
            count = chesttype + 1;
            this.gameObject.SetActive(false);
        }
        else
            StartCoroutine(UnlockChestT());
    }
}