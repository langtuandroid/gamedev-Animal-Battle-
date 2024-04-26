using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelBtnAB : MonoBehaviour
{
    [SerializeField] private int leveLId;
    [SerializeField] private GameObject xLockK;
    [SerializeField] private GameObject rewardsS;
    [SerializeField] private bool locked = true;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private Text levelName;
    [SerializeField] private Text coinsTextT;
    [SerializeField] private Text xpTextT;

    [HideInInspector] [SerializeField] private int coinsS;

    [HideInInspector] [SerializeField] private int xpP;


    private void Start()
    {
        xpP = Mathf.Clamp(20 + (leveLId + 1) * 5 * (leveLId + 1), 0, 300);

        if (leveLId > 39)
            coinsS = Mathf.Clamp(100 + (leveLId + 1) * 100 * (leveLId + 1), 0, 25000);
        else if (leveLId > 29)
            coinsS = Mathf.Clamp(100 + (leveLId + 1) * 100 * (leveLId + 1), 0, 20000);
        else if (leveLId > 19)
            coinsS = Mathf.Clamp(100 + (leveLId + 1) * 100 * (leveLId + 1), 0, 15000);
        else if (leveLId > 9)
            coinsS = Mathf.Clamp(100 + (leveLId + 1) * 100 * (leveLId + 1), 0, 10000);
        else
            coinsS = Mathf.Clamp(100 + (leveLId + 1) * 100 * (leveLId + 1), 0, 5000);

        levelName.text = "" + (leveLId + 1);
        coinsTextT.text = "" + coinsS;
        xpTextT.text = "" + xpP;

        if (leveLId < PlayerPrefs.GetInt("ulevels", 1))
        {
            locked = false;
            rewardsS.SetActive(true);
            xLockK.SetActive(false);
            this.GetComponent<Button>().enabled = true;
            this.GetComponent<Button>().interactable = true;

            int s = PlayerPrefs.GetInt("levelStars" + leveLId, 0);

            for (int i = 0; i < s; i++)
            {
                
                stars[i].SetActive(true);
            }
        }
        else
        {
            xLockK.SetActive(true);
            rewardsS.SetActive(false);
            this.GetComponent<Button>().enabled = false;
        }
    }

    public void SelectLevelL()
    {
        LevelManagerAB.rewardxPP = xpP;
        LevelManagerAB.rewardCoinsS = coinsS;
        LevelManagerAB.selectedD = true;
        LevelManagerAB.levelIdD = leveLId;
    }
}