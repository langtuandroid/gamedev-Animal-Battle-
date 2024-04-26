using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class TipsManagerAB : MonoBehaviour
{
    [SerializeField] private Text tipTextT;
    [SerializeField] private string[] tipsS;
    [SerializeField] private int currentTipP = -1, lastTipP = -1;

    private void Start()
    {
        InvokeRepeating("ChangeTipP", 0.1f, 2f);
    }

    private IEnumerator LoadTipP()
    {
        yield return new WaitForSeconds(1f);
        ChangeTipP();
    }

    private void ChangeTipP()
    {
        X:
        currentTipP = Random.Range(0, tipsS.Length);
        if (currentTipP == lastTipP)
            goto X;
        else
        {
            lastTipP = currentTipP;
            tipTextT.text = "" + tipsS[currentTipP];
        }
    }
}