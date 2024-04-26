using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace This_Game.Scripts
{
    public class MainMenu : MonoBehaviour {

        public Text coinText;
        public GameObject upgradeCardsPanel;
        public GameObject Loading;
        public Image sliderBar;
        public GameObject _gDailyRewards,_gChangeName;
        public GameObject _rateusDialogue;

        public void Start()
        {
        
            UpdateCoins();

            if (PlayerPrefs.GetInt("firstTimeReward", 0) == 0)
            {
                StartCoroutine(ShowReward());
            }
        }

        private IEnumerator ShowReward()
        {
            bool done = false;
            while (!done)
            {
                yield return new WaitForSeconds(0.5f);
                if (!_gChangeName.activeSelf)
                {
                    _gDailyRewards.SetActive(true);
                    done = true;
                    PlayerPrefs.SetInt("firstTimeReward", 1);
                    PlayerPrefs.Save();
                }
            }
        }

        public void StartBattle()
        {
            Loading.SetActive(true);
            BGMAB.instanceE.BtnClickSfxX();
            if (PlayerPrefs.GetInt("menuInter", 0) > 0)
            {
                // FBAdsManager.instance.ShowInterstitial();
            }
            else
            {
                PlayerPrefs.SetInt("menuInter", 1);
                PlayerPrefs.Save();
            }

            StartCoroutine(LoadNewScene(2));
        }

        // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
        IEnumerator LoadNewScene(int sceneToLoad)
        {

            Loading.SetActive(true);
            sliderBar.fillAmount = 0;
            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
            yield return new WaitForSeconds(0.2f);
            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone)
            {
                float progress = async.progress / 10000f;
                Debug.Log(progress);
                sliderBar.fillAmount = progress;


                yield return null;

            }

        }

        public void UpdateCoins()
        {
            coinText.text = "" + PlayerPrefs.GetInt("Coins", 0);
        }

        public void UpgradeCards()
        {

            int r = UnityEngine.Random.Range(0, 2);
            if (r % 2 == 0)
            {

                // FBAdsManager.instance.ShowInterstitial();

            }
        

            BGMAB.instanceE.BtnClickSfxX();
            upgradeCardsPanel.SetActive(true);
        }

        public void AllCharacters()
        {
            int r = UnityEngine.Random.Range(0,2);
            if (r % 2 == 0)
            {

                // AdsManager.instance.ShowInter();

            }
            else
                //AdsManager.instance.ShowCB();



                Loading.SetActive(true);
            BGMAB.instanceE.BtnClickSfxX();
            StartCoroutine(LoadNewScene(4));
        }

        public void RateUs()
        {
            _rateusDialogue.SetActive(true);
            BGMAB.instanceE.BtnClickSfxX();
        }
        public void MoreGames()
        {
            Application.OpenURL("https://play.google.com/store/apps/developer?id=300+Battle+Games");
            BGMAB.instanceE.BtnClickSfxX();
        }
    }
}
