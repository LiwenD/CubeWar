using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

	public SkillData[] Skills;

	public int GetManaCost(int skillIndex,int level){
		if(Skills[skillIndex].ManaCost.Length<=0)
			return 0;
		
		if(level-1>=Skills[skillIndex].ManaCost.Length){
			return Skills[skillIndex].ManaCost[Skills[skillIndex].SkillObject.Length-1];
		}
		return Skills[skillIndex].ManaCost[level-1];
	}
	
	public GameObject GetSkillObject(int skillIndex,int level){
		if(Skills[skillIndex].SkillObject.Length<=0)
			return null;
		
		if(level-1>=Skills[skillIndex].SkillObject.Length){
			return Skills[skillIndex].SkillObject[Skills[skillIndex].SkillObject.Length-1];
		}
		return Skills[skillIndex].SkillObject[level-1];
	}
}
