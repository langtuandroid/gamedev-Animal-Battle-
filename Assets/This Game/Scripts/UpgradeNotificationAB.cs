using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeNotificationAB : MonoBehaviour
{
    [SerializeField] private GameObject hintT;
    [SerializeField] private Text notificationTextT;

    [SerializeField] private int iI = 0;
    [SerializeField] private int notificationN = 0;

    private void FixedUpdate()
    {
        iI++;
        if (iI > 4)
        {
            iI = 0;
            CheckUpgradeE();
        }
    }

    private void CheckUpgradeE()
    {
        notificationN = 0;
        for (int id = 0; id < GameCharactersAB.instanceE.playerCharacters.Length; id++)
        {
            int level = PlayerPrefs.GetInt("P" + id, 1);
            float x = PlayerPrefs.GetInt("CardsFound" + id, 1);
            int CardsNeeded =
                (int)GameCharactersAB.instanceE.cardxLevelL[
                    Mathf.Clamp((level - 1), 0, GameCharactersAB.instanceE.cardxLevelL.Length - 1)];
            if (x > CardsNeeded)
                notificationN++;
        }

        if (notificationN > 0 && !hintT.activeSelf)
        {
            this.GetComponent<Animator>().SetBool("Highlight", true);
            hintT.SetActive(true);
        }
        else if (notificationN <= 0)
        {
            this.GetComponent<Animator>().SetBool("Highlight", false);
            hintT.SetActive(false);
        }

        notificationTextT.text = "" + notificationN;
    }
}