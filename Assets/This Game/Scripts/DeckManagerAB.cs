using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeckManagerAB : MonoBehaviour
{
    [SerializeField] private CardAB[] deckCards;
    [SerializeField] private CardAB[] cardsS;
    [SerializeField] private int id = 0;
    [SerializeField] private GameObject[] selectBtnsS;
    [SerializeField] private GameObject[] changeBtnsS;
    [SerializeField] private Text avgCostText;


    private void OnEnable()
    {
        id = -1;


        StartCoroutine(UpdateDeck());
    }

    public void ChangeE(int x)
    {
        id = x;

        foreach (CardAB c in cardsS)
            c.GetComponent<Button>().enabled = true;

        foreach (GameObject g in changeBtnsS)
            g.SetActive(false);

        for (int kk = 0; kk < selectBtnsS.Length; kk++)
        {
            if (!cardsS[kk].lockedD)
                selectBtnsS[kk].SetActive(true);
        }

        foreach (CardAB c in deckCards)
        {
            c.GetComponent<Outline>().effectColor = Color.cyan;
            c.GetComponent<Button>().enabled = false;
        }

        deckCards[id].GetComponent<Outline>().effectColor = Color.yellow;
    }

    public void SelectT(int i)
    {
        if (id < 0)
            return;

        foreach (CardAB c in cardsS)
            c.GetComponent<Button>().enabled = false;

        for (int kk = 0; kk < selectBtnsS.Length; kk++)
        {
            
            selectBtnsS[kk].SetActive(false);
        }

        foreach (GameObject g in changeBtnsS)
            g.SetActive(true);

        foreach (CardAB c in deckCards)
        {
            c.GetComponent<Outline>().effectColor = Color.cyan;
            c.GetComponent<Button>().enabled = true;
        }

        CopyCardD(deckCards[id], cardsS[i]);
        PlayerPrefs.SetInt("DeckCard" + id, i);
        PlayerPrefs.Save();
        deckCards[id].GetComponent<Animator>().SetTrigger("zoom");
        UpdateCardsS();
    }

    private IEnumerator UpdateDeck()
    {
        foreach (CardAB c in deckCards)
        {
            c.GetComponent<Outline>().effectColor = Color.cyan;
            c.gameObject.SetActive(false);
        }

        foreach (CardAB c in deckCards)
        {
            yield return new WaitForEndOfFrame();
            c.gameObject.SetActive(true);
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < cardsS.Length; i++)
        {
            if (i < GameCharactersAB.instanceE.playerCharacters.Length)
            {
                cardsS[i].GetComponent<StatsAB>()
                    .CopyStats(GameCharactersAB.instanceE.playerCharacters[i].GetComponent<StatsAB>());
                cardsS[i].UpdateInfoO();
            }
        }

        UpdateCardsS();
    }

    private void CopyCardD(CardAB a, CardAB b)
    {
        a.mstatT.CopyStats(b.mstatT);
        b.gameObject.SetActive(false);
        //a.mstat.UpdateInfo();
        a.UpdateInfoO();
    }

    private void UpdateCardsS()
    {
        foreach (CardAB c in cardsS)
            c.gameObject.SetActive(true);


        int[] DeckID = new int[deckCards.Length];
        int TotalPrice = 0;

        for (int i = 0; i < deckCards.Length; i++)
        {
            DeckID[i] = PlayerPrefs.GetInt("DeckCard" + i, i);
            CopyCardD(deckCards[i], cardsS[DeckID[i]]);
            TotalPrice += cardsS[DeckID[i]].mstatT.price;
        }

        int avgCost = TotalPrice / deckCards.Length;
        avgCostText.text = "" + avgCost;
    }
}