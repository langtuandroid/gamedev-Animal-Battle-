using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FreeChestAB : MonoBehaviour
{
    [SerializeField] private Button chestBtn;
    [SerializeField] private Text timeTextT;
    private ulong _lastChestOpened;
    [SerializeField] private float timeToOpen = 3;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject highlight;

    [SerializeField] private GameObject highlightChest;


    private void Start()
    {
        _lastChestOpened = ulong.Parse(PlayerPrefs.GetString("LastChestOpened"));
        if (!IsChestReadyY())
        {
            highlight.SetActive(false);
            chestBtn.interactable = false;
            chestBtn.GetComponent<Animator>().enabled = false;
            highlightChest.GetComponent<Outline>().enabled = false;
        }
    }


    private void Update()
    {
        if (!chestBtn.IsInteractable())
        {
            UpdateTextT();

            if (IsChestReadyY())
            {
                highlight.SetActive(true);
                chestBtn.interactable = true;
                chestBtn.GetComponent<Animator>().enabled = true;
                highlightChest.GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void OpenChestT()
    {
        _lastChestOpened = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastChestOpened", _lastChestOpened.ToString());
        highlight.SetActive(false);
        chestBtn.interactable = false;
        chestBtn.GetComponent<Animator>().enabled = false;
        highlightChest.GetComponent<Outline>().enabled = false;


        rewardPanel.SetActive(true);
    }

    private void UpdateTextT()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - _lastChestOpened);
        ulong m = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (float)(timeToOpen - m) / 1000.0f;


        int s = (int)secondsLeft;
        string r = "";
        r += (s / 3600).ToString() + "h ";
        s -= ((int)s / 3600) * 3600;
        r += (s / 60).ToString("00") + "m ";
        r += (s % 60).ToString("00") + "s";
        timeTextT.text = r;
    }

    private bool IsChestReadyY()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - _lastChestOpened);
        ulong m = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (float)(timeToOpen - m) / 1000.0f;

        if (secondsLeft < 0)
        {
            timeTextT.text = "UNLOCK FREE CHEST";
            return true;
        }

        return false;
    }
}