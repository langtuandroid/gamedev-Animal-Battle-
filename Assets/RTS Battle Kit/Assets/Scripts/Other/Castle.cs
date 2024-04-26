using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Castle : MonoBehaviour {
	
	//variables visible in the inspector
	public float lives;
	public float size;
	public GameObject fracture;
    

    void Update(){
      
	    if(lives <= 0f)
            {
		        lives = 0;
                transform.gameObject.tag = "Untagged";

            //if a fractured version of this part of the castle has been assigned, instantiate it in order to have a cool destruction effect (not recommended on mobile devices)
            /*
                 if (fracture && gameObject.name != "Castle gate" && gameObject.name != "1"){
                     Instantiate(fracture, transform.position, Quaternion.Euler(0, transform.eulerAngles.y, 0));
                 }
                 else if(fracture){
                     Instantiate(fracture, transform.position, Quaternion.Euler(0, 0, 0));
                 }

          */
            if (fracture)
            {
                fracture.SetActive(true);
                fracture.transform.parent = null;
            }
            Destroy(gameObject);
		        
        }
	}
    
}
