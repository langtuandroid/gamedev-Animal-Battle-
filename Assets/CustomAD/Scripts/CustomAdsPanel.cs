using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomAdsPanel : MonoBehaviour {

    public CustomAd[] AllAds;
    int currentAd = 0;

    public Image AdImage;
    public Text TitleText;
    public Text TagLineText;

    public GameObject _mainPanel;
    static int AdShown = 0;

	// Use this for initialization
	void OnEnable () {

        if (AdShown>1)
            CloseAD();

        AdShown++;

        currentAd = Random.Range(0,AllAds.Length);
        AdImage.sprite = AllAds[currentAd].AdSprite;
        TitleText.text = AllAds[currentAd].Title;
        TagLineText.text = AllAds[currentAd].TagLine;

    }

    public void OpenLink()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + AllAds[currentAd].Link);
    }

    public void CloseAD()
    {
        _mainPanel.SetActive(false);
    }
	
}
