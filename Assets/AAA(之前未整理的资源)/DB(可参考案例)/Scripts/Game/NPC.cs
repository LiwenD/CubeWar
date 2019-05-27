using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC:MonoBehaviour {

	public GUISkin skin;
	public string Text = "NPC name";
	public QuestBase[] Quest;
	public int[] ItemShopIndex;
	
	private int npcState;
	private Vector2 scrollPosition;
	private List<ItemSlot> itemshopList;
	private List<QuestBase> questList;
	private ItemRenderer itemRenderer;
	private PlayerManager playerManage;
	private QuestRenderer questRenderer;
	
		
	void Start () {
		
		itemRenderer = new ItemRenderer(new Vector2(Screen.width-320,20),null);
		questRenderer = new QuestRenderer(new Vector2(Screen.width-320,20),null);
		// item in shop
		itemshopList = new List<ItemSlot>();
		ItemManager itemManage = (ItemManager)GameObject.FindObjectOfType(typeof(ItemManager));
		if(itemManage!=null){
			for(int i=0;i<ItemShopIndex.Length;i++){
				if(ItemShopIndex[i] < itemManage.Items.Length){
					ItemSlot item = new ItemSlot();
					item.Index = ItemShopIndex[i];
					itemshopList.Add(item);
				}
			}
		}
		questList = new List<QuestBase>();
		for(int i=0;i<Quest.Length;i++){
			questList.Add(Quest[i]);	
		}
		
	}
	void Update(){
		itemRenderer.Update();	
		questRenderer.Update();
	}
	
	void drawItemShop(){
		if(playerManage){
			itemRenderer.Draw(itemshopList,playerManage.Player.GetComponent<CharacterInventory>(),ItemRenderMode.Buy,"Item Shop  "+playerManage.Player.GetComponent<CharacterInventory>().Money+"$");
			if(!itemRenderer.Show){
				npcState = 0;	
			}
		}
	}
	
	void drawItemSell(){
		if(playerManage){
			itemRenderer.Draw(playerManage.Player.GetComponent<CharacterInventory>().ItemSlots,playerManage.Player.GetComponent<CharacterInventory>(),ItemRenderMode.Sell,"Sell Item");
			if(!itemRenderer.Show){
				npcState = 0;	
			}
		}
	}
	
	void drawQuest(){
		if(playerManage){
			questRenderer.Draw(questList,"Quest",QuestRenderMode.NonActive);
			if(!questRenderer.Show){
				npcState = 0;	
			}
		}
	}
	
	
	void OnGUI(){
		if(skin)
			GUI.skin = skin;
		
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);	
		var dir	= (Camera.main.transform.position - this.transform.position).normalized;
	    var direction = Vector3.Dot(dir,Camera.main.transform.forward);
	    
		switch(npcState){
		case 0:
	
			if(direction < 0.6f){
				Vector2 textsize = GUI.skin.label.CalcSize(new GUIContent(Text));
				if(GUI.Button(new Rect(screenPos.x - (textsize.x + 20)/2,Screen.height - screenPos.y,textsize.x + 20,30),Text)){
					npcState = 1;
					playerManage = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));
				}
			}
			break;
		case 1:
			if(GUI.Button(new Rect(screenPos.x - 75,Screen.height - screenPos.y,150,30),"Shop")){
				itemRenderer.Show = true;
				npcState = 2;
			}
			
			if(GUI.Button(new Rect(screenPos.x - 75,(Screen.height - screenPos.y) + 35,150,30),"Sell")){
				itemRenderer.Show = true;
				npcState = 3;
			}
			
			if(GUI.Button(new Rect(screenPos.x - 75,(Screen.height - screenPos.y) + 70,150,30),"Quest")){
				questRenderer.Show = true;
				npcState = 4;
			
			}
			
			if(GUI.Button(new Rect(screenPos.x - 75,(Screen.height - screenPos.y) + 105,150,30),"Bye")){
				npcState = 0;
			}
			
			break;
		case 2:
			drawItemShop();
			break;
		case 3:
			drawItemSell();
			break;
		case 4:
			drawQuest();
			break;
		}
	}
}
