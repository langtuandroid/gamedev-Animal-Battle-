using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VictoryChestAB : MonoBehaviour
{
    [SerializeField] private Button chestBtn;
    [SerializeField] private Text victoryProgressTextT;

    [SerializeField] private int victoryToOpen = 5;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Image victoryFillBarR;
    [SerializeField] private bool filling = false;

    private void Start()
    {
        if (!IsChestReadyY())
        {
            chestBtn.interactable = false;
            chestBtn.GetComponent<Animator>().SetBool("available", false);
        }

        UpdateTextT();
    }


    public void OpenChestT()
    {
        PlayerPrefs.SetInt("VictoryChests", 0);

        chestBtn.interactable = false;
        chestBtn.GetComponent<Animator>().SetBool("available", false);
        UpdateTextT();
        rewardPanel.GetComponent<CardChestAB>().count = 3;
        rewardPanel.SetActive(true);
    }

    private void UpdateTextT()
    {
        int s = PlayerPrefs.GetInt("VictoryChests", 0);
        s = Mathf.Clamp(s, 0, victoryToOpen);
        string r = s + " / " + victoryToOpen;

        victoryProgressTextT.text = r;

        if (!filling)
            StartCoroutine(FillBar(s));
    }

    private bool IsChestReadyY()
    {
        int Victorychests = PlayerPrefs.GetInt("VictoryChests", 0);
        Victorychests = Mathf.Clamp(Victorychests, 0, victoryToOpen);
        if (Victorychests >= victoryToOpen)
        {
            chestBtn.interactable = true;
            chestBtn.GetComponent<Animator>().SetBool("available", true);
            //victoryProgressText.text = "UNLOCK VICTORY CHEST";
            return true;
        }

        return false;
    }


    private IEnumerator FillBar(float x)
    {
        filling = true;
        float i = x / victoryToOpen;
        victoryFillBarR.fillAmount = 0;
        while (victoryFillBarR.fillAmount < i)
        {
            victoryFillBarR.fillAmount += 0.01f;
            yield return new WaitForEndOfFrame();

            if (victoryFillBarR.fillAmount / 0.2f == 0)
                yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        filling = false;
    }
}