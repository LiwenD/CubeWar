using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillData{
	public Texture2D SkillIcon;
	public string SkillName = "Skill Name";
	public string Descrtiption = "Description";
	public int Level = 1;
	public int LevelMax = 1;
	public int[] ManaCost;
	
	public GameObject[] SkillObject;
	
	public SkillData(int levelmax){
		Level = 1;
		LevelMax = levelmax;
		SkillObject = new GameObject[levelmax];
		ManaCost = new int[levelmax];
	}
	
}
