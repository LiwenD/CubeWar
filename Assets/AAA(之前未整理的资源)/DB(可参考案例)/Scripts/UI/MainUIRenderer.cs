using UnityEngine;
using System.Collections;

public class MainUIRenderer : Frame {
	
	Texture2D bg_bar,exp_bar,hp_bar,sp_bar;
	
	public MainUIRenderer(Vector2 position,PlayerCharacterUI ui) :base(position,ui){
		
		bg_bar = new Texture2D(1,1);
		bg_bar.SetPixel(0,0,new Color(0,0,0,0.8f));
		bg_bar.Apply();
		
		exp_bar = new Texture2D(1,1);
		exp_bar.SetPixel(0,0,new Color(0.3f,0.3f,0.3f,0.5f));
		exp_bar.Apply();

		hp_bar = new Texture2D(1,1);
		hp_bar.SetPixel(0,0,new Color(0.3f,1.0f,0.1f,1.0f));
		hp_bar.Apply();
		
		sp_bar = new Texture2D(1,1);
		sp_bar.SetPixel(0,0,new Color(0.0f,0.5f,0.9f,1.0f));
		sp_bar.Apply();
		
		Show = true;
	}
	
	public void Draw(CharacterStatus characterStatus){
		if(Show){
		GUI.BeginGroup(new Rect(Position.x,Position.y,400,120));
		//GUI.Box(new Rect(0,0,400,120),"");

		if(characterStatus.ThumbnailImage){
			GUI.DrawTexture(new Rect(10,10,100,100),characterStatus.ThumbnailImage);
		}
		
		GUI.DrawTexture(new Rect(120,52,255,10),bg_bar);	
		GUI.DrawTexture(new Rect(120,52,(255.0f/characterStatus.SPmax)*characterStatus.SP,10),sp_bar);		
			
		GUI.DrawTexture(new Rect(120,30,255,20),bg_bar);	
		GUI.DrawTexture(new Rect(120,30,(255.0f/characterStatus.HPmax)*characterStatus.HP,20),hp_bar);	
			
		GUI.DrawTexture(new Rect(120,64,255,13),bg_bar);	
		GUI.DrawTexture(new Rect(120,64,(255.0f/characterStatus.EXPmax)*characterStatus.EXP,13),exp_bar);
		GUI.skin.label.normal.textColor = Color.white;	
		GUI.skin.label.fontSize = 15;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(120,10,300,20),characterStatus.Name+"  Level."+characterStatus.LEVEL);
		GUI.skin.label.fontSize = 12;
		GUI.Label(new Rect(130,60,300,20),"Exp "+characterStatus.EXP + " / "+characterStatus.EXPmax);
		

		if(GUI.Button(new Rect(120,80,60,30),"Status")){
			PlayerUI.statueRenderer.Show = !PlayerUI.statueRenderer.Show;
		}
		if(GUI.Button(new Rect(185,80,60,30),"Skill")){
			PlayerUI.skillRenderer.Show = !PlayerUI.skillRenderer.Show;
		}
		if(GUI.Button(new Rect(250,80,60,30),"Item")){
			PlayerUI.itemRenderer.Show = !PlayerUI.itemRenderer.Show;
		}
		if(GUI.Button(new Rect(315,80,60,30),"Quests")){
			PlayerUI.questRenderer.Show = !PlayerUI.questRenderer.Show;
		}
		GUI.EndGroup();	
		}
	}
}
