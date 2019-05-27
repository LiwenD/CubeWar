/// <summary>
/// Player character controller.
/// Player Controller by Keyboard and Mouse
/// </summary>

using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class PlayerCharacterController : MonoBehaviour
{
	
	private CharacterSystem character;
	private TouchScreenVal touchScreenMover;
	private TouchScreenVal touchScreenPress;

	
	void Start()
	{
		touchScreenMover = new TouchScreenVal(new Rect(0,0,Screen.width/2,Screen.height));
		touchScreenPress = new TouchScreenVal(new Rect(Screen.width/2,0,Screen.width/2,Screen.height));
		if(this.gameObject.GetComponent<CharacterSystem>()){
			character = this.gameObject.GetComponent<CharacterSystem>();
		}
		Screen.lockCursor = true;
	}
	
	
	void Update()
	{

		if(!character)
			return;
		
			var direction	= Vector3.zero;
			var forward	= Quaternion.AngleAxis(-90,Vector3.up) * Camera.main.transform.right;
		
			mobileController();
		
			if(Input.GetKey(KeyCode.W))
				direction	+= forward;
			if(Input.GetKey(KeyCode.S))
				direction	-= forward;
			if(Input.GetKey(KeyCode.A))
				direction	-= Camera.main.transform.right;
			if(Input.GetKey(KeyCode.D))
				direction	+= Camera.main.transform.right;
				
			if(Input.GetMouseButtonDown(0))
			{
				character.Attack();
			}
			var orbit = (OrbitGameObject)FindObjectOfType(typeof(OrbitGameObject));
			
			if(Input.GetKey(KeyCode.LeftShift))
			{
				orbit.HoldAim();
			}

			if(Input.GetMouseButtonDown(1))
			{
				character.Attack();
				var skillDeployer = this.gameObject.GetComponent<CharacterSkillBase>();
				if(skillDeployer != null)
					skillDeployer.DeployWithAttacking();	
			}
		
			direction.Normalize();
			character.Move(direction);
			if(direction.magnitude>0){
				if(!Screen.lockCursor)
				{
					Screen.lockCursor = true;
				}
			}
	}
	
	// Control with Touchscreen

	void mobileController(){
		if(!character)
			return;
		
		if(touchScreenPress.OnTouchPress()){
			character.Attack();	
		}
		
		
		var direction	= Vector3.zero;
		var touchDirection = touchScreenMover.OnTouchDirection();
		direction.x = touchDirection.x;
		direction.z = touchDirection.y;
		
		character.Move(direction);
	}

	
}

