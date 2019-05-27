using UnityEngine;
using System.Collections;

public struct ShotcutSlot{
	public int Type;
	public int Index;
	public bool Active;
 	public KeyCode Key;
	public float cooldown;
	public float timepress;
}


public class ShotcutRenderer : Frame {
	
	public ShotcutSlot[] Shotcut;
	private ItemManager itemManage;
	private SkillManager skillManage;
	
	public void AddShopCut(int index,int type,float cooldown){
		
		for(int i=0;i<Shotcut.Length;i++){
			if(Shotcut[i].Active){
				if(Shotcut[i].Index == index && Shotcut[i].Type == type){
					return;
				}
			}
		}
		
		for(int i=0;i<Shotcut.Length;i++){
			if(!Shotcut[i].Active){
				Shotcut[i].Active = true;
				Shotcut[i].Index = index;
				Shotcut[i].Type = type;
				Shotcut[i].cooldown = cooldown;
				return;
			}
		}
	}
	
	public ShotcutRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){
		Shotcut = new ShotcutSlot[5];
		Shotcut[0].Key = KeyCode.Alpha1;
		Shotcut[1].Key = KeyCode.Alpha2;
		Shotcut[2].Key = KeyCode.Alpha3;
		Shotcut[3].Key = KeyCode.Alpha4;
		Shotcut[4].Key = KeyCode.Alpha5;
		
		skillManage = (SkillManager)FindObjectOfType(typeof(SkillManager));
		itemManage = (ItemManager)FindObjectOfType(typeof(ItemManager));
	}
	
	public void Draw(){
		GUI.BeginGroup(new Rect(Position.x,Position.y,Shotcut.Length * 60,80));
		for(int i=0;i<Shotcut.Length;i++){
			if(Shotcut[i].Active){
				switch(Shotcut[i].Type){
				case 0:
					DrawItem(i,new Vector2(i*60,18),PlayerUI.characterInventory);
					break;
				case 1:
					DrawSkill(i,new Vector2(i*60,18),PlayerUI.characterSkill);
					break;
				}
			}else{
				GUI.Box(new Rect(i*60,18,50,50),"");	
			}
			if(GUI.Button(new Rect(i*60,0,50,15),"x")){
				Shotcut[i].Active = false;
			}
		}
		GUI.EndGroup();
	}
	
	
	public void DrawItem(int index,Vector2 position,CharacterInventory characterInventory){
		if(!characterInventory || !itemManage || Shotcut[index].Index >= characterInventory.ItemSlots.Count){
			Shotcut[index].Active = false;
			return;
			
		}
		var itemslot = characterInventory.ItemSlots[Shotcut[index].Index];

		if(itemslot!=null){
			var item = itemManage.Items[itemslot.Index];

			if(GUI.Button(new Rect(position.x, position.y,50,50),item.Icon) || Input.GetKeyDown(Shotcut[index].Key)){
				if(Time.time > Shotcut[index].timepress + Shotcut[index].cooldown){
					switch(item.ItemType)
					{
					case ItemType.Weapon:
						if(characterInventory.CheckEquiped(itemslot)){
							characterInventory.UnEquipItem(itemslot);
						}else{
							characterInventory.EquipItem(itemslot);	
						}
						break;
					case ItemType.Edible:
						characterInventory.UseItem(itemslot);
						break;
				
					}
					Shotcut[index].timepress = Time.time;
				}
			}
			GUI.skin.label.normal.textColor = Color.white;

			if(itemslot.Num>0){
				GUI.skin.label.fontSize = 13;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label(new Rect(4+position.x,4+position.y,30,30),itemslot.Num.ToString());
			}
		}
		if(itemslot== null){
			Shotcut[index].Active = false;	
		}
	}

	
	public void DrawSkill(int index,Vector2 position,CharacterSkillManager characterSkill){

		if(!characterSkill || !skillManage){
			Shotcut[index].Active = false;
			return;
			
		}
		if(GUI.Button(new Rect(position.x, position.y,50,50),skillManage.Skills[characterSkill.SkillIndex[Shotcut[index].Index]].SkillIcon) || Input.GetKeyDown(Shotcut[index].Key)){
			if(Time.time > Shotcut[index].timepress + Shotcut[index].cooldown){
				characterSkill.indexSkill = characterSkill.SkillIndex[Shotcut[index].Index];
				characterSkill.DeployWithAttacking();
				Shotcut[index].timepress = Time.time;
			}
		}
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 13;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(4+position.x,4+position.y,30,30),characterSkill.SkillLevel[Shotcut[index].Index].ToString());
		
	}
}
