using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ExperienceManagerAB : MonoBehaviour
{
    [SerializeField] private Text xpNeededTextT;
    [SerializeField] private Text xpLevelTextT;
    [SerializeField] private Image xpFillBarR;
    [SerializeField] private Text xpLevelText2T;
    [SerializeField] private Slider xpSliderR;
    [SerializeField] private Text cardsUnlockedText;
    [SerializeField] private GameObject[] lvlBtns;

    [SerializeField] private int xpNeededD = 0;
    [SerializeField] private int xpP = 0;
    [SerializeField] private int xX = 1;
    [SerializeField] private GameObject xpRewardPanel;
    [SerializeField] private ScrollRect xpScrollL;


    private void Start()
    {
        ChekXpP();
    }

    private void ChekXpP()
    {
        xX = PlayerPrefs.GetInt("XpLevel", 1);
        xpNeededD = (int)((50 * xX) * ((xX) * 2));
        xpP = PlayerPrefs.GetInt("Xp", 0);
        xpLevelTextT.text = "" + xX;
        xpLevelText2T.text = "Level " + xX;

        xpNeededTextT.text = xpP + " / " + xpNeededD;
        StartCoroutine(FillBarR());
    }

    private IEnumerator FillBarR()
    {
        float i = Mathf.Clamp((float)xpP / xpNeededD, 0, 1);
        //Debug.Log(i);
        xpFillBarR.fillAmount = 0f;
        xpFillBarR.GetComponent<Outline>().enabled = true;
        while (xpFillBarR.fillAmount < i)
        {
            xpFillBarR.fillAmount += 0.01f;
            yield return new WaitForEndOfFrame();

            if (xpFillBarR.fillAmount / 0.2f == 0)
                yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.2f);
        xpFillBarR.GetComponent<Outline>().enabled = false;

        if (xpP >= xpNeededD)
        {
            xX += 1;
            xpP -= xpNeededD;
            PlayerPrefs.SetInt("XpLevel", (int)xX);
            PlayerPrefs.SetInt("Xp", xpP);
            PlayerPrefs.Save();
            this.GetComponent<Animator>().SetTrigger("zoom");
            ChekXpP();
            yield return new WaitForSeconds(1f);
            OpenXpRewardsS();
        }
    }


    public void OpenXpRewardsS()
    {
        xpRewardPanel.SetActive(true);
        StartCoroutine(FillSliderR());
    }

    private IEnumerator FillSliderR()
    {
        float i = (float)(xpP / xpNeededD) * 100;
        xpSliderR.value = 0f;
        //xpFillBar.GetComponent<Outline>().enabled = true;
        i = (100 * xX) + i;
        int unlockedCards = 7;
        float xpscrollposition = 0;
        while (xpSliderR.value < i)
        {
            xpSliderR.value += 1f;
            yield return new WaitForEndOfFrame();

            if (xpSliderR.value % 100 == 0)
            {
                xpscrollposition = Mathf.Clamp01((xpSliderR.value - 100) / xpSliderR.maxValue);
                while (xpScrollL.horizontalNormalizedPosition < xpscrollposition)
                {
                    xpScrollL.horizontalNormalizedPosition += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }

                int g = (int)(xpSliderR.value / 100);

                lvlBtns[g - 1].GetComponent<Outline>().enabled = true;
                lvlBtns[g - 1].GetComponent<Animator>().SetTrigger("Pop");
                yield return new WaitForSeconds(0.01f);
                lvlBtns[g - 1].GetComponent<XpLevelAB>().enabled = true;

                unlockedCards += lvlBtns[g - 1].GetComponent<XpLevelAB>().cardsS.Length;
                foreach (GameObject h in lvlBtns[g - 1].GetComponent<XpLevelAB>().cardsS)
                {
                    h.GetComponent<XpCardAB>().enabled = true;
                    h.GetComponent<XpCardAB>().CheckCardD();
                    yield return new WaitForSeconds(0.5f);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        int uCards = 8;
        for (int j = 8; j < GameCharactersAB.instanceE.playerCharacters.Length; j++)
        {
            uCards += PlayerPrefs.GetInt("uCard" + j, 0);
        }

        cardsUnlockedText.text = unlockedCards + " / " + GameCharactersAB.instanceE.playerCharacters.Length;
        yield return new WaitForSeconds(0.2f);
        //xpSlider.GetComponent<Outline>().enabled = false;
    }
}