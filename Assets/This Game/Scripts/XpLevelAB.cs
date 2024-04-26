using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class XpLevelAB : MonoBehaviour
{
    public GameObject[] cardsS;

    [SerializeField] private GameObject hintT;

    private void Start()
    {
        if (hintT)
            hintT.SetActive(false);
    }
}