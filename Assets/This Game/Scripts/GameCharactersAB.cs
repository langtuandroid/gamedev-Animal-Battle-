using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCharactersAB : MonoBehaviour {

	public GameObject[] playerCharacters;
    public static GameCharactersAB instanceE;
    public int[] cardxLevelL;
   
	private void Awake () {
        if (instanceE == null)
        {
            instanceE = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
    }
	
	
}
