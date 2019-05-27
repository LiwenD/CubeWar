using UnityEngine;

using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterStatus))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(CharacterInventory))]

public class CharacterSystem : MonoBehaviour
{
	
	public float Speed	= 2; // Move speed
	public float SpeedAttack = 1.5f; // Attack speed
	public float TurnSpeed	= 5; // turning speed
	
	public AttackAnimation[] AttackAnimations;
	public string[] ComboAttackLists;// list of combo set
	public int ComboType; // type of attacking
	
	public string PoseHit = "Hit";// pose animation when character got hit
	public string PoseIdle = "Idle";
	public string PoseRun = "Run";
	public bool IsHero;
	
	//private variable
	private bool diddamaged;
	private int attackStep = 0;
	private string[] comboList;
	private int attackStack;
	private float attackStackTimeTemp;
	private float frozetime;
	private bool hited;
	private bool attacking;
	

	CharacterMotor motor;
	
	void Start()
	{
		motor = gameObject.GetComponent<CharacterMotor>();
		// Play pose Idle first
		gameObject.GetComponent<Animation>().CrossFade(PoseIdle);
		attacking = false;
	}

	
	void Update()
	{
		// Animation combo system
		
		if(ComboAttackLists.Length<=0 || ComboType >= ComboAttackLists.Length){// if have no combo list
			return;
		}
		
		comboList = ComboAttackLists[ComboType].Split(","[0]);// Get list of animation index from combolists split by ComboType
		
		if(comboList.Length > attackStep){
			int poseIndex = int.Parse(comboList[attackStep]);// Read index of current animation from combo array
			if(poseIndex < AttackAnimations.Length && this.gameObject.GetComponent<Animation>()[AttackAnimations[poseIndex].AttackName]){	
				// checking index of AttackAnimations list
				
				AnimationState attackState = this.gameObject.GetComponent<Animation>()[AttackAnimations[poseIndex].AttackName]; // get animation name AttackAnimations[poseIndex]
				attackState.layer = 2;
    			attackState.blendMode = AnimationBlendMode.Blend;
				attackState.speed = SpeedAttack;
				
				if(attackState.time >= attackState.length * 0.1f){
					// set attacking to True when time of attack animation is running to 10% of animation
		  			attacking = true;	
	  			}	
	  	 		if(attackState.time >= AttackAnimations[poseIndex].AttackTime){
					// if the time of attack animation is running to marking point (AttackAnimations[poseIndex].AttackTime) 
					// calling CharacterAttack.cs to push a damage out
	      			if(!diddamaged){
						// push a damage out
		 				this.gameObject.GetComponent<CharacterAttack>().DoAttack(); 
						
		 	 		}
				}
				
				if(attackState.time >= attackState.length * 0.8f){
					// if the time of attack animation is running to 80% of animation. It's should be Finish this pose.
					
					attackState.normalizedTime = attackState.length;
					diddamaged = true;
					attacking = false;
					attackStep += 1;

					if(attackStack>1){
						// checking if a calling attacking is stacked
						fightAnimation();	
					}else{
						if(attackStep>=comboList.Length){
							// finish combo and reset to idle pose
							resetCombo();
							if(this.gameObject.GetComponent<Animation>()[PoseIdle])
		  					this.gameObject.GetComponent<Animation>().Play(PoseIdle);
						}	
					}
					// reset character damage system
					this.gameObject.GetComponent<CharacterAttack>().StartDamage();
	  			}	
			}
		}
		
		if(hited){// Freeze when got hit
			if(frozetime>0){
				frozetime--;	
			}else{
				hited = false;
				if(this.gameObject.GetComponent<Animation>()[PoseIdle])
				this.gameObject.GetComponent<Animation>().Play(PoseIdle);
			}
		}
		
		if(Time.time > attackStackTimeTemp+2){
			resetCombo();
		}
		
	}

	public void GotHit(float time){
		if(!IsHero){
			if(this.gameObject.GetComponent<Animation>()[PoseHit]){
				this.gameObject.GetComponent<Animation>().Play(PoseHit, PlayMode.StopAll);
			}
			frozetime = time * Time.deltaTime;// froze time when got hit
			hited = true;
				
		}
	}
	
	private void resetCombo(){
		attackStep = 0;
		attackStack = 0;
		
	}
	
	private void fightAnimation(){
		
		attacking = false;
		if(attackStep>=comboList.Length){
		  	resetCombo();	
		}
		
		int poseIndex = int.Parse(comboList[attackStep]);
		if(poseIndex < AttackAnimations.Length){// checking poseIndex is must in the AttackAnimations list.
			if(this.gameObject.GetComponent<CharacterAttack>()){
				// Play Attack Animation 
				if(this.gameObject.GetComponent<Animation>()[AttackAnimations[poseIndex].AttackName])
				this.gameObject.GetComponent<Animation>().Play(AttackAnimations[poseIndex].AttackName,PlayMode.StopAll);	 
			}
    		diddamaged = false;
		}
	}
	
	public void Attack()
	{	
		if(frozetime<=0){
			attackStackTimeTemp = Time.time;
			fightAnimation();
			attackStack+=1;
		}	
	}
	
	public void Move(Vector3 dir){
		if(!attacking){
			moveDirection = dir;
		}else{
			moveDirection = dir/2f;	
		}
	}

	
	Vector3 direction;
	private Vector3 moveDirection
	{
		get { return direction; }
		set
		{
			direction = value;
			if(direction.magnitude > 0.1f)  
	    	{
	    		var newRotation	= Quaternion.LookRotation(direction);
				transform.rotation	= Quaternion.Slerp(transform.rotation,newRotation,Time.deltaTime * TurnSpeed);
			}
			direction *= Speed * 0.5f * (Vector3.Dot(gameObject.transform.forward,direction) + 1);
				
			if(direction.magnitude > 0.001f)  
			{
				// Play Runing Animation when moving
				float speedaimation = direction.magnitude * 3;
				gameObject.GetComponent<Animation>().CrossFade(PoseRun);
				if(speedaimation<1){
					speedaimation = 1;	
				}
				// Speed animation sync to Move Speed
				gameObject.GetComponent<Animation>()[PoseRun].speed	= speedaimation;
			
			}
			else{
				// Play Idle Animation when stoped
				gameObject.GetComponent<Animation>().CrossFade(PoseIdle);
			}
			if(motor){
				motor.inputMoveDirection = direction;
			}
		}
	}
	
	float pushPower = 2.0f;
	void OnControllerColliderHit(ControllerColliderHit hit)// Character can push an object.
	{
	    var body = hit.collider.attachedRigidbody;
	    if(body == null || body.isKinematic){
			return;
		}
	    if(hit.moveDirection.y < -0.3){
			return;
		}
		
	    var pushDir = Vector3.Scale(hit.moveDirection,new Vector3(1,0,1));
	    body.velocity = pushDir * pushPower;
	}
}