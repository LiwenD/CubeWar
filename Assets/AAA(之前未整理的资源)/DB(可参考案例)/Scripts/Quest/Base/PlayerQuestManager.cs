/// <summary>
/// Quest manager. Add into Player Character this class Contain a Quest management function such as AddQuest , RemoveQuest , Checking or etc..
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerQuestManager : MonoBehaviour {
	
	public GUISkin skin;
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
	
	public void QuestPreparing(QuestBase quest){
		quest.Info();
		currentQuestShowing = quest;
		showQuest = true;
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
		if(skin)
			GUI.skin = skin;
		
		DrawQuestList();
	}

	public void DrawQuestList(){
		if(Quests.Count>0){
			GUI.skin.label.normal.textColor = Color.white;
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label(new Rect(Screen.width-350,110,300,30),"Quests");
		}
		for(int i=0;i<Quests.Count;i++){
			if(Quests[i]!=null){
				DrawQuest(Quests[i],new Vector2(Screen.width-350,140+(i * 30)));
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
	}
}
