using UnityEngine;
using System.Collections;

public class StatusRenderer : Frame {
	
	public StatusRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){
		
	}

	public void Draw(CharacterStatus characterStatus){
		if(Show){
		GUI.BeginGroup(new Rect(Position.x,Position.y,250,330));
		GUI.Box(new Rect(0,0,250,330),"");
		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
			
		if(GUI.Button(new Rect(0,0,250,30),"Status")){
			OnDraging();
		}	
		if(GUI.Button(new Rect(190,290,50,30),"Close")){
			Show = false;
		}
			
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 14;
		GUI.Label(new Rect(30,50,100,30),"STR");
		GUI.Label(new Rect(30,80,100,30),"AGI");
		GUI.Label(new Rect(30,110,100,30),"INT");
		GUI.Label(new Rect(30,140,100,30),"Point");
		GUI.Label(new Rect(30,170,100,30),"HP");
		GUI.Label(new Rect(30,200,100,30),"SP");
		GUI.Label(new Rect(30,230,100,30),"Damage");
		GUI.Label(new Rect(30,260,100,30),"Defend");
		
		GUI.Label(new Rect(120,50,100,30),""+characterStatus.STR);
		GUI.Label(new Rect(120,80,100,30),""+characterStatus.AGI);
		GUI.Label(new Rect(120,110,100,30),""+characterStatus.INT);
		GUI.Label(new Rect(120,140,100,30),""+characterStatus.StatusPoint);
		GUI.Label(new Rect(120,170,100,30),""+characterStatus.HP+" / "+characterStatus.HPmax);
		GUI.Label(new Rect(120,200,100,30),""+characterStatus.SP+" / "+characterStatus.SPmax);
		GUI.Label(new Rect(120,230,100,30),""+characterStatus.Damage);
		GUI.Label(new Rect(120,260,100,30),""+characterStatus.Defend);	
			
		if(characterStatus.StatusPoint>0){
			if(GUI.Button(new Rect(200,60,20,20),"+")){
				characterStatus.STR += 1;
				characterStatus.StatusPoint -= 1;
			}
			if(GUI.Button(new Rect(200,90,20,20),"+")){
				characterStatus.AGI += 1;
				characterStatus.StatusPoint -= 1;	
			}
			if(GUI.Button(new Rect(200,120,20,20),"+")){
				characterStatus.INT += 1;
				characterStatus.StatusPoint -= 1;	
			}
		}
		GUI.EndGroup();
		}
	}
	
}
