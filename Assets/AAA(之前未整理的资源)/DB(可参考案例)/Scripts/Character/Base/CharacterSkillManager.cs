/// <summary>
/// Character skill deployer.
/// the skills is reference to Skills in SkillManager
/// </summary>


using UnityEngine;
using System.Collections;

public class CharacterSkillManager : CharacterSkillBase {

	public int[] SkillIndex;// list of available skills 
	public int[] SkillLevel;// list of skills levels

	public override void DeploySkill()
	{
		
		if(skillManage == null)
			return;
		
		// Launch an ojbect sync with Animation Attacking
		if(SkillIndex.Length > 0 && skillManage.Skills[SkillIndex[indexSkill]].SkillObject != null)
		{
		
			if(character != null && character.SP >= skillManage.GetManaCost(SkillIndex[indexSkill],SkillLevel[indexSkill]))
			{
				var skill = (GameObject)GameObject.Instantiate(skillManage.GetSkillObject(SkillIndex[indexSkill],SkillLevel[indexSkill]),this.transform.position,this.transform.rotation);
				if(skill.GetComponent<SkillBase>()){
					skill.GetComponent<SkillBase>().Owner = this.gameObject;	
				}
				skill.transform.forward	= this.transform.forward;
				character.SP -= skillManage.GetManaCost(SkillIndex[indexSkill],SkillLevel[indexSkill]);
			}
		}
		base.DeploySkill();
	}
}
