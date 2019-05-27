using UnityEngine;
using System.Collections;

public class CharacterSkillBase : MonoBehaviour {

	public int indexSkill;// Current skill index	
	[HideInInspector]
	public SkillManager skillManage;
	[HideInInspector]
	public bool attackingSkill;
	[HideInInspector]
	public CharacterStatus character;
	[HideInInspector]
	public CharacterAttack characterAttack;
	[HideInInspector]
	public CharacterSystem characterSystem;
	public int SkillPoint;
	
	void Start () {
		if(this.gameObject.GetComponent<CharacterStatus>()){
			character = this.gameObject.GetComponent<CharacterStatus>();
		}
		if(this.gameObject.GetComponent<CharacterAttack>()){
			characterAttack = this.gameObject.GetComponent<CharacterAttack>();
		}
		if(this.gameObject.GetComponent<CharacterSystem>()){
			characterSystem = this.gameObject.GetComponent<CharacterSystem>();
		}
		skillManage = (SkillManager)GameObject.FindObjectOfType(typeof(SkillManager));
	}
	
	public virtual void DeploySkill(int index)
	{
		indexSkill = index;
		DeploySkill();
	}	
	
	public virtual void DeploySkill()
	{
		
	}
	
	public virtual void DeployWithAttacking(int index){
		if(characterSystem)
		characterSystem.Attack();
		indexSkill = index;
		attackingSkill = true;		
	}
	
	public virtual void DeployWithAttacking(){
		if(characterSystem)
		characterSystem.Attack();
		attackingSkill = true;
	}
	
	void Update()
	{
		if(attackingSkill && characterAttack.Activated)
		{
			DeploySkill();
			attackingSkill = false;
			characterAttack.Activated = false;
		}
	}
}
