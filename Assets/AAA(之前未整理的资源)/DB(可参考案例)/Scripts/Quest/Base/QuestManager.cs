/// <summary>
/// Quest manager. Contain a Quest management function such as AddQuest , RemoveQuest , Checking or etc..
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {
	
	
	public GameObject TextFloating;
	public List<QuestBase> Quests = new List<QuestBase>();
	private bool showQuest;
	private QuestBase currentQuestShowing;
	
	
	
	void Start () {
	}
	
	public bool QuestHavedCheck(QuestBase quest){
		bool res = false;
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				if(Quests[i] == quest){
					res = true;
					break;	
				}
			}
		}
		return res;
	}
	public void AddQuest(QuestBase quest){
		
		if(!QuestHavedCheck(quest)){
			Debug.Log("Add Quest "+quest.QuestName);
			addText("Quest "+quest.QuestName+" Accepted");
			quest.Initialize(this.gameObject);
			Quests.Add(quest);	
		}
	}
	public void RemoveQuest(QuestBase quest){
		if(QuestHavedCheck(quest)){
			Debug.Log("Remove Quest "+quest.QuestName);
			Quests.Remove(quest);
		}
	}
	
	public void ReadEventMessage(string message){
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				Quests[i].ActioneMessage(message);
			}
		}
	}
	public void QuestCompleteCheck(QuestBase quest){
		Debug.Log("Quest Check");
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				if(Quests[i] == quest){
					if(Quests[i].IsSuccess){
						Quests[i].Rewarded();
						if(TextFloating){
							addText("Quest Complete");
						}
						RemoveQuest(Quests[i]);
					}
				}
			}
		}
	}
	
	void Update () {
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				Quests[i].Update();
			}
		}
	}
	
	void addText(string text){
		if(TextFloating){
			GameObject floattext = (GameObject)GameObject.Instantiate(TextFloating,this.gameObject.transform.position,Quaternion.identity);	
			floattext.GetComponent<FloatingText>().Text = text;
		}	
	}
	

	
	void OnGUI(){
		DrawQuestList();
		
		// Draw quest detail dialog
		if(showQuest){
			if(currentQuestShowing){
				GUI.BeginGroup(new Rect(Screen.width/2 - 150,Screen.height/2 - 100,300,200));
				GUI.Box(new Rect(0,0,300,200),"Quest "+currentQuestShowing.QuestName);
				GUI.TextArea(new Rect(10,30,280,120),currentQuestShowing.Description);
				if(GUI.Button(new Rect(10,160,80,25),"Abaddon")){
					RemoveQuest(currentQuestShowing);
					showQuest = false;
				}
				if(GUI.Button(new Rect(210,160,80,25),"Close")){
					showQuest = false;
				}
			}
			GUI.EndGroup();
		}
	}
	
	public void DrawQuestList(){
		if(Quests.Count>0){
			GUI.skin.label.normal.textColor = Color.white;
			GUI.Label(new Rect(50,110,300,30),"Quests");
		}
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				DrawQuest(Quests[i],new Vector2(50,140+(i * 30)));
			}
		}
	}
	
	public void DrawQuest(QuestBase quest,Vector2 position){
		
		if(quest.IsSuccess){
			GUI.skin.label.normal.textColor = Color.green;
		}else{
			GUI.skin.label.normal.textColor = Color.yellow;
		}
		
		GUI.Label(new Rect(position.x,position.y,300,30),quest.QuestDisplay);
		if(GUI.Button(new Rect(position.x+200,position.y,50,20),"Info")){
			currentQuestShowing = quest;
			showQuest = true;
		}
	}
}
