using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum QuestRenderMode{
	NonActive,
	Active,
}

public class QuestRenderer : Frame {
	
	private QuestBase currentQuestShowing;
	private Vector2 scrollPosition;
	private int step;
	
	
	public QuestRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){	
	}

	public void Draw(List<QuestBase> quests,string headertext,QuestRenderMode rendermode){
		if(Show && quests!=null){
			GUI.BeginGroup(new Rect(Position.x,Position.y,300,380));
			GUI.Box(new Rect(0,0,300,380),"");
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.normal.textColor = Color.white;
			if(GUI.Button(new Rect(240,340,50,30),"Close")){
				Show = false;
				step = 0;
			}	
			if(GUI.Button(new Rect(0,0,300,30),headertext)){
				OnDraging();
			}	
			switch(step){
			case 0:
				DrowList(quests,rendermode);
				break;
			case 1:
				DrawQuest();
				break;
			}
		
			GUI.EndGroup();
		}
	}
	
	
	private void DrawQuest(){
		PlayerQuestManager questManager = (PlayerQuestManager)FindObjectOfType(typeof(PlayerQuestManager));
		if(questManager!=null){
			if(currentQuestShowing){
				GUI.Label(new Rect(20,30,280,200),"Quest "+currentQuestShowing.QuestName);
				string text = currentQuestShowing.Description;
				if(questManager.QuestHavedCheck(currentQuestShowing)){
					if(currentQuestShowing.IsSuccess){
						text = currentQuestShowing.FinishText;
						if(GUI.Button(new Rect(10,340,80,30),"Complete")){
							questManager.QuestCompleteCheck(currentQuestShowing);
							Show = false;
							step = 0;
						}
					}else{
						if(GUI.Button(new Rect(10,340,80,30),"Abandoned")){
							questManager.RemoveQuest(currentQuestShowing);
							Show = false;
							step = 0;
						}
					}
				}else{
					if(GUI.Button(new Rect(10,340,80,30),"Accept")){
						questManager.AddQuest(currentQuestShowing);
						Show = false;
						step = 0;
					}
				}

				GUI.TextArea(new Rect(10,60,280,120),text);
			}	
		}
	}
	
	
	private void DrowList(List<QuestBase> quests,QuestRenderMode rendermode){
		if(quests!=null){
			scrollPosition = GUI.BeginScrollView(new Rect(0, 40, 300, 290), scrollPosition, new Rect(0, 50, 280, quests.Count * 40));
			for(int i=0;i<quests.Count;i++){
				quests[i].Info();
				
				switch(rendermode){
				case QuestRenderMode.NonActive:
					if(GUI.Button(new Rect(10,40*i+50,275,35),quests[i].QuestName)){
						PlayerQuestManager questmanage = (PlayerQuestManager)FindObjectOfType(typeof(PlayerQuestManager));
						currentQuestShowing = quests[i];
						step = 1;
					}
					break;
				case QuestRenderMode.Active:
					if(GUI.Button(new Rect(10,40*i+50,275,35),quests[i].QuestDisplay)){
						PlayerQuestManager questmanage = (PlayerQuestManager)FindObjectOfType(typeof(PlayerQuestManager));
						currentQuestShowing = quests[i];
						step = 1;
					}
					
					break;
				}
			}
			GUI.EndScrollView();
		}
	}
}
