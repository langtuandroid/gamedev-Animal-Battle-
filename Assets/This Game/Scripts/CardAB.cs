using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardAB : MonoBehaviour
{
    [SerializeField] private Text levelTextT;
    [SerializeField] private Text cardNameTextT;
    [SerializeField] private Image fillBar;
    [SerializeField] private Text remainingCardsText;
    [SerializeField] private Image avatarImg;
    [SerializeField] private Sprite avatarR;
    [SerializeField] private GameObject hint;
    [HideInInspector] public StatsAB mstatT;
    [SerializeField] private GameObject xLockK;
    [SerializeField] private Text priceTextT;
    [HideInInspector] public bool lockedD = true;
    [SerializeField] private bool effectBtnN = true;
    [SerializeField] private bool isDeckCardD = false;
    [SerializeField] private int cardsNeededD = 1;
    private Coroutine _c = null;

    private void OnEnable()
    {
        UpdateInfoO();
    }


    public void UpdateInfoO()
    {
        mstatT = this.GetComponent<StatsAB>();
        mstatT.UpdateInfoO();
        avatarImg.sprite = mstatT.avatarR;
        if (priceTextT)
            priceTextT.text = mstatT.price + "";

        levelTextT.text = "LEVEL " + mstatT.level;
        int x = PlayerPrefs.GetInt("uCard" + mstatT.idD, 0);
        if (!isDeckCardD)
            if (x == 1 || mstatT.idD <= 7)
            {
                lockedD = false;
                xLockK.SetActive(false);
                this.GetComponent<Button>().interactable = true;
            }
            else
            {
                xLockK.SetActive(true);
                if (effectBtnN)
                    this.GetComponent<Button>().interactable = false;
            }

        if (_c != null)
            StopCoroutine(_c);

        _c = StartCoroutine(FillBarR());
    }

    private IEnumerator FillBarR()
    {
        hint.SetActive(false);
        fillBar.fillAmount = 0;
        float y = PlayerPrefs.GetInt("CardsFound" + mstatT.idD, 1);


        cardsNeededD =
            (int)GameCharactersAB.instanceE.cardxLevelL[
                Mathf.Clamp((mstatT.level - 1), 0, GameCharactersAB.instanceE.cardxLevelL.Length - 1)];

        cardNameTextT.text = mstatT.Name;

        remainingCardsText.text = y + "/" + cardsNeededD;
        float kkg = Mathf.Clamp((y / cardsNeededD), 0.0f, 1.0f);

        if (y >= cardsNeededD)
        {
            //this.GetComponent<Button>().enabled = true;
            hint.SetActive(true);
        }

        fillBar.fillAmount = 0.0f;

        while (fillBar.fillAmount < (float)kkg)
        {
            fillBar.fillAmount += 0.01f;
            yield return new WaitForEndOfFrame();


            if (fillBar.fillAmount % 0.1f == 0)
                yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);
    }
}