using UnityEngine;
using System.Collections;

public class SkillRenderer : Frame {

	private SkillManager skillManage;
	private Vector2 scrollPosition;
	
	public SkillRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){
		skillManage = (SkillManager)FindObjectOfType(typeof(SkillManager));
	}
	
	public void Draw(CharacterSkillManager characterSkill){
		if(Show && characterSkill!=null && characterSkill.SkillIndex.Length>0){
			GUI.BeginGroup(new Rect(Position.x,Position.y,300,380));
			GUI.Box(new Rect(0,0,300,380),"");
			
			if(GUI.Button(new Rect(240,340,50,30),"Close")){
				Show = false;
			}	
			if(GUI.Button(new Rect(0,0,300,30),"Skills")){
				OnDraging();
			}	
			GUI.skin.label.normal.textColor = Color.white;
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(30,350,150,30),"Skill Point "+characterSkill.SkillPoint.ToString());
			
			scrollPosition = GUI.BeginScrollView(new Rect(0, 40, 300, 290), scrollPosition, new Rect(0, 50, 280, characterSkill.SkillIndex.Length * 60));
			for(int i=0;i<characterSkill.SkillIndex.Length;i++){
				DrwoSkill(i,i,new Vector2(0,(i*60) + 50),characterSkill);
			}
			
			GUI.EndScrollView();
			GUI.EndGroup();
		}
	}
	
	public void DrwoSkill(int indexSlot,int indexSkill,Vector2 position,CharacterSkillManager characterSkill){
		if(!skillManage)
			return;
		
		if(!characterSkill)
			return;
		
			if(GUI.Button(new Rect(10 + position.x,10 + position.y,50,50),skillManage.Skills[characterSkill.SkillIndex[indexSkill]].SkillIcon)){
				PlayerUI.shotcutRenderer.AddShopCut(indexSkill,1,0.1f);	
			}
			
			GUI.skin.label.normal.textColor = Color.white;
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(14+position.x,14+position.y,30,30),characterSkill.SkillLevel[indexSkill].ToString());
			
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(position.x+70,position.y,100,60),skillManage.Skills[characterSkill.SkillIndex[indexSkill]].SkillName);
			GUI.skin.label.normal.textColor = new Color(0.1f,0.5f,1.0f,1);
			GUI.Label(new Rect(position.x+70,position.y+15,100,60),"Mana "+skillManage.GetManaCost(characterSkill.SkillIndex[indexSkill],characterSkill.SkillLevel[indexSkill]).ToString());
			
			
			if(characterSkill.SkillPoint>0){
				if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Up")){
					if(characterSkill.SkillLevel[indexSkill] < skillManage.Skills[characterSkill.SkillIndex[indexSkill]].LevelMax){
						if(characterSkill.SkillPoint>0){
							characterSkill.SkillLevel[indexSkill] += 1;
							characterSkill.SkillPoint-=1;
						}
					}
				}
			}
		
	}
}
