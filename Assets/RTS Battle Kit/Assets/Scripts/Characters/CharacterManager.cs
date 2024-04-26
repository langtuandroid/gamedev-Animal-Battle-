using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//troop/unit settings
[System.Serializable]
public class Troop{
	public GameObject deployableTroops;
	public int troopCosts;
	public Sprite buttonImage;
	[HideInInspector]
	public Button button;
}

public class CharacterManager : MonoBehaviour {

	//variables visible in the inspector	
	public int startGold;
	public GUIStyle rectangleStyle;
	public ParticleSystem newUnitEffect;
	public Texture2D cursorTexture;
	public Texture2D cursorTexture1;
	public bool highlightSelectedButton;
	public Color buttonHighlight;
	public Button button;
	public float bombLoadingSpeed;
	public float BombRange;
	public GameObject bombExplosion;
	[Space(10)]
	
	public List<Troop> troops;
	
	//variables not visible in the inspector
	public static Vector3 clickedPos;
	public static int gold;
	public static GameObject target;
	
	private Vector2 mouseDownPos;
    private Vector2 mouseLastPos;
	private bool visible; 
    private bool isDown;
	private GameObject[] knights;
	private int selectedUnit = -1;
	public GameObject goldText;
	private GameObject goldWarning;
	private GameObject addedGoldText;
	public GameObject characterList;
	private GameObject characterParent;
	private GameObject selectButton;
	
	private GameObject bombLoadingBar;
	private GameObject bombButton;
	private float bombProgress;
	private bool isPlacingBomb;
	private GameObject bombRange;
	
	public static bool selectionMode;
    public CharacterBtnAB[] Deck_cards;

    public GameObject startBtn;

    int[] DeckID;
    public bool bUnityEditor = false;

    void Awake(){
        //find some objects
        troops.Clear();

        for (int i = 0; i < GameCharactersAB.instanceE.playerCharacters.Length; i++)
        {
            Troop ttt = new Troop();
            ttt.troopCosts = GameCharactersAB.instanceE.playerCharacters[i].GetComponent<StatsAB>().price;
            ttt.deployableTroops = GameCharactersAB.instanceE.playerCharacters[i];
            ttt.buttonImage = GameCharactersAB.instanceE.playerCharacters[i].GetComponent<StatsAB>().avatarR;
               troops.Add(ttt);
                
         }
        
        characterParent = new GameObject("Characters");	
		//selectButton = GameObject.Find("Character selection button");
		//target = GameObject.Find("target");
		//target.SetActive(false);
	}
	
	void Start(){
		//set cursor and add the character buttons
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		characterList = GameObject.Find("Character buttons");
        characterList.SetActive(false);
		addCharacterButtons();
		//set gold amount to start gold amount
		gold = startGold;
		//find text that displays your amount of gold
		goldText = GameObject.Find("gold text");
		//find text that appears when you get extra gold and set it not active
		//addedGoldText = GameObject.Find("added gold text");
		//addedGoldText.SetActive(false);	
		//find warning that appears when you don't have enough gold to deploy troops and set it not active
		goldWarning = GameObject.Find("gold warning");
		goldWarning.SetActive(false);
		//play function addGold every five seconds
		//InvokeRepeating("AddGold", 1.0f, 5.0f);
		
		//find bomb gameobjects
		
	}
     
    void Update(){
        
		
		//set gold text to gold amount
		goldText.GetComponent<UnityEngine.UI.Text>().text = "" + gold;
        if (selectedUnit < 0)
            return;
        //ray from main camera
        RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
				
		//check if left mouse button gets pressed
        if(Input.GetMouseButtonDown(0)){
                if (Manager.canStart)
                {
                    return;
                }
			//if you didn't click on UI and you have not enought gold, display a warning
			if(gold < troops[selectedUnit].troopCosts && !EventSystem.current.IsPointerOverGameObject()){
			StartCoroutine(GoldWarning());	
			}
			//check if you hit any collider when clicking (just to prevent errors)
			if(hit.collider != null){
                    //if you click battle ground, if click doesn't hit any UI, if space is not down and if you have enough gold, deploy the selected troops and decrease gold amount
                    if (hit.collider.gameObject.CompareTag("Knight"))
                    {
                        gold += troops[hit.collider.gameObject.GetComponent<StatsAB>().idD].troopCosts;
                        Destroy(hit.collider.gameObject);
                        return;
                    }

                    
                    if (!bUnityEditor)
                    {
                        if (hit.collider.gameObject.CompareTag("Battle ground") && !selectionMode && !isPlacingBomb && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
                            && gold >= troops[selectedUnit].troopCosts)
                        {
                            GameObject newTroop = Instantiate(troops[selectedUnit].deployableTroops, hit.point, troops[selectedUnit].deployableTroops.transform.rotation) as GameObject;
                            Instantiate(newUnitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                            newTroop.transform.parent = characterParent.transform;
                            gold -= troops[selectedUnit].troopCosts;
                            if(!startBtn.activeSelf)
                                startBtn.SetActive(true); 
                        }
                    }
                    else
                    {
                        if (hit.collider.gameObject.CompareTag("Battle ground") && !selectionMode && !isPlacingBomb && !EventSystem.current.IsPointerOverGameObject()
                           && gold >= troops[selectedUnit].troopCosts)
                        {
                            GameObject newTroop = Instantiate(troops[selectedUnit].deployableTroops, hit.point, troops[selectedUnit].deployableTroops.transform.rotation) as GameObject;
                            Instantiate(newUnitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                            newTroop.transform.parent = characterParent.transform;
                            gold -= troops[selectedUnit].troopCosts;
                            if (!startBtn.activeSelf)
                                startBtn.SetActive(true);
                        }
                    }
                    //if you are placing a bomb and click...
                    if (isPlacingBomb && !EventSystem.current.IsPointerOverGameObject()){
				//instantiate explosion
				Instantiate(bombExplosion, hit.point, Quaternion.identity);
				
				//reset bomb progress
				bombProgress = 0;
				isPlacingBomb = false;
				bombRange.SetActive(false);
				
				//find enemies
				GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

					foreach(GameObject enemy in enemies){
					if(enemy != null && Vector3.Distance(enemy.transform.position, hit.point) <= BombRange/2){
					//kill enemy if its within the bombrange
					enemy.GetComponent<Character>().lives = 0;	
					}
					}
			}
			else if(hit.collider.gameObject.CompareTag("Battle ground") && isPlacingBomb && EventSystem.current.IsPointerOverGameObject()){
				//if you place a bomb and click any UI element, continue but don't reset bomb progress
				isPlacingBomb = false;
				bombRange.SetActive(false);
			}
			}
        }
		
		if(hit.collider != null && isPlacingBomb && !EventSystem.current.IsPointerOverGameObject()){
			//show the bomb range at mouse position using a spot light
			bombRange.transform.position = new Vector3(hit.point.x, 75, hit.point.z);
			//adjust spotangle to correspond to bomb range
			bombRange.GetComponent<Light>().spotAngle = BombRange;
			bombRange.SetActive(true);
		}
		
		//if space is down too set the position where you first clicked
		if(Input.GetMouseButtonDown(0) && selectionMode && !isPlacingBomb 
		&& (!GameObject.Find("Mobile") || (GameObject.Find("Mobile") && !Mobile.selectionModeMove))){
		mouseDownPos = Input.mousePosition;
        isDown = true;
        visible = true;
		}
 
        // Continue tracking mouse position until mouse button is up
        if(isDown){
        mouseLastPos = Input.mousePosition;
			//if you release mouse button, remove rectangle and stop tracking
            if(Input.GetMouseButtonUp(0)){
                isDown = false;
                visible = false;
            }
        }
		
		//find and store all selectable objects (objects in scene with knight tag)
		knights = GameObject.FindGameObjectsWithTag("Knight");
		
		//if player presses d, deselect all characters
		if(Input.GetKey("x")){
		foreach(GameObject Knight in knights){
		if(Knight != null){
		Knight.GetComponent<Character>().selected = false;	
		}
		}
		}
		
		//start selection mode when player presses spacebar
		if(Input.GetKeyDown("space")){
		selectCharacters();	
		}
		
		//for each button, check if we have enough gold to deploy the unit and color the button grey if it can not be deployed yet
		for(int i = 0; i < troops.Count; i++){
			if(troops[i].troopCosts <= gold){
				//troops[i].button.gameObject.GetComponent<Image>().color = Color.white;
			}
			else{
			//	troops[i].button.gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
			}
		}
    }
 
    void OnGUI(){
		//check if rectangle should be visible
		if(visible){
            // Find the corner of the box
            Vector2 origin;
            origin.x = Mathf.Min(mouseDownPos.x, mouseLastPos.x);
			
            // GUI and mouse coordinates are the opposite way around.
            origin.y = Mathf.Max(mouseDownPos.y, mouseLastPos.y);
            origin.y = Screen.height - origin.y;  
			
            //Compute size of box
            Vector2 size = mouseDownPos - mouseLastPos;
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);   
			
            // Draw the GUI box
            Rect rect = new Rect(origin.x, origin.y, size.x, size.y);
            GUI.Box(rect, "", rectangleStyle);
			
			foreach(GameObject Knight in knights){
			if(Knight != null){
			Vector3 pos = Camera.main.WorldToScreenPoint(Knight.transform.position);
			pos.y = Screen.height - pos.y;
			//foreach selectable character check its position and if it is within GUI rectangle, set selected to true
			if(rect.Contains(pos)){
			Knight.GetComponent<Character>().selected = true;
			}
			}
			}
		}
    }
	
	//function to select another unit
	public void selectUnit(int unit){
	
	    //selected unit is the pressed button
	    selectedUnit = DeckID[unit];
        HighlightBtn();

    }
    public Color highlightColor,baseColor;
    void HighlightBtn()
    {

        for (int i = 0; i<Deck_cards.Length; i++)
        {
            if (selectedUnit == DeckID[i])
            {
                Deck_cards[i].GetComponent<Outline>().effectColor = highlightColor;
                Deck_cards[i].statsObjj.SetActive(true);
            }
            else
            {
                Deck_cards[i].GetComponent<Outline>().effectColor = baseColor;
                Deck_cards[i].statsObjj.SetActive(false);
            }
        }

    }

    public void selectCharacters(){
		//turn selection mode on/off
		selectionMode = !selectionMode;
		if(selectionMode){
		//set cursor and button color to show the player selection mode is active
		selectButton.GetComponent<Image>().color = Color.red;	
		Cursor.SetCursor(cursorTexture1, Vector2.zero, CursorMode.Auto);
		if(GameObject.Find("Mobile")){
			if(Mobile.deployMode){
				GameObject.Find("Mobile").GetComponent<Mobile>().toggleDeployMode();
			}
			Mobile.camEnabled = false;
		}
		}
		else{
		//show the player selection mode is not active
		selectButton.GetComponent<Image>().color = Color.white;	
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		
		//set target object false and deselect all units
		foreach(GameObject Knight in knights){
		if(Knight != null){
		Knight.GetComponent<Character>().selected = false;	
		}
		}
		target.SetActive(false);
		if(GameObject.Find("Mobile")){
			Mobile.camEnabled = true;
		}
		}
	}
	
	//warning if you need more gold
	IEnumerator GoldWarning(){
	if(!goldWarning.activeSelf){
	goldWarning.SetActive(true);
	
	//wait for 2 seconds
	yield return new WaitForSeconds(2);
	goldWarning.SetActive(false);
	}
	}
   

    void addCharacterButtons(){
        //for all troops...
            DeckID = new int[Deck_cards.Length];

       // StartCoroutine(UpdateCardsDelay());
       
	}
    public IEnumerator UpdateCardsDelay()
    {
        
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < Deck_cards.Length; i++)
        {
            DeckID[i] = PlayerPrefs.GetInt("DeckCard" + i, i);
            Deck_cards[i].priceE = troops[DeckID[i]].troopCosts;
            Deck_cards[i].spP = troops[DeckID[i]].buttonImage;
            Deck_cards[i].levelL = PlayerPrefs.GetInt("P" + DeckID[i], 1);
            CopyStats(Deck_cards[i].mstatT, troops[DeckID[i]].deployableTroops.GetComponent<StatsAB>());
            Deck_cards[i].UpdateInfo();

        }
    }
    void CopyStats(StatsAB a, StatsAB b)
    {
        
        //a.level = b.level;
        a.Name = b.Name;
        a.bHealth = b.bHealth;
        a.bDmg = b.bDmg;
        a.bRange = b.bRange;
        a.bMovSpeed = b.bMovSpeed;
        a.idD = b.idD;
        a.isPlayerR = true;

        a.UpdateInfoO();

    }

    public void placeBomb(){
		//start placing a bomb
		isPlacingBomb = true;
	}
	
	//functions which adds 100 to your gold amount and shows text to let player know
	void AddGold(){
	//gold += 100;
	//StartCoroutine(AddedGoldText());
	}
	
	IEnumerator AddedGoldText(){
	addedGoldText.SetActive(true);	
	yield return new WaitForSeconds(0.7f);
	addedGoldText.SetActive(false);	
	}
}