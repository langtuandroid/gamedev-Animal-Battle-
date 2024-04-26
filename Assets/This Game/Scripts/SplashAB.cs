using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SplashAB : MonoBehaviour
{
    [SerializeField] private float waitTimeE;
    [SerializeField] private Image sliderBarR;
    [SerializeField] private GameObject loading;
    [SerializeField] private Text loadingProgressTextT;


    private void Start()
    {
        StartCoroutine(WaitandLoad());
    }

    private IEnumerator WaitandLoad()
    {
        yield return new WaitForSeconds(waitTimeE);

        StartCoroutine(LoadNewSceneE(1));
    }


    private IEnumerator LoadNewSceneE(int sceneNum)
    {
        yield return new WaitForFixedUpdate();

        loading.SetActive(true);

        sliderBarR.fillAmount = 0;

        float progress = Random.Range(0.19f, 0.31f);
        while (sliderBarR.fillAmount < progress)
        {
            loadingProgressTextT.text = "" + (int)(sliderBarR.fillAmount * 100) + "%";


            sliderBarR.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneNum);

        progress = Mathf.Clamp01(async.progress);


        while (sliderBarR.fillAmount < 1.0f)
        {
            progress = Mathf.Clamp01(async.progress);
            loadingProgressTextT.text = "" + (int)(sliderBarR.fillAmount * 100) + "%";


            sliderBarR.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.005f);
        }
    }
}