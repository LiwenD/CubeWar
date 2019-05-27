// Save all character data

using UnityEngine;
using System.Collections;

public class PlayerSave : MonoBehaviour {
	public void Save(){
		
		CharacterStatus characterStatus = this.gameObject.GetComponent<CharacterStatus>();
		CharacterInventory characterInventory = this.gameObject.GetComponent<CharacterInventory>();
		string namesave = "_"+characterStatus.Name;
		// save status
		PlayerPrefs.SetString("SAVEDATE"+namesave,System.DateTime.Now.ToString());
		PlayerPrefs.SetInt("STR"+namesave,characterStatus.STR);
		PlayerPrefs.SetInt("AGI"+namesave,characterStatus.AGI);
		PlayerPrefs.SetInt("INT"+namesave,characterStatus.INT);
		PlayerPrefs.SetInt("LEVEL"+namesave,characterStatus.LEVEL);
		PlayerPrefs.SetInt("EXP"+namesave,characterStatus.EXP);
		PlayerPrefs.SetInt("EXPmax"+namesave,characterStatus.EXPmax);
		PlayerPrefs.SetInt("HP"+namesave,characterStatus.HP);
		PlayerPrefs.SetInt("SP"+namesave,characterStatus.SP);
		
		string itemIndex = "";
		string itemNum = "";
		for(int i=0;i<characterInventory.ItemSlots.Count;i++){
			if(characterInventory.ItemSlots[i]!=null){
				itemIndex += characterInventory.ItemSlots[i].Index+",";
				itemNum += characterInventory.ItemSlots[i].Num+",";
			}
		}
		PlayerPrefs.SetString("ItemsIndex"+namesave,itemIndex);
		PlayerPrefs.SetString("ItemsNum"+namesave,itemNum);
		
		string itemEqIndex = "";
		for(int i=0;i<characterInventory.ItemsEquiped.Length;i++){
			if(characterInventory.ItemsEquiped[i]!=null){
				itemEqIndex += characterInventory.ItemsEquiped[i].Index+",";
			}
		}
		PlayerPrefs.SetString("ItemsEqIndex"+namesave,itemEqIndex);
		Debug.Log("Save Game");
	}
	
	
	
	
	
	public void Load(){
		CharacterStatus characterStatus = this.gameObject.GetComponent<CharacterStatus>();
		CharacterInventory characterInventory = this.gameObject.GetComponent<CharacterInventory>();
		string namesave = "_"+characterStatus.Name;
		if(PlayerPrefs.GetString("SAVEDATE"+namesave) == ""){
			Debug.Log("No Save Game");
			return;
		}
		Debug.Log("Load Game");
		
		characterStatus.STR = PlayerPrefs.GetInt("STR"+namesave);
		characterStatus.AGI = PlayerPrefs.GetInt("AGI"+namesave);
		characterStatus.INT = PlayerPrefs.GetInt("INT"+namesave);
		characterStatus.LEVEL = PlayerPrefs.GetInt("LEVEL"+namesave);
		characterStatus.EXP = PlayerPrefs.GetInt("EXP"+namesave);
		characterStatus.EXPmax = PlayerPrefs.GetInt("EXPmax"+namesave);
		characterStatus.HP = PlayerPrefs.GetInt("HP"+namesave);
		characterStatus.SP = PlayerPrefs.GetInt("SP"+namesave);
		
		string[] itemIndex = PlayerPrefs.GetString("ItemsIndex"+namesave).Split(","[0]);
		string[] itemNum = PlayerPrefs.GetString("ItemsNum"+namesave).Split(","[0]);
		string[] itemEqIndex = PlayerPrefs.GetString("ItemsEqIndex"+namesave).Split(","[0]);
		Debug.Log(PlayerPrefs.GetString("ItemsEqIndex"+namesave));
		characterInventory.ItemSlots.Clear();
		for(int i=0;i<itemIndex.Length;i++){
			if(itemIndex[i]!=""){
				ItemSlot itemS = new ItemSlot();
				itemS.Index = int.Parse(itemIndex[i]);
				itemS.Num = int.Parse(itemNum[i]);
				characterInventory.ItemSlots.Add(itemS);
			}
		}
		characterInventory.UnEquipAll();
		for(int i=0;i<itemEqIndex.Length;i++){
			if(itemEqIndex[i]!=null && itemEqIndex[i]!=""){
				ItemSlot itemQe = new ItemSlot();
				itemQe.Index = int.Parse(itemEqIndex[i]);
				characterInventory.EquipItem(itemQe);
			}
		}
		
	}
	
	void OnGUI(){
		if(GUI.Button(new Rect(Screen.width/2 - 85,10,80,25),"Save")){
			Save();
		}
		if(GUI.Button(new Rect(Screen.width/2 + 5,10,80,25),"Load")){
			Load();
		}
	}
	
}
