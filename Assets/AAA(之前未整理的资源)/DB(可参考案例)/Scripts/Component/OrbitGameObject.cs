using UnityEngine;
using System.Collections;

public class OrbitGameObject : Orbit
{
	public GameObject Target;
	public GameObject CameraObject;
	public Vector3 TargetOffset	= Vector3.zero;
	public Vector3 CameraPositionZoom;
	public float CameraLength;
	public float CameraLengthZoom;
	public Vector3 ShakeVal = Vector3.zero;
	private float zoomTemp;
	private float zoomVal;
	private Vector3 cameraPositiontemp;
	private Vector3 cameraPosition;
	
	

	void Start()
	{
		Data.Zenith	= -0.4f;
		Data.Length	= CameraLength;
		zoomTemp = CameraLength;

		if(CameraObject){
			cameraPositiontemp = CameraObject.transform.localPosition;
			cameraPosition = cameraPositiontemp;
		}
	}
	
	public void HoldAim(){
		if(CameraObject){
			cameraPosition = CameraPositionZoom;	
			zoomVal = CameraLengthZoom;
			
		}
	}

	protected override void Update()
	{
		if(CameraObject){
			CameraObject.transform.localPosition = Vector3.Lerp(CameraObject.transform.localPosition,cameraPosition,10.0f * Time.deltaTime);
		}	
		
		if(Screen.lockCursor){
			Data.Azimuth += Input.GetAxis("Mouse X") / 100;
			Data.Zenith	+= Input.GetAxis("Mouse Y") / 100;
		}
	
		
		Data.Zenith	= Mathf.Clamp(Data.Zenith,-0.8f,0f);
		Data.Length	+= (zoomVal - Data.Length) / 10;

		
		Time.timeScale	+= (1 - Time.timeScale) / 10f;
		var lookAt	= TargetOffset + ShakeVal;
		
		if(!Target){
			var character = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
			if(character){
				Target = character.Player;	
			}
		}
		
		if(Target != null){
			lookAt	+= Target.transform.position;
			base.Update();
			gameObject.transform.position	+= lookAt;
			gameObject.transform.LookAt(lookAt);
			if(zoomVal == CameraLengthZoom){
				Quaternion targetRotation = this.transform.rotation;
				targetRotation.x = 0;
				targetRotation.z = 0;
   				Target.transform.rotation = targetRotation;
			}
		}
		
		cameraPosition = cameraPositiontemp;
		zoomVal = CameraLength;
		rotationXtemp = Data.Azimuth;
		rotationYtemp = Data.Zenith;
	}
	

	// Rotation screen with touchs
	
	private float rotationX = 0;
	private float rotationY = 0;
	private float rotationXtemp = 0;
	private float rotationYtemp = 0;
	private Vector2 controllerPositionTemp;
	private Vector2 controllerPositionNext;	
	
	void mobileController(){
		for (var i = 0; i < Input.touchCount; ++i) {
			if(Input.GetTouch(i).position.x < Screen.width/2){
				if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary){
                	
				}	
			}else{	
				if(Input.GetTouch(i).phase == TouchPhase.Began){
					controllerPositionNext = new Vector2(Input.GetTouch(i).position.x,Screen.height - Input.GetTouch(i).position.y);
					controllerPositionTemp = controllerPositionNext;
					
				}else{
					controllerPositionNext = new Vector2(Input.GetTouch(i).position.x,Screen.height - Input.GetTouch(i).position.y);
					Vector2 deltagrag = (controllerPositionNext-controllerPositionTemp);
					Data.Azimuth = rotationXtemp + (deltagrag.x * 0.01f * Time.deltaTime);
					Data.Zenith = rotationYtemp + (-deltagrag.y * 0.01f * Time.deltaTime);
					controllerPositionTemp = Vector2.Lerp(controllerPositionTemp,controllerPositionNext,0.5f);
				}	
			}
        }
	}
	
}
