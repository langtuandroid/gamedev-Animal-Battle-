using UnityEngine;
using System.Collections;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CharacterMultiplayer : NetworkBehaviour {

	//variables visible in the inspector
	public float lives;
	public float damage;
	public float minAttackDistance;
	public float castleStoppingDistance;
	public GameObject dieParticles;
	public string unitType;
	public GameObject healthBarFill;
	
	//variables not visible in the inspector
	[HideInInspector]
	public string attackTag = "Untagged";
	[HideInInspector]
	public string attackCastleTag = "Untagged";
	
	[HideInInspector]
	public bool selected;
	[HideInInspector]
	public Transform currentTarget;
	[HideInInspector]
	public Vector3 castleAttackPosition;
	
	private UnityEngine.AI.NavMeshAgent agent;
	private GameObject[] enemies;
	private GameObject health;
	private GameObject healthbar;
	private GameObject selectedObject;
	private float defaultStoppingDistance;
	private GameObject castle;
	private Animator animator;
	
	//SyncVars
	[HideInInspector]
	public NetworkVariable<Vector3> clickedPosition;
	
	[HideInInspector]
	public NetworkVariable<Vector3> currentTargetPosition;
	
	[HideInInspector]
	public NetworkVariable<bool> goingToClickedPos = new NetworkVariable<bool>();
	
	[HideInInspector]
	public int destination;
	
	void Start(){
	//find the animator component
	animator = gameObject.GetComponent<Animator>();
	
	//character is not selected
	selected = false;
	//selected character is not moving to clicked position
	goingToClickedPos = new NetworkVariable<bool>(false);
	//find navmesh agent component
	agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
	
	//find objects attached to this character
	health = transform.Find("Health").gameObject;
	healthbar = health.transform.Find("Healthbar").gameObject;
	selectedObject = transform.Find("selected object").gameObject;
	selectedObject.SetActive(false);
	
	//set healtbar value
	healthbar.GetComponent<Slider>().maxValue = lives;
	//get default stopping distance
	defaultStoppingDistance = agent.stoppingDistance;
	}
	
	void Update(){
		
		//rotate healtbar towards camera and assign the correct amount of lives
		health.transform.LookAt(2 * transform.position - Camera.main.transform.position);
		healthbar.GetComponent<Slider>().value = lives;
		
		//if this character is a knight, set healthtext color based on selected/not selected
		if(selected){
			selectedObject.SetActive(true);
		}
		else{
			selectedObject.SetActive(false);
		}
		
		//if destination is 1, it means that this character should attack the nearest enemy
		if(destination == 1 && currentTarget != null && !goingToClickedPos.Value){
			//move agent towards enemy
			agent.SetDestination(currentTargetPosition.Value); 
			
			//if the the distance is small enough:
			if(Vector3.Distance(currentTargetPosition.Value, transform.position) <= agent.stoppingDistance){
				
				//look at the enemy
				Vector3 currentTargetPosition = currentTarget.position;
				currentTargetPosition.y = transform.position.y;
				transform.LookAt(currentTargetPosition);	
				
				//play attack animation
				animator.SetBool("Attacking", true);
				
				//apply damage to the enemy
				currentTarget.gameObject.GetComponent<CharacterMultiplayer>().lives -= Time.deltaTime * damage;
			}
		}
		//if destination is 2, it means that this character should approach the nearest castle
		else if(destination == 2){
			//move agent towards the castle
			agent.SetDestination(castleAttackPosition);
			agent.Resume();
			
			//play running animation [attacking = false] -> [running = true]
			animator.SetBool("Attacking", false);
		}
		//if destination is 3, it means that this character should move towards the clicked position on the battle ground
		else if(destination == 3){
			agent.SetDestination(clickedPosition.Value);
			
			//play running animation
			animator.SetBool("Attacking", false);
		}
		//if destination is 4, this character has reached the target castle
		else if(destination == 4){
			if(castle != null){
			
			//make sure we're really close enough to the castle to attack it
			if(Vector3.Distance(transform.position, castleAttackPosition) <= castleStoppingDistance + castle.GetComponent<Castle>().size){
			//stop agent and play attack animation
			agent.Stop();
			animator.SetBool("Attacking", true);
			}
			
			//look at the castle
			Vector3 currentCastlePosition = castle.transform.position;
			currentCastlePosition.y = transform.position.y;
			transform.LookAt(currentCastlePosition);	
			
			//apply damage to the castle
			castle.GetComponent<Castle>().lives -= Time.deltaTime * damage;
			}
		}
		
		//check if a position was clicked on the battleground
		checkForClickedPosition();
		
		//from this point, only the server executes this code
		if(!IsServer)
			return;
		
		//find all potential targets (enemies of this character)
		enemies = GameObject.FindGameObjectsWithTag(attackTag);
		
		//distance between character and its nearest enemy
		float closestDistance = Mathf.Infinity;
		
		foreach(GameObject potentialTarget in enemies){
			//check if there are enemies left to attack and per enemy check if its closest to this character
			if(Vector3.Distance(transform.position, potentialTarget.transform.position) < closestDistance && potentialTarget != null){
				
			closestDistance = Vector3.Distance(transform.position, potentialTarget.transform.position);
				
				//check if we don't have a target enemy currently
				if(currentTarget == null){
					//if there is no enemy at the moment, tell the clients to set a new target using the enemy's network ID
					SetRpcSetCurrentTargetClientRpc(potentialTarget.GetComponent<NetworkObject>().NetworkObjectId);
				}
				
			}
		}
		
		//set currenttarget position
		if(currentTarget != null){
			currentTargetPosition.Value = currentTarget.position;
		}
		
		//find closest castle
		if(castle == null && GameObject.FindGameObjectsWithTag(attackCastleTag) != null){
			
			//find the castles that should be attacked by this character
			GameObject[] castles = GameObject.FindGameObjectsWithTag(attackCastleTag);
			
			GameObject lastCastle = castle;
		
			//distance between character and its nearest castle
			float closestCastle = Mathf.Infinity;
		
			foreach(GameObject potentialCastle in castles){
			//check if there are castles left to attack and check per castle if its closest to this character
				if(Vector3.Distance(transform.position, potentialCastle.transform.position) < closestCastle && potentialCastle != null){
				//if this castle is closest to character, set closest distance to distance between character and this castle
				closestCastle = Vector3.Distance(transform.position, potentialCastle.transform.position);
				//also set current target to closest target (this castle)
				castle = potentialCastle;
				}
			}	
		
			//Define a position to attack the castles(to spread characters when they are attacking the castle)
			if(castle != null && castle != lastCastle){
				SetRpcSetClosestCastleClientRpc(castle.transform.position, int.Parse(castle.name));
			}	
		}
		
		//first check if character is not selected and moving to a clicked position
		if(!goingToClickedPos.Value){
		//If there's a currentTarget and its within the attack range, move agent to currenttarget
		if(currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < minAttackDistance && destination != 4){
			
			//change agent destination on clients
			if(destination != 1){
				SetRpcChangeDestinationClientRpc(1);
			}
			
		//if its still traveling to the target, play running animation
		if(Vector3.Distance(currentTarget.position, transform.position) > agent.stoppingDistance){	
			
			//change animation on clients
			if(animator.GetBool("Attacking") == true){
				SetRpcSetAnimationClientRpc("Attacking", false);
			}
			
		}
		}
		//if currentTarget is out of range...
		else{
		
		//if character is close enough to castle, attack castle
		if(castle != null && Vector3.Distance(transform.position, castleAttackPosition) <= castleStoppingDistance + castle.GetComponent<Castle>().size){
			
			//change agent destination on clients
			if(destination != 4){
				SetRpcChangeDestinationClientRpc(4);	
			}
		}
		//if character is traveling to castle play running animation
		else{
			
			//change agent destination on clients
			if(destination != 2){
				SetRpcChangeDestinationClientRpc(2);	
			}
			
		}
		}
		}
		//if character is going to clicked position...
		else{
		//if character is close enough to clicked position, let it attack enemies again
		if(Vector3.Distance(transform.position, clickedPosition.Value) < agent.stoppingDistance + 1 || !CharacterManagerMultiplayer.selectionMode){
			
			//resume agent movement on clients
			if(agent.velocity == Vector3.zero){
				SetRpcResumeAgentClientRpc();
			}
			
		}
		}
		
		//if character ran out of lives add blood particles, add gold and destroy character
		if(lives < 1){
		Vector3 position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
		GameObject particles = Instantiate(dieParticles, position, transform.rotation) as GameObject;
		
		particles.GetComponent<NetworkObject>().Spawn();

		CharacterManagerMultiplayer[] managers = GameObject.FindObjectsOfType<CharacterManagerMultiplayer>();
		foreach(CharacterManagerMultiplayer manager in managers){
			if(manager.gameObject.transform.Find("Camera").gameObject.activeSelf){
				//find the local player manager and tell it to show a kill label for this killed character
				manager.enemyUnitKilled(gameObject.tag, unitType);
			}
		}
		
		//destroy this character
		Destroy(gameObject);
		}
	}
	
	void checkForClickedPosition(){
	RaycastHit hit;
	
	//if character is selected and right mouse button gets clicked...
    if(selected && ((Input.GetMouseButtonDown(1) && !GameObject.Find("Mobile")) || (Input.GetMouseButtonDown(0) && GameObject.Find("Mobile") && Mobile.selectionModeMove) 
	|| (Input.GetMouseButtonDown(0) && GameObject.Find("Mobile multiplayer") && MobileMultiplayer.selectionModeMove))){
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if(Physics.Raycast(ray, out hit))
		//if you clicked battle ground, move character to clicked point and play running animation
		if(hit.collider.gameObject.CompareTag("Battle ground")){
			
			//set the yellow target position
			CharacterManagerMultiplayer.target.transform.position = hit.point;
			CharacterManagerMultiplayer.target.SetActive(true);
			
			//move this character towards the clicked position
			if(IsServer){
				goToClickedPosition(hit.point);
			}
			else{
				GoToClickedPositionServerRpc(hit.point);
			}
		}
    }	
	}
	
	void goToClickedPosition(Vector3 position){
		//the character is moving to the clicked position:
		goingToClickedPos.Value = true;
		clickedPosition.Value = position;
		
		//change agent destination on clients
		SetRpcChangeDestinationClientRpc(3);
		 
		//change stopping distance (temporary) if this is an archer
		if(GetComponent<archer>() != null){
		agent.stoppingDistance = 2;
		}
		
		//change animation on clients
		if(animator.GetBool("Attacking") == true){
			SetRpcSetAnimationClientRpc("Attacking", false);
		}
	}
	
	//assigns the closest castle on the client
	[ClientRpc]
	void SetRpcSetClosestCastleClientRpc(Vector3 attackPosition, int castleId){
		castleAttackPosition = attackPosition;
		
		GameObject[] castles = null;
		
		//find the attackable castles, according to the team this character belongs to
		if(gameObject.CompareTag("Player 1 unit")){
			castles = GameObject.FindGameObjectsWithTag("Player 2 castle");
		}
		else if(gameObject.CompareTag("Player 2 unit")){
			castles = GameObject.FindGameObjectsWithTag("Player 1 castle");
		}
		
		//if there are castles, find the correct castle using the castleId and assign it
		if(castles != null && castles.Length > 0){
			for(int i = 0; i < castles.Length; i++){
				if(castles[i].name == castleId.ToString()){
					castle = castles[i];
				}
			}
		}
	}
	
	//sets the current target enemy on the client
	[ClientRpc]
	public void SetRpcSetCurrentTargetClientRpc(ulong netID)
	{
		// First, check if the local object of this id still exists and then assign the currentTarget
		if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netID, out NetworkObject netObject))
		{
			currentTarget = netObject.transform;
		}
	}
	
	//change character destination on the client
	[ClientRpc]
	void SetRpcChangeDestinationClientRpc(int newDestination){
		//check if we have a proper destination
		if(newDestination != 0){
			destination = newDestination;	
		}
		//resume the agent
		if(agent != null){
			agent.Resume();
		}
	}
	
	//play animation on client
	[ClientRpc]
	void SetRpcSetAnimationClientRpc(string animation, bool state){
		//check for animation and play animation
		if(animation != "" && animator != null){
			animator.SetBool(animation, state);
		}
	}
	
	//resume agent on client
	[ClientRpc]
	void SetRpcResumeAgentClientRpc(){
		//agent is not going to the clicked position 
		goingToClickedPos.Value = false;	
		//reset the stoppingdistance
		agent.stoppingDistance = defaultStoppingDistance;
		
		//actually resume the agent
		if(agent != null){
			agent.Resume();
		}
	}
	
	//tell the server that this character must move towards the clicked position
	[ServerRpc]
	void GoToClickedPositionServerRpc(Vector3 position){
		goToClickedPosition(position);
	}
}
