using UnityEngine;
using System.Collections;
using Unity.Netcode;
using UnityEngine.Networking;

public class ArcherMultiplayer : NetworkBehaviour {

	//variables visible in the inspector
	public GameObject arrow;
	public Transform arrowSpawner;
	public GameObject animationArrow;
	
	//not visible in the inspector
	private bool shooting;
	private bool addArrowForce;
	private GameObject newArrow;
	private float shootingForce;
	private Animator animator;
	
	//get the animator
	void Start(){
		animator = GetComponent<Animator>();
	}
	
	void Update(){
		//only shoot when animation is almost done (when the character is shooting)
		if(animator.GetBool("Attacking") == true && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.95f && !shooting){
			StartCoroutine(shoot());
		}
		
		//set an extra arrow active to make illusion of shooting more realistic
		if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.25f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.95f){
			animationArrow.SetActive(true);
		}
		else{
			animationArrow.SetActive(false);
		}
	}
	
	void LateUpdate(){
		//check if the archer shoots an arrow
		if(addArrowForce && this.gameObject != null && GetComponent<CharacterMultiplayer>().currentTarget != null && newArrow != null && arrowSpawner != null){
			//create a shootingforce
			shootingForce = Vector3.Distance(transform.position, GetComponent<CharacterMultiplayer>().currentTarget.transform.position);
			//add shooting force to the arrow
			newArrow.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(new Vector3(0, shootingForce * 12 + 
			((GetComponent<CharacterMultiplayer>().currentTarget.transform.position.y - transform.position.y) * 45), shootingForce * 55)));
			addArrowForce = false;
		}
		else if(addArrowForce && this.gameObject != null && newArrow != null && arrowSpawner != null){
			//shoot with a different force when archer is attacking a castle
			shootingForce = Vector3.Distance(transform.position, GetComponent<CharacterMultiplayer>().castleAttackPosition);
			newArrow.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(new Vector3(0, shootingForce * 12 + 
			((GetComponent<CharacterMultiplayer>().castleAttackPosition.y - transform.position.y) * 45), shootingForce * 55)));
			addArrowForce = false;
		}
	}
	
	IEnumerator shoot(){
		//archer is currently shooting
		shooting = true;
		
		//tell the server to shoot 
		ShootServerRpc(arrowSpawner.rotation);
		
		//shoot it using rigidbody addforce
		addArrowForce = true;
	
		//wait and set shooting back to false
		yield return new WaitForSeconds(0.5f);
		shooting = false;
	}
	
	[ServerRpc]
	public void ShootServerRpc(Quaternion rotation)
	{
		GameObject newArrow = Instantiate(arrow, arrowSpawner.position, rotation);
		newArrow.GetComponent<NetworkObject>().Spawn();

		// Call the client RPC to notify all clients about the new arrow
		AssignArrowClientRpc(newArrow.GetComponent<NetworkObject>().NetworkObjectId);
	}
	
	//assign the new arrow on client
	[ClientRpc]
	public void AssignArrowClientRpc(ulong networkObjectId)
	{
		if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject netObject))
		{
			GameObject arrow = netObject.gameObject;
			// Now you can do whatever is needed with 'arrow' on the clients
			// For example, set this as the new arrow the client should track
			newArrow = arrow;
		}
	}
}
