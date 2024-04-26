using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace This_Game.Scripts
{
    public class LevelManager : MonoBehaviour {
	
        public GameObject loading;
	
        public GameObject nextbtn;

        public Text T_Cash;

        public GameObject[] Levels;

        public Text LevelsText;
        public Text TrophyText;
        public Text ProgressText;
        public Image ProgressFill;


        public static bool selected = false;
        public static int LevelId = 0;
        public static int rewardxP = 0, rewardCoins = 0;
        int tLevels = 1;
    
        Color32 baseColor;
        public Image sliderBar;
        public Text loadingProgressText;
        public Text loadingHintText;
        public string[] loadingHints;

        private void Start()
        {
       
            LevelId = rewardxP = rewardCoins = 0;
            baseColor = Levels[0].GetComponent<Outline>().effectColor;
            tLevels = Levels.Length;
            int ulevels = PlayerPrefs.GetInt("ulevels",0);
            LevelsText.text = (ulevels)+ " / "+ tLevels;
            int totalTrophies = tLevels * 3;
            int gainedTrophies = 0;

            for (int i = 0; i < ulevels; i++)
            {
                gainedTrophies += PlayerPrefs.GetInt("levelStars" + i, 0);
            }

            StartCoroutine(fillBAR(gainedTrophies,totalTrophies));

            TrophyText.text = gainedTrophies + " / " + totalTrophies;
            T_Cash.text = PlayerPrefs.GetInt("Coins",500)+"";
     
        
        }
        // Use this for initialization
        public void SelectLevel () {
            selected = false;
            BGMAB.instanceE.BtnClickSfxX();
            nextbtn.SetActive (true);
            foreach(GameObject g in Levels)
                g.GetComponent<Outline>().effectColor = baseColor;

            Levels[LevelId].GetComponent<Outline>().effectColor = Color.yellow;
        
        }
    
        public void next()
        {


            if (PlayerPrefs.GetInt("LevelSelection", 0) > 0)
            {
                if (LevelId % 2 == 0)
                {

                    //  AdsManager.instance.ShowInter();

                }
            }
            else
            {
                PlayerPrefs.SetInt("LevelSelection", 1);
                PlayerPrefs.Save();
            }
            BGMAB.instanceE.BtnClickSfxX();
            StartCoroutine (WaitandLoad(3));
        }

        public void back()
        {
            BGMAB.instanceE.BtnClickSfxX();

            //AdsManager.instance.ShowUnity();

            StartCoroutine(WaitandLoad(1));
        }


        void FixedUpdate()
        {
            if (selected)
            {
                SelectLevel();
            }
        }

        IEnumerator WaitandLoad(int sceneToLoad)
        {
            //loading.GetComponent<LoadingScreen>().sceneToLoad = 4;
        
            yield return new WaitForFixedUpdate();
            //SceneManager.LoadScene(2);
            loading.SetActive(true);

            sliderBar.fillAmount = 0;
            float progress = 0;
            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            if (sceneToLoad > 1)
            {
                progress = Random.Range(0.19f, 0.31f);
                while (sliderBar.fillAmount < progress)
                {

                    loadingProgressText.text = "" + (int)(sliderBar.fillAmount * 100) + "%";
                    if (sliderBar.fillAmount < 0.2f)
                    {
                        loadingHintText.text = loadingHints[0];
                    }
                    else if (sliderBar.fillAmount < 0.4f)
                    {
                        loadingHintText.text = loadingHints[1];
                    }
                    else if (sliderBar.fillAmount < 0.6f)
                    {
                        loadingHintText.text = loadingHints[2];
                    }
                    else if (sliderBar.fillAmount < 0.8f)
                    {
                        loadingHintText.text = loadingHints[3];
                    }
                    else if (sliderBar.fillAmount < 1f)
                    {
                        loadingHintText.text = loadingHints[4];
                    }

                    sliderBar.fillAmount += 0.01f;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        
            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            progress = Mathf.Clamp01(async.progress);

         
            while (sliderBar.fillAmount < 1.0f)
            {
                progress = Mathf.Clamp01(async.progress);
                loadingProgressText.text = "" + (int)(sliderBar.fillAmount * 100) + "%";
                if(sceneToLoad>1)
                    if (sliderBar.fillAmount < 0.2f)
                    {
                        loadingHintText.text = loadingHints[0];
                    }
                    else if (sliderBar.fillAmount < 0.4f)
                    {
                        loadingHintText.text = loadingHints[1];
                    }
                    else if (sliderBar.fillAmount < 0.6f)
                    {
                        loadingHintText.text = loadingHints[2];
                    }
                    else if (sliderBar.fillAmount < 0.8f)
                    {
                        loadingHintText.text = loadingHints[3];
                    }
                    else if (sliderBar.fillAmount < 1f)
                    {
                        loadingHintText.text = loadingHints[4];
                    }

                sliderBar.fillAmount += 0.01f;
                yield return new WaitForSeconds(0.005f);
            }
        
        
        }



        IEnumerator fillBAR(int a, int b)
        {

            ProgressFill.fillAmount = 0;

            yield return new WaitForSeconds(0.5f);

            float p = (float)a / (float)b;

            while (ProgressFill.fillAmount < p)
            {
                ProgressFill.fillAmount += 0.02f;
                float g = ProgressFill.fillAmount * 100;
                ProgressText.text = "" + (int)g+" %";
                yield return new WaitForFixedUpdate();
            }

        }


    }
}
