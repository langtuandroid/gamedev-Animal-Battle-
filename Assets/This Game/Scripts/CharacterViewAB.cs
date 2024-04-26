using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CharacterViewAB : MonoBehaviour
{
    [SerializeField] private CardAB[] cardsS;
    [SerializeField] private Image avatarImg;
    [SerializeField] private Text cardNameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text rangeText;
    [SerializeField] private Text levelText;


    [SerializeField] private Image fillBar;
    [SerializeField] private Text remainingCardsText;
    [SerializeField] private GameObject hint;
    [SerializeField] private int id = 0;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Scrollbar scC;
    [SerializeField] private GameObject loading;
    [SerializeField] private Text coinTextT;
    [SerializeField] private GameObject buyBtn;
    [SerializeField] private Text priceTextT;
    [Space(10)] 
    [SerializeField] private Text costText;
    [SerializeField] private Image healthFillBar;
    [SerializeField] private Image dmgFillBar;
    [SerializeField] private Image rangeFillBar;
    [SerializeField] private Image speedFillBar;
    [SerializeField] private int cardsNeeded = 1;

    [SerializeField] private GameObject insufficentCoins;
    [SerializeField] private GameObject cardUnlocked;

    
    private void Start()
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
        UpdateCoinsS();
    }

    private void UpdateCoinsS()
    {
        coinTextT.text = "" + PlayerPrefs.GetInt("Coins", 0);
    }

    private IEnumerator ShowCards()
    {
        id = 0;
        yield return new WaitForSeconds(0.2f);
        UpdateInfoO();
    }

    public void ShowInfoO(int id)
    {
        this.id = id;
        BGMAB.instanceE.BtnClickSfxX();
        UpdateInfoO();
    }

    public void NextCardD()
    {
        id = Mathf.Clamp(id + 1, 0, cardsS.Length - 1);
        scC.value = (float)id / (float)cardsS.Length;
        UpdateInfoO();
        BGMAB.instanceE.BtnClickSfxX();
    }

    public void PreCardD()
    {
        id = Mathf.Clamp(id - 1, 0, cardsS.Length - 1);
        scC.value = (float)id / (float)cardsS.Length;
        UpdateInfoO();
        BGMAB.instanceE.BtnClickSfxX();
    }

    private void UpdateInfoO()
    {
        hint.SetActive(false);
        fillBar.fillAmount = 0;

        foreach (CardAB c in cardsS)
            c.gameObject.GetComponent<Outline>().effectColor = Color.cyan;


        cardsS[id].gameObject.GetComponent<Outline>().effectColor = Color.yellow;

        if (cardsS[id].lockedD && cardsS[id].mstatT.idD > 7)
        {
            buyBtn.SetActive(true);
            priceTextT.text = "" + (cardsS[id].mstatT.price * 400);
        }
        else
        {
            buyBtn.SetActive(false);
            priceTextT.text = "" + 0;
        }

        foreach (GameObject g in characters)
            g.SetActive(false);

        characters[id].SetActive(true);

        levelText.text = "" + cardsS[id].mstatT.level;
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
        costText.text = "" + cardsS[id].mstatT.price;

        StartCoroutine(FillBarR(id));
    }

    

    private IEnumerator FillBarR(int id)
    {
        float x = PlayerPrefs.GetInt("CardsFound" + cardsS[id].mstatT.idD, 1);
        cardsNeeded =
            (int)GameCharactersAB.instanceE.cardxLevelL[
                Mathf.Clamp((cardsS[id].mstatT.level - 1), 0, GameCharactersAB.instanceE.cardxLevelL.Length - 1)];
        Debug.Log(cardsNeeded);
        remainingCardsText.text = x + "/" + cardsNeeded;
        float i = Mathf.Clamp(x / cardsNeeded, 0, 1);
        if (cardsS[id].lockedD)
            x = 0;

        if (x >= cardsNeeded)
        {
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

    public void BackK()
    {
        

        BGMAB.instanceE.BtnClickSfx2X();
        loading.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void UnLockK()
    {
        BGMAB.instanceE.BtnClickSfxX();
        int CostToUnlock = (cardsS[id].mstatT.price * 400);
        if (PlayerPrefs.GetInt("Coins", 0) >= CostToUnlock)
        {
            PlayerPrefs.SetInt("Coins", (PlayerPrefs.GetInt("Coins", 0) - CostToUnlock));
            PlayerPrefs.SetInt("uCard" + cardsS[id].mstatT.idD, 1);

            PlayerPrefs.Save();
            cardsS[id].UpdateInfoO();
            UpdateInfoO();
            cardUnlocked.SetActive(true);
        }
        else
            insufficentCoins.SetActive(true);
    }
}