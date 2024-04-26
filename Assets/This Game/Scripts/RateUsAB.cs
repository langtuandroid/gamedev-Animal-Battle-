using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RateUsAB : MonoBehaviour
{
    [SerializeField] private GameObject[] starsS;
    [SerializeField] private Text messageTextT;
    [SerializeField] private GameObject thanksDialogue;

    private void Start()
    {
        int i = PlayerPrefs.GetInt("StarRating", 0);
        if (i > 0 && i < 4)
        {
            messageTextT.text = "Have you changed your mind \n Please Let us know";
            StartCoroutine(ShowStarsS(i));
        }
    }

    public void StarRatingG(int i)
    {
        PlayerPrefs.SetInt("StarRating", i);
        PlayerPrefs.Save();
        StartCoroutine(ShowStarsS(i, true));
    }

    private IEnumerator ShowStarsS(int i, bool xxx = false)
    {
        for (int j = 0; j < i; j++)
        {
            starsS[j].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        if (i > 3)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        }

        if (xxx)
            thanksDialogue.SetActive(true);
    }
}