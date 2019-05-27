/// <summary>
/// Character skill deployer.
/// Basic Skill Launcher system
/// </summary>

using UnityEngine;
using System.Collections;


public class CharacterSkillDeployer : CharacterSkillBase {

	public GameObject[] Skill;//List of Launch object
	public int[] ManaCost;// List of Mana cost
	public Texture2D[] SkillIcon;// List of Skill image

	public override void DeploySkill()
	{
		// Launch an ojbect sync with Animation Attacking
		if(Skill.Length > 0 && Skill[indexSkill] != null)
		{
			if(character != null && character.SP >= ManaCost[indexSkill])
			{
				var skill = (GameObject)GameObject.Instantiate(Skill[indexSkill],this.transform.position,this.transform.rotation);
				if(skill.GetComponent<SkillBase>()){
					skill.GetComponent<SkillBase>().Owner = this.gameObject;	
				}
				skill.transform.forward	= this.transform.forward;
				character.SP -= ManaCost[indexSkill];
			}
		}
		base.DeploySkill();
	}

}
