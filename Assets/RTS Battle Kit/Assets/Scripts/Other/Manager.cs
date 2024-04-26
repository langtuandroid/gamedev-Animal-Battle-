using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using This_Game.Scripts;

public class Manager : MonoBehaviour {
	
	[Header("Mission")]
	public bool destroyCastle;
	public int killEnemies;
	public int timeSeconds;
	
	[Space(10)] 
	
	//variables visible in the inspector
	public float fadespeed;
	public GUIStyle minimap;
	public GameObject Minimap;
	
	//variables not visible in the inspector
	public GameObject GameOverMenu;
	public GameObject VictoryMenu;
	private GameObject GamePanel;
	private GameObject characterButtons;
	
	private bool fading;
	private float alpha;
	private bool showMinimap;
	
	private GameObject showHideUnitsButton;
	private GameObject showHideMinimapButton;
	private GameObject unitsLabel;
	
	//private GameObject playerCastleStrengthText;
	//private GameObject enemyCastleStrengthText;
	private GameObject playerCastleStrengthBar;
	//private GameObject enemyCastleStrengthBar;
	
	private GameObject missionPanel;
	
	private float playerCastleStrengthStart;
	private float enemyCastleStrengthStart;
	
	private float playerCastleStrength;
	private float enemyCastleStrength;
	
	public static int enemiesKilled;
    public static int playersKilled;
    private float time;

    public Text enemiesKilledText;
    public Text playersKilledText;

    public static bool gameOver;
	public static bool victory;
	public static GameObject StartMenu;
    public GameObject SpawnArea;
    public static bool canStart = false;

    public Text timerText;
    float t = 0;

    public GameObject[] Levels;

    public GameObject RewardsPanel;
    public GameObject[] trophies;
    public Text xprewardText, coinsrewardText;
    int xpreward, coinsreward;
    public GameObject CamJoystick,blendlist;
    public CinemachineVirtualCamera vCam;

    public Image playerHealthBar, enemyHealthBar;
    public GameObject cam, pausecam;
    public Text LevelnumText;
    public GameObject RateUsDialogue;
    public RewardBoxAB _rewardBox;
    public GameObject VictoryBtns;
    public GameObject _FadeBox;
    public GameObject _Tutorial,_tutorialFade;

	private void Start(){
		//find some objects
		characterButtons = GameObject.Find("Character panel");
		
		playerCastleStrengthBar = GameObject.Find("Player castle strength bar");
        //enemyCastleStrengthBar = GameObject.Find("Enemy castle strength bar");
        int r = LevelManager.LevelId;

        LevelnumText.text = ""+(r+1);

        Levels[r].SetActive(true);
        killEnemies = Levels[r].GetComponent<LevelAB>().targetKills;
        int _levelGold = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            _levelGold += g.GetComponent<StatsAB>().price;
        }
       CharacterManager.gold = _levelGold;

        xpreward = Mathf.Clamp(20 + (r + 1) * 5 * (r + 1), 0, 300);

        if (r > 39)
            coinsreward = Mathf.Clamp(100 + (r + 1) * 100 * (r + 1), 0, 25000);
        else if (r > 29)
            coinsreward = Mathf.Clamp(100 + (r + 1) * 100 * (r + 1), 0, 20000);
        else if (r > 19)
            coinsreward = Mathf.Clamp(100 + (r + 1) * 100 * (r + 1), 0, 15000);
        else if (r > 9)
            coinsreward = Mathf.Clamp(100 + (r + 1) * 100 * (r + 1), 0, 10000);
        else
            coinsreward = Mathf.Clamp(100 + (r + 1) * 100 * (r + 1), 0, 5000);

       

        

        //immediately freeze game
        Time.timeScale = 1;
		
		//find all UI panels
		//StartMenu = GameObject.Find("Start panel");
		
		GamePanel = GameObject.Find("Game panel");

        enemiesKilled = 0; playersKilled = 0;
		//turn off all panels except the start menu panel
		//StartMenu.SetActive(true);
		
       
		
		//game over and victory are false
		gameOver = false;
		victory = false;
        canStart = false;
        //start menu alpha is alpa
        //alpha = StartMenu.GetComponent<Image>().color.a;

        //get strength of all castles together
        GetCastleStrength();
		
		//set start castle strengths
		playerCastleStrengthStart = playerCastleStrength;
		enemyCastleStrengthStart = enemyCastleStrength;
		
		missionPanel = GameObject.Find("Mission");
		string mission = "";
		
		if(destroyCastle){
		mission = "Destroy castle of the opponent";
		}
		else if(killEnemies > 0 && timeSeconds > 0){
		mission = "- Kill " + killEnemies + " enemies. \n- Battle at least " + timeSeconds + " seconds.";
		}
		else if(killEnemies > 0 && timeSeconds <= 0){
		mission = "Kill " + killEnemies + " enemies.";
		}
		else if(killEnemies <= 0 && timeSeconds > 0){
		mission = "Battle at least " + timeSeconds + " seconds.";
		}
		else{
		mission = "No mission";	
		}
        StartCoroutine(ActivateCamera());
		//missionPanel.transform.Find("Mission text").gameObject.GetComponent<Text>().text = mission;
		//missionPanel.SetActive(false);
		time = 0;
	}
    private IEnumerator ActivateCamera()
    {
        blendlist.SetActive(true);
        CamJoystick.SetActive(false);
        yield return new WaitForSeconds(1.75f);
        _FadeBox.SetActive(false);
        yield return new WaitForSeconds(5.25f);
        vCam.m_Priority = 20;
        blendlist.SetActive(false);
        CamJoystick.SetActive(true);
        this.GetComponent<CharacterManager>().characterList.SetActive(true);
        _tutorialFade.SetActive(true);
        _Tutorial.SetActive(true);
        StartCoroutine(this.GetComponent<CharacterManager>().UpdateCardsDelay());
    }

	private void Update(){
		//keep updating castle strengths
		GetCastleStrength();
        UpdateTime();
        time += Time.deltaTime;
		
		//set game over true when the castles are destroyed
		if(playerCastleStrength <= 0){
			//Time.timeScale = 0;
			gameOver = true;
           
		}
		
        //set victory based on mission
		if(enemyCastleStrength <= 0 && destroyCastle){
			//Time.timeScale = 0;
			victory = true;
		}

		else if(killEnemies > 0 && enemiesKilled >= killEnemies && timeSeconds > 0 && timeSeconds < time){
			//Time.timeScale = 0;
			//victory = true;
		}
		else if((timeSeconds > 0 && timeSeconds < time) && !(killEnemies > 0)){
			//Time.timeScale = 0;
			//victory = true;
		}
		else if((killEnemies > 0 && enemiesKilled == killEnemies) && !(timeSeconds > 0)){
			//Time.timeScale = 0;
			//victory = true;
		}
		if(gameOver && !GameOverMenu.activeSelf){
            StartCoroutine(DoGameOver());
		}
		if(victory && !VictoryMenu.activeSelf && !_winning){

            StartCoroutine(DoGameWin());
            
		}
		
		if(fading && alpha > 0){
			alpha -= Time.deltaTime * fadespeed;
		}
		else if(alpha <= 0){
			fading = false;
		}
		
		if(showMinimap && !StartMenu.activeSelf && !Settings.settingsMenu.activeSelf && !GameOverMenu.activeSelf && !VictoryMenu.activeSelf){
			Minimap.SetActive(true);
		}
		else{
			Minimap.SetActive(false);	
		}
		
		//get the amount of units and the amount of selected units
		int playerUnits = GameObject.FindGameObjectsWithTag("Knight").Length;
		int enemyUnits = GameObject.FindGameObjectsWithTag("Enemy").Length;
		int selectedUnits = 0;
       float totalUnits = playerUnits + enemyUnits;
        playerCastleStrengthBar.GetComponent<Image>().fillAmount = playerUnits / totalUnits;

        if (!gameOver && canStart && !victory)
        {
            if (playerUnits <= 0 || enemyUnits <=0)
            {
                if (!pausecam.activeSelf)
                {
                    pausecam.SetActive(true);
                    cam.SetActive(false);
                    CamJoystick.SetActive(false);
                    this.GetComponent<CharacterManager>().characterList.SetActive(false);
                }
            }
        }
		//add 1 to selected units for each selected unit
		foreach(GameObject unit in GameObject.FindGameObjectsWithTag("Knight")){
			if(unit.GetComponent<Character>().selected){
			selectedUnits++;	
			}
		}
		
        enemiesKilledText.text = ""+ enemiesKilled;
        playersKilledText.text = "" + playersKilled;
	}
	
	void OnGUI(){
		//show minimap border when minimap is active
		if(showMinimap && !GameOverMenu.activeSelf && !VictoryMenu.activeSelf){
		GUI.Box(new Rect(Screen.width * 0.795f, Screen.height * 0.69f, Screen.width * 0.205f, Screen.height * 0.31f), "", minimap);
		}
	}

    void GetCastleStrength() {
        //castle strength is 0
        playerCastleStrength = 0;
        enemyCastleStrength = 0;

        //add the strength of each enemy castle
        foreach (GameObject enemyCastle in GameObject.FindGameObjectsWithTag("Enemy Castle")) {
            if (enemyCastle.GetComponent<Castle>().lives > 0) {
                enemyCastleStrength += enemyCastle.GetComponent<Castle>().lives;
            }
        }
        float x = enemyCastleStrength / 500;
        enemyHealthBar.fillAmount = x;
        //add the strength of each player castle
        foreach (GameObject playerCastle in GameObject.FindGameObjectsWithTag("Player Castle")) {
            if (playerCastle.GetComponent<Castle>().lives > 0) {
                playerCastleStrength += playerCastle.GetComponent<Castle>().lives;
            }
        }
        x = playerCastleStrength / 500;
        playerHealthBar.fillAmount = x;


        if (playerCastleStrength <= 0)
        {
            gameOver = true;
           
        }
        if (enemyCastleStrength <= 0)
        {
            victory = true;
            
        }

	}

    IEnumerator DoGameOver()
    {
        yield return new WaitForSeconds(1f);
        //AdsManager.instance.CloseBanner();
        int tCash = PlayerPrefs.GetInt("Coins", 500);
        PlayerPrefs.SetInt("Coins", tCash + 50);

        int tXp = PlayerPrefs.GetInt("Xp", 0);
        PlayerPrefs.SetInt("Xp", tXp + 10);
        PlayerPrefs.Save();

        

        GamePanel.SetActive(false);
        GameOverMenu.SetActive(true);
        RewardsPanel.SetActive(true);

        int playerUnits = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _starsValue = Mathf.Clamp(playerUnits, 0, 3);
        _starsValue = (_starsValue - (_starsValue - 1));
        int best = PlayerPrefs.GetInt("levelStars" + LevelManager.LevelId, 0);


        if (_starsValue > best)
        {
            PlayerPrefs.SetInt("levelStars" + LevelManager.LevelId, _starsValue);
            PlayerPrefs.Save();
        }

        for (int i = 0; i < _starsValue; i++)
        {
            yield return new WaitForSeconds(0.5f);
            trophies[i].SetActive(true);
        }

        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForEndOfFrame();
            coinsrewardText.text = "" + i;
        }

        for (int i = 0; i <= 10; i++)
        {
            yield return new WaitForEndOfFrame();
            xprewardText.text = "" + i;
        }

    }
    private bool _winning = false;
    private int _starsValue = 0;

    private IEnumerator DoGameWin()
    {
        _winning = true;
        BGMAB.instanceE.PlayVictoryMusicC();
        int x = PlayerPrefs.GetInt("VictoryChests",0);

        int victoryCount = PlayerPrefs.GetInt("Victories", 0);
        PlayerPrefs.SetInt("Victories", victoryCount + 1);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("VictoryChests", x+1);
        
        int tCash = PlayerPrefs.GetInt("Coins", 500);
        PlayerPrefs.SetInt("Coins", tCash + coinsreward);
        
        int tXp = PlayerPrefs.GetInt("Xp", 0);
        PlayerPrefs.SetInt("Xp", tXp + xpreward);
        PlayerPrefs.Save();

        int ulevel = PlayerPrefs.GetInt("ulevels", 1);
        
        if (LevelManager.LevelId + 2 > ulevel)
        {
            PlayerPrefs.SetInt("ulevels", LevelManager.LevelId+2);
        }
        GamePanel.SetActive(false);
        VictoryMenu.SetActive(true);

        if (PlayerPrefs.GetInt("levelRewarded" + LevelManager.LevelId, 0) == 0)
        {
            PlayerPrefs.SetInt("levelRewarded" + LevelManager.LevelId, 1);
            PlayerPrefs.Save();
            int cardId = 0;
            Xpoint: cardId = Random.Range(0, GameCharactersAB.instanceE.playerCharacters.Length);
            bool isvalidCardID = PlayerPrefs.GetInt("uCard" + cardId, 0) == 1 ? true : false;

            if (!isvalidCardID && cardId > 7)
                goto Xpoint;

            _rewardBox.cardIdD = cardId;
            _rewardBox.numCardsS = xpreward;
            _rewardBox.gameObject.SetActive(true);

            while (!_rewardBox.rewarded)
            {
                yield return new WaitForSeconds(0.1f);
            }
            _rewardBox.gameObject.SetActive(false);
        }

        RewardsPanel.SetActive(true);
        int playerUnits = GameObject.FindGameObjectsWithTag("Knight").Length;
        _starsValue = Mathf.Clamp(playerUnits,0,3);

        int best = PlayerPrefs.GetInt("levelStars" + LevelManager.LevelId, 0);

        if (_starsValue > best)
        {
            PlayerPrefs.SetInt("levelStars" + LevelManager.LevelId, _starsValue);
            PlayerPrefs.Save();
        }

        for (int i = 0; i < _starsValue; i++)
        {
            yield return new WaitForSeconds(0.5f);
            trophies[i].SetActive(true);
        }

        for (int i = 0,j=0; i < coinsreward; )
        {
            if (coinsreward - i > 2000)
            {
                i += 500;
                yield return new WaitForSeconds(0.03f);
            }
            else if (coinsreward - i > 1000)
            {
                i += 200;
                yield return new WaitForSeconds(0.03f);
            }
            else if (coinsreward - i > 500)
            {
                i += 100;
                yield return new WaitForSeconds(0.03f);
            }
            else if (coinsreward - i > 100)
            {
                i += 50;
                yield return new WaitForSeconds(0.02f);

            }
            else if (coinsreward - i > 10)
            {
                i += 10;
                yield return new WaitForSeconds(0.01f);

            }
            else
            {
                i += 1;

                yield return new WaitForSeconds(0.01f);
            }

            if(j < xpreward)
                if (xpreward - j > 20)
                {
                    j += 5;
                }
                else if (xpreward - j > 10)
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }

            coinsrewardText.text = "" + i;
            xprewardText.text = "" + j;
        }
        
        int starRating = PlayerPrefs.GetInt("StarRating", 0);
        if (starRating < 4)
        {
           if(Random.Range(0,2)!=0 || LevelManager.LevelId<=0)
            {
                RateUsDialogue.SetActive(true);
            }
        }

        VictoryBtns.SetActive(true);
    }
    
	//start the game
	public void startGame(){

        //set buttons false
        SpawnArea.SetActive(false);
        //AdsManager.instance.ShowBanner();


		foreach (Transform button in StartMenu.transform){
		button.gameObject.SetActive(false);
		}
		//set timescale to normal and start fading out
		Time.timeScale = 1;
		fading = true;
	}
	
	//open mission panel to start game
	public void openMissionPanel(){
		missionPanel.SetActive(true);
	}
    public GameObject pauseDialogue;
    public void PauseGame()
    {
        vCam.gameObject.SetActive(false);
        pausecam.SetActive(true);
        cam.SetActive(false);
        Time.timeScale = 0;
        pauseDialogue.SetActive(true);

        if (PlayerPrefs.GetInt("PauseBtnAd", 0) > 0)
        {
            int r = Random.Range(0,2);

            if (r % 2 == 0)
            {

                //AdsManager.instance.ShowInter();

            }
            else
            {
	            //AdsManager.instance.ShowCB();
            }
        }
        else
        {
            PlayerPrefs.SetInt("PauseBtnAd", 1);
            PlayerPrefs.Save();
        }
    }

    public void ResumeGame()
    {
        vCam.gameObject.SetActive(true);
        pausecam.SetActive(false);
        cam.SetActive(true);
        Time.timeScale = 1;
        pauseDialogue.SetActive(false);
    }

	public void endGame(){
		//end game
		Application.Quit();
	}
	
	public void surrender(){
		//Freeze game and set the game over panel visible
		Time.timeScale = 1;
		Manager.gameOver = true;
	}
	public void showHideUnits(){
		//show or hide the units panel
		characterButtons.SetActive(!characterButtons.activeSelf);
		//change button text
		if(characterButtons.activeSelf){
		showHideUnitsButton.GetComponentInChildren<Text>().text =	"-";
		}
		else{
		showHideUnitsButton.GetComponentInChildren<Text>().text =	"+";	
		}
	}
	
	public void showHideMinimap(){
		//show or hide minimap
		showMinimap = !showMinimap;
		
		//change button text
		if(showMinimap){
		showHideMinimapButton.GetComponentInChildren<Text>().text =	"-";
		}
		else{
		showHideMinimapButton.GetComponentInChildren<Text>().text =	"+";	
		}
	}

    public void StartFight()
    {
        Time.timeScale = 1;
        canStart = true;
//        AdsManager.instance.ShowBanner();
	    BGMAB.instanceE.BtnClickSfxX();
    }

    public void Restart()
    {
	    Time.timeScale = 1;
       // AdsManager.instance.CloseBanner();
        int r = Random.Range(0, 2);

        if (r % 2 == 0)
        {

          //  AdsManager.instance.ShowInter();

        }

        BGMAB.instanceE.BtnClickSfxX();
        SceneManager.LoadScene(3);
    }

    public void Next()
    {
        Time.timeScale = 1;
      //  AdsManager.instance.CloseBanner();

		BGMAB.instanceE.BtnClickSfxX();
        LevelManager.LevelId += 1;
        SceneManager.LoadScene(3);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
//        AdsManager.instance.CloseBanner();
        int r = Random.Range(0, 2);

        
    //   AdsManager.instance.ShowUnity();

		BGMAB.instanceE.BtnClickSfxX();
        SceneManager.LoadScene(1);
    }

    void UpdateTime()
    {

        if (!canStart || victory || gameOver)
            return;

        t = t + Time.deltaTime;
       
        timerText.text = "";
        //Debug.Log(t);
        int m = (int)t / 60;
        int s = (int)t % 60;
        if (m > 9)
            timerText.text += "" + m;
        else
            timerText.text += "0" + m;

        timerText.text += " : ";


        if (s > 9)
            timerText.text += "" + s;
        else
            timerText.text += "0" + s;
    }
}
