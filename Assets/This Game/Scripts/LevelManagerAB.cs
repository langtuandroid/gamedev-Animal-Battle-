using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelManagerAB : MonoBehaviour
{
    [SerializeField] private GameObject loadingG;

    [SerializeField] private GameObject nextbtnN;

    [SerializeField] private Text tcash;

    [SerializeField] private GameObject[] levels;

    [SerializeField] private Text levelsText;
    [SerializeField] private Text trophyText;
    [SerializeField] private Text progressText;
    [SerializeField] private Image progressFill;


    public static bool selectedD = false;
    public static int levelIdD = 0;
    public static int rewardxPP = 0, rewardCoinsS = 0;
    [SerializeField] private int tLevels = 1;

    [SerializeField] private Color32 baseColorR;
    [SerializeField] private Image sliderBarR;
    [SerializeField] private Text loadingProgressTextT;
    [SerializeField] private Text loadingHintTextT;
    [SerializeField] private string[] loadingHintsS;

    private void Start()
    {
        levelIdD = rewardxPP = rewardCoinsS = 0;
        baseColorR = levels[0].GetComponent<Outline>().effectColor;
        tLevels = levels.Length;
        int ulevels = PlayerPrefs.GetInt("ulevels", 0);
        levelsText.text = (ulevels) + " / " + tLevels;
        int totalTrophies = tLevels * 3;
        int gainedTrophies = 0;

        for (int i = 0; i < ulevels; i++)
        {
            gainedTrophies += PlayerPrefs.GetInt("levelStars" + i, 0);
        }

        StartCoroutine(FillBar(gainedTrophies, totalTrophies));

        trophyText.text = gainedTrophies + " / " + totalTrophies;
        tcash.text = PlayerPrefs.GetInt("Coins", 500) + "";
    }

    public void SelectLevelL()
    {
        selectedD = false;
        BGMAB.instanceE.BtnClickSfxX();
        nextbtnN.SetActive(true);
        foreach (GameObject g in levels)
            g.GetComponent<Outline>().effectColor = baseColorR;

        levels[levelIdD].GetComponent<Outline>().effectColor = Color.yellow;
    }

    public void Next()
    {
        if (PlayerPrefs.GetInt("LevelSelection", 0) > 0)
        {
            if (levelIdD % 2 == 0)
            {
                AdsManagerAB.instanceE.ShowInterR();
            }
            else
                AdsManagerAB.instanceE.ShowCbB();
        }
        else
        {
            PlayerPrefs.SetInt("LevelSelection", 1);
            PlayerPrefs.Save();
        }

        BGMAB.instanceE.BtnClickSfxX();
        StartCoroutine(WaitandLoadD(3));
    }

    public void Back()
    {
        BGMAB.instanceE.BtnClickSfxX();

        AdsManagerAB.instanceE.ShowUnityY();

        StartCoroutine(WaitandLoadD(1));
    }


    private void FixedUpdate()
    {
        if (selectedD)
        {
            SelectLevelL();
        }
    }

    private IEnumerator WaitandLoadD(int sceneToLoad)
    {
        //loading.GetComponent<LoadingScreen>().sceneToLoad = 4;

        yield return new WaitForFixedUpdate();
        //SceneManager.LoadScene(2);
        loadingG.SetActive(true);

        sliderBarR.fillAmount = 0;
        float progress = 0;
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        if (sceneToLoad > 1)
        {
            progress = Random.Range(0.19f, 0.31f);
            while (sliderBarR.fillAmount < progress)
            {
                loadingProgressTextT.text = "" + (int)(sliderBarR.fillAmount * 100) + "%";
                if (sliderBarR.fillAmount < 0.2f)
                {
                    loadingHintTextT.text = loadingHintsS[0];
                }
                else if (sliderBarR.fillAmount < 0.4f)
                {
                    loadingHintTextT.text = loadingHintsS[1];
                }
                else if (sliderBarR.fillAmount < 0.6f)
                {
                    loadingHintTextT.text = loadingHintsS[2];
                }
                else if (sliderBarR.fillAmount < 0.8f)
                {
                    loadingHintTextT.text = loadingHintsS[3];
                }
                else if (sliderBarR.fillAmount < 1f)
                {
                    loadingHintTextT.text = loadingHintsS[4];
                }

                sliderBarR.fillAmount += 0.01f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        progress = Mathf.Clamp01(async.progress);


        while (sliderBarR.fillAmount < 1.0f)
        {
            progress = Mathf.Clamp01(async.progress);
            loadingProgressTextT.text = "" + (int)(sliderBarR.fillAmount * 100) + "%";
            if (sceneToLoad > 1)
                if (sliderBarR.fillAmount < 0.2f)
                {
                    loadingHintTextT.text = loadingHintsS[0];
                }
                else if (sliderBarR.fillAmount < 0.4f)
                {
                    loadingHintTextT.text = loadingHintsS[1];
                }
                else if (sliderBarR.fillAmount < 0.6f)
                {
                    loadingHintTextT.text = loadingHintsS[2];
                }
                else if (sliderBarR.fillAmount < 0.8f)
                {
                    loadingHintTextT.text = loadingHintsS[3];
                }
                else if (sliderBarR.fillAmount < 1f)
                {
                    loadingHintTextT.text = loadingHintsS[4];
                }

            sliderBarR.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.005f);
        }
    }


    private IEnumerator FillBar(int a, int b)
    {
        progressFill.fillAmount = 0;

        yield return new WaitForSeconds(0.5f);

        float p = (float)a / (float)b;

        while (progressFill.fillAmount < p)
        {
            progressFill.fillAmount += 0.02f;
            float g = progressFill.fillAmount * 100;
            progressText.text = "" + (int)g + " %";
            yield return new WaitForFixedUpdate();
        }
    }
}