using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialAB : MonoBehaviour
{
    [SerializeField] private GameObject[] tips;
    [SerializeField] private int currentTipP = -1;
    [SerializeField] private Toggle dontShowW;
    [SerializeField] private GameObject bgFade;

    private void Start()
    {
        if (PlayerPrefs.GetInt("DontShowTutorial", 0) > 0)
            SkipBtnN();
        else
            NextTip();
    }

    public void NextBtnN()
    {
        NextTip();
    }

    private void NextTip()
    {
        if (BGMAB.instanceE)
            BGMAB.instanceE.BtnClickSfxX();
        if (currentTipP >= 0 && currentTipP < tips.Length)
            tips[currentTipP].SetActive(false);

        currentTipP++;

        if (currentTipP < tips.Length)
            tips[currentTipP].SetActive(true);
        else
        {
            dontShowW.isOn = true;
            SkipBtnN();
        }
    }

    public void SkipBtnN()
    {
        if (BGMAB.instanceE)
            BGMAB.instanceE.BtnClickSfxX();

        if (dontShowW.isOn)
            PlayerPrefs.SetInt("DontShowTutorial", 1);

        bgFade.SetActive(false);
        this.gameObject.SetActive(false);
    }
}