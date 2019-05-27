using UnityEngine;
using System.Collections;

public class ItemUI : MonoBehaviour {
	
	[HideInInspector]
	public CharacterInventory characterInventory;
	[HideInInspector]
	public ItemManager itemManage;
	
	public void SettingItemUI(){

		if(!itemManage)
		itemManage = (ItemManager)FindObjectOfType(typeof(ItemManager));
	}
	

		
	// Draw item icon
	public void DrawItemBox(ItemSlot itemslot,Vector2 position){
		if(itemslot!=null){
			ItemCollector item = itemManage.Items[itemslot.Index];
			GUI.Box(new Rect(10 + position.x,10 + position.y,50,50),"");	
			GUI.DrawTexture(new Rect(10 + position.x,10 + position.y,50,50),item.Icon);
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(14+position.x,14+position.y,30,30),itemslot.Num.ToString());
		}
	}
	
	
	// Draw Item icon with detail
	public void DrawItemBoxDetail(ItemSlot itemslot,Vector2 position){
		if(itemslot!=null){
			var item = itemManage.Items[itemslot.Index];
			GUI.Box(new Rect(10 + position.x,10 + position.y,50,50),"");	
			GUI.DrawTexture(new Rect(10 + position.x,10 + position.y,50,50),item.Icon);
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(14+position.x,14+position.y,30,30),itemslot.Num.ToString());
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(position.x+70,position.y,100,60),item.Name);
			
			if(!characterInventory)
				return;
			
			switch(item.ItemType)
			{
			case ItemType.Weapon:
				if(characterInventory.CheckEquiped(itemslot)){
					if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"UnEquipped")){
						characterInventory.UnEquipItem(itemslot);
					}
				}else{
					if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Equip")){
						characterInventory.EquipItem(itemslot);
					}
				}
				break;
			case ItemType.Edible:
				if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Use")){
					characterInventory.UseItem(itemslot);
				}
				break;
				
			}
		}
	}
	
	public void DrawItemBoxShopDetail(ItemSlot itemslot,Vector2 position){
		if(itemslot!=null){
			var item = itemManage.Items[itemslot.Index];
			GUI.Box(new Rect(10 + position.x,10 + position.y,50,50),"");	
			GUI.DrawTexture(new Rect(10 + position.x,10 + position.y,50,50),item.Icon);
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(position.x+70,position.y,100,60),item.Name);
		 	
			if(!characterInventory)
				return;
			
			if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Buy "+item.Price+"$")){
				if(characterInventory.Money >= item.Price){
					characterInventory.AddItem(itemslot.Index,1);
					characterInventory.Money -= item.Price;
				}
			}
		}
	}
	
	public void DrawItemBoxSellDetail(ItemSlot itemslot,Vector2 position){
		if(itemslot!=null){
			var item = itemManage.Items[itemslot.Index];
			GUI.Box(new Rect(10 + position.x,10 + position.y,50,50),"");	
			GUI.DrawTexture(new Rect(10 + position.x,10 + position.y,50,50),item.Icon);
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(14+position.x,14+position.y,30,30),itemslot.Num.ToString());
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(position.x+70,position.y,100,60),item.Name);
			
		 	if(!characterInventory)
				return;
			
			if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Sell "+item.Price+"$")){
				characterInventory.RemoveItem(itemslot,1);
				characterInventory.Money += item.Price;
			}
			
		}
	}
}
