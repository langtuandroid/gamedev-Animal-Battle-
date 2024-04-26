using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CamController : MonoBehaviour {
	
	public bool multiplayer;
	
	//variables not visible in the inspector
	public static float movespeed = 10;
	public static float zoomSpeed;
	public static float mouseSensitivity;
    public static float clampAngle;
	
    float rotationY = 0;
    float rotationX = 0;
	
	Vector2 moveVec;
	Vector2 dragStartPos;

   public  Vector2 xLimit, zLimit;

    void Start(){
	//get start rotation
    Vector3 rot = transform.localRotation.eulerAngles;
    rotationY = rot.y;
    rotationX = rot.x;
	
		if(transform.position.x < 0 && multiplayer){
			rotationY += 180;
		}
    }
	
	void Update(){
	//if the mobile prefab is added to the scene, use mobile controls. Else use pc controls
	if(GameObject.Find("Mobile") == null && GameObject.Find("Mobile multiplayer") == null){
            
	PcCamera();
	}
	else if((GameObject.Find("Mobile") && Mobile.camEnabled) || (GameObject.Find("Mobile multiplayer") && MobileMultiplayer.camEnabled)){
	MobileCamera();
          
        }
	}
	
	void PcCamera(){
        Vector3 v = Vector3.forward * CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 h = Vector3.right * CrossPlatformInputManager.GetAxis("Horizontal");
        //if key gets pressed move left/right
        
        //v = v / 2;
        transform.Translate((v+h) * Time.deltaTime * movespeed);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x ,xLimit.x,xLimit.y),transform.position.y, Mathf.Clamp(transform.position.z,zLimit.x,zLimit.y)); 


            //if scrollwheel is down rotate camera
            if (Input.GetMouseButton(2)){
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");
		rotateCamera(mouseX, mouseY);
		}
	
		//move camera when you scroll
		//transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime * zoomSpeed);
	}
	
	
	void MobileCamera(){
		//check if exactly one finger is touching the screen
		if(Input.touchCount == 1){
			//rotate camera based on the touch position
			Touch touch = Input.GetTouch(0);
			float mouseX = touch.deltaPosition.x;
			float mouseY = -touch.deltaPosition.y;
			rotateCamera(mouseX, mouseY);
		}
		//check for two touches
		else if(Input.touchCount == 2){
            //store two touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //find the position in the previous frame of each touch
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            //find the magnitude of the vector (the distance) between the touches in each frame
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            //find the difference in the distances between each frame
            float z = (prevTouchDeltaMag - touchDeltaMag) * 0.005f * zoomSpeed;
			
			//zoom camera by moving it forward/backward
			transform.Translate(new Vector3(0, 0, -z));
		}
	}
	
	
	void rotateCamera(float mouseX, float mouseY){
		//check if mobile controls are enabled to adjust sensitivity
		if(GameObject.Find("Mobile") == null && GameObject.Find("Mobile multiplayer") == null){
		rotationY += mouseX * mouseSensitivity * Time.deltaTime;
		rotationX += mouseY * mouseSensitivity * Time.deltaTime;
		}
		else{
		rotationY += mouseX * mouseSensitivity * Time.deltaTime * 0.2f;
		rotationX += mouseY * mouseSensitivity * Time.deltaTime * 0.2f;	
		}
	
		//clamp x rotation to limit it
		rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);
	
		//apply rotation
		transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
	}
}
