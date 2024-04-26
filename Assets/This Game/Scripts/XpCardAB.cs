using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class XpCardAB : MonoBehaviour
{
    [SerializeField] private Text cardNameTextT;
    [SerializeField] private GameObject @lock;
    [SerializeField] private Animator animM;
    [SerializeField] private Image avatarImg;
    [SerializeField] private int idD = 0;

    private void Awake()
    {
        animM = this.GetComponent<Animator>();
        cardNameTextT.text = GameCharactersAB.instanceE.playerCharacters[idD].GetComponent<StatsAB>().Name;
        avatarImg.sprite = GameCharactersAB.instanceE.playerCharacters[idD].GetComponent<StatsAB>().avatarR;
    }

    public void CheckCardD()
    {
        UnlockCardD();
        int x = PlayerPrefs.GetInt("uCard" + idD, 0);

        if (x == 1)
        {
            @lock.SetActive(false);
            animM.SetTrigger("Pop");
            this.GetComponent<Outline>().enabled = true;
            this.GetComponent<Outline>().effectColor = Color.green;
        }
    }

    private void UnlockCardD()
    {
        PlayerPrefs.SetInt("uCard" + idD, 1);
        PlayerPrefs.Save();
    }
}