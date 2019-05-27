using UnityEngine;
using System.Collections;

public class AIFriendCharacterController : MonoBehaviour {

	public GameObject ObjectTarget;
	public string TargetTag = "Enemy";
	private CharacterSystem character;
	public float DistanceAttack = 2;
	public float DistanceMoveTo = 5;
	public float TurnSpeed = 1.0f;
	
	private int aiTime = 0;
	private int aiState = 0;

	void Start () {
		character = gameObject.GetComponent<CharacterSystem>();
	}
	
	void Update () {
		var direction = Vector3.zero;
		
		if(aiTime<=0){
			aiState = Random.Range(0,4);
			aiTime = Random.Range(10,100);
		}else{
			aiTime--;
		}
		
		
		if(this.GetComponent<CharacterStatus>()){
			DistanceAttack = this.gameObject.GetComponent<CharacterStatus>().PrimaryWeaponDistance;	
		}
		if(ObjectTarget){
			float distance = Vector3.Distance(ObjectTarget.transform.position,this.gameObject.transform.position);
			Quaternion targetRotation = Quaternion.LookRotation (ObjectTarget.transform.position - this.transform.position);
			targetRotation.x = 0;
			targetRotation.z = 0;
   			float str = Mathf.Min (TurnSpeed * Time.deltaTime, 1);
			
			if(distance<=DistanceAttack){
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
				character.Attack();
				if(aiState == 0){
					if(Random.Range(0,100)>95){// Random Using Skill
						if(this.gameObject.GetComponent<CharacterSkillBase>()){
							this.gameObject.GetComponent<CharacterSkillBase>().DeployWithAttacking();	
						}
					}
				}
			}else{
				if(distance<=DistanceMoveTo){
   					transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
					direction = this.transform.forward;
					
				}else{
					ObjectTarget = null;
				}
			}
			
		}else{
			GameObject[] targets = (GameObject[])GameObject.FindGameObjectsWithTag(TargetTag);
			float length = float.MaxValue;
			for(int i=0;i<targets.Length;i++){
				float distancetargets = Vector3.Distance(targets[i].gameObject.transform.position,this.gameObject.transform.position);
				if(distancetargets<=length){
					length = distancetargets;
					ObjectTarget = targets[i].gameObject;
				}
			}
		}
		
		direction.Normalize();
		character.Move(direction);
	}
}

