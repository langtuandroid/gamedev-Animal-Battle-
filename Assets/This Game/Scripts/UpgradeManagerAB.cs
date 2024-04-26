using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeManagerAB : MonoBehaviour
{
    [SerializeField] private CardAB[] cardsS;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image avatarImg;
    [SerializeField] private Text cardNameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text rangeText;
    [Space(10)] [SerializeField] private Text healthText2;
    [SerializeField] private Text speedText2;
    [SerializeField] private Text damageText2;
    [SerializeField] private Text rangeText2;
    [Space(10)] [SerializeField] private Text levelText;
    [SerializeField] private Button upgradeBtn;
    [Space(10)] [SerializeField] private Image healthFillBar;
    [SerializeField] private Image dmgFillBar;
    [SerializeField] private Image rangeFillBar;
    [SerializeField] private Image speedFillBar;

    [Space(10)] [SerializeField] private Image healthFillBar2;
    [SerializeField] private Image dmgFillBar2;
    [SerializeField] private Image rangeFillBar2;
    [SerializeField] private Image speedFillBar2;
    [SerializeField] private Text costText;


    [SerializeField] private Image fillBar;
    [SerializeField] private Text remainingCardsText;
    [SerializeField] private GameObject hint;
    [SerializeField] private Text tCostT;
    [SerializeField] private Text tCashH;
    [SerializeField] private int costToUpgradeE = 0;

    [SerializeField] private GameObject insufficentCoins;
    [SerializeField] private GameObject cardUpgraded;
    [SerializeField] private int id = 0;
    [SerializeField] private int cardsNeeded = 1;


    private void Awake()
    {
        for (int i = 0; i < cardsS.Length; i++)
        {
            if (i < GameCharactersAB.instanceE.playerCharacters.Length)
            {
                cardsS[i].GetComponent<StatsAB>()
                    .CopyStats(GameCharactersAB.instanceE.playerCharacters[i].GetComponent<StatsAB>());
                cardsS[i].UpdateInfoO();
            }
        }

        StartCoroutine(ShowCards());
    }


    private IEnumerator ShowCards()
    {
        yield return new WaitForSeconds(0.05f);

        foreach (CardAB g in cardsS)
        {
            yield return new WaitForSeconds(0.05f);
            g.gameObject.SetActive(true);
        }
    }

    public void ShowInfoO(int id)
    {
        this.id = id;
        BGMAB.instanceE.BtnClickSfxX();
        infoPanel.SetActive(true);
        UpdateInfoO();
    }

    private void UpdateInfoO()
    {
        hint.SetActive(false);
        upgradeBtn.interactable = false;
        fillBar.fillAmount = 0;
        tCashH.text = "" + PlayerPrefs.GetInt("Coins", 0);

        levelText.text = "Level " + cardsS[id].mstatT.level;
        avatarImg.sprite = cardsS[id].mstatT.avatarR;
        cardNameText.text = cardsS[id].mstatT.Name;

        damageText.text = "" + cardsS[id].mstatT.dmgG;
        dmgFillBar.fillAmount = (cardsS[id].mstatT.dmgG / 110);
        healthText.text = "" + cardsS[id].mstatT.healthH;
        healthFillBar.fillAmount = (cardsS[id].mstatT.healthH / 2000);
        rangeText.text = "" + cardsS[id].mstatT.rangeE;
        rangeFillBar.fillAmount = (cardsS[id].mstatT.rangeE / 30);
        speedText.text = "" + cardsS[id].mstatT.movSpeedD;
        speedFillBar.fillAmount = (cardsS[id].mstatT.movSpeedD / 3);

        float health = (int)(cardsS[id].mstatT.bHealth +
                             (cardsS[id].mstatT.bHealth * ((cardsS[id].mstatT.level) * (0.2f))));
        float movSpeed = (cardsS[id].mstatT.bMovSpeed +
                          (cardsS[id].mstatT.bMovSpeed * ((cardsS[id].mstatT.level) * (0.1f))));
        float range = (cardsS[id].mstatT.bRange + (cardsS[id].mstatT.bRange * ((cardsS[id].mstatT.level) * (0.1f))));
        float dmg = (cardsS[id].mstatT.bDmg + (cardsS[id].mstatT.bDmg * ((cardsS[id].mstatT.level) * (0.25f))));

        damageText2.text = "+" + (dmg - cardsS[id].mstatT.dmgG);
        dmgFillBar2.fillAmount = (dmg / 110);
        healthText2.text = "+" + (health - cardsS[id].mstatT.healthH);
        healthFillBar2.fillAmount = (health / 2000);
        rangeText2.text = "+" + (range - cardsS[id].mstatT.rangeE);
        rangeFillBar2.fillAmount = (range / 30);
        speedText2.text = "+" + (movSpeed - cardsS[id].mstatT.movSpeedD);
        speedFillBar2.fillAmount = (movSpeed / 3);

        costText.text = "" + cardsS[id].mstatT.price;

        StartCoroutine(FillBar(id));
    }


    private IEnumerator FillBar(int id)
    {
        float x = PlayerPrefs.GetInt("CardsFound" + cardsS[id].mstatT.idD, 1);
        cardsNeeded = (int)GameCharactersAB.instanceE.cardxLevelL[
            Mathf.Clamp((cardsS[id].mstatT.level - 1), 0, GameCharactersAB.instanceE.cardxLevelL.Length - 1)];

        costToUpgradeE = cardsNeeded * 200;
        tCostT.text = costToUpgradeE.ToString();
        remainingCardsText.text = x + "/" + cardsNeeded;
        float i = Mathf.Clamp(x / cardsNeeded, 0, 1);
        if (x >= cardsNeeded)
        {
            upgradeBtn.interactable = true;
            hint.SetActive(true);
        }

        while (fillBar.fillAmount < i)
        {
            fillBar.fillAmount += 0.01f;
            yield return new WaitForEndOfFrame();

            if (fillBar.fillAmount / 0.2f == 0)
                yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.1f);
    }

    public void UpgradeE()
    {
        BGMAB.instanceE.BtnClickSfxX();
        if (PlayerPrefs.GetInt("Coins", 0) >= costToUpgradeE)
        {
            int x = PlayerPrefs.GetInt("CardsFound" + cardsS[id].mstatT.idD, 1);
            PlayerPrefs.SetInt("CardsFound" + cardsS[id].mstatT.idD, (x - cardsNeeded));
            PlayerPrefs.SetInt("Coins", (PlayerPrefs.GetInt("Coins", 0) - costToUpgradeE));
            PlayerPrefs.SetInt("P" + id, (cardsS[id].mstatT.level + 1));
            PlayerPrefs.Save();

            cardsS[id].UpdateInfoO();
            UpdateInfoO();
            cardUpgraded.SetActive(true);
        }
        else
            insufficentCoins.SetActive(true);
    }
}