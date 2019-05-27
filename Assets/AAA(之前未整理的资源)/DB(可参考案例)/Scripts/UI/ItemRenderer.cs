using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemRenderer : Frame {
	
	private ItemManager itemManage;
	private Vector2 scrollPosition;
	
	public ItemRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){
		itemManage = (ItemManager)FindObjectOfType(typeof(ItemManager));
	}
	
	public void Draw(List<ItemSlot> Items,CharacterInventory characterInventory,ItemRenderMode renderMode,string headertext){
		if(Show && Items!=null && Items.Count>0){
			GUI.BeginGroup(new Rect(Position.x,Position.y,300,380));
			GUI.Box(new Rect(0,0,300,380),"");
			
			if(GUI.Button(new Rect(240,340,50,30),"Close")){
				Show = false;
			}	
			if(GUI.Button(new Rect(0,0,300,30),headertext)){
				OnDraging();
			}	

		
			scrollPosition = GUI.BeginScrollView(new Rect(0, 40, 300, 290), scrollPosition, new Rect(0, 50, 280, Items.Count * 60));
			for(int i=0;i<Items.Count;i++){
				DrawItemBoxDetail(i,Items[i],new Vector2(0,(i*60) + 50),characterInventory,renderMode);
			}
			
			GUI.EndScrollView();
			GUI.EndGroup();
		}
	}
	
	
	
	
	public void DrawItemBoxDetail(int indexSlot,ItemSlot itemslot,Vector2 position,CharacterInventory characterInventory,ItemRenderMode renderMode){
		if(itemslot!=null){
			var item = itemManage.Items[itemslot.Index];
			GUI.skin.label.normal.textColor = Color.white;
			//GUI.Box(new Rect(10 + position.x,10 + position.y,50,50),"");	
			if(GUI.Button(new Rect(10 + position.x,10 + position.y,50,50),item.Icon)){
				if(renderMode == ItemRenderMode.Player){
					PlayerUI.shotcutRenderer.AddShopCut(indexSlot,0,1);	
				}
			}
			
			if(itemslot.Num>0){
				GUI.skin.label.fontSize = 13;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label(new Rect(14+position.x,14+position.y,30,30),itemslot.Num.ToString());
			}
			GUI.skin.label.fontSize = 13;
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(new Rect(position.x+70,position.y,100,60),item.Name+" "+itemslot.Index);
			
			if(!characterInventory)
				return;
			
			switch(renderMode){
				case ItemRenderMode.Buy:
					if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Buy "+item.Price+"$")){
						if(characterInventory.Money >= item.Price){
							characterInventory.AddItem(itemslot.Index,1);
							characterInventory.Money -= item.Price;
						}
					}
					break;
				case ItemRenderMode.Sell:
					if(GUI.Button(new Rect(200 + position.x, position.y+10,80,30),"Sell "+item.Price+"$")){
						characterInventory.RemoveItem(itemslot,1);
						characterInventory.Money += item.Price;
					}
					break;
				case ItemRenderMode.Player:
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
				break;
			}
		}
	}
}
