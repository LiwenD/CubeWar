/// <summary>
/// Mainmenu. Just Main menu GUI
/// </summary>

using UnityEngine;
using System.Collections;

public class Mainmenu : MonoBehaviour {

	public Texture2D LogoGame;
	public GUISkin skin;
	public int GameState = 0;
	public GameObject[] TargetLookat;

	void Start ()
	{

	}
	
	void OnGUI()
	{
		Screen.lockCursor = false;
		if(skin)
			GUI.skin = skin;

		GUI.DrawTexture(new Rect(Screen.width/2 - (LogoGame.width*0.5f)/2,Screen.height/2 - 200,LogoGame.width *0.5f,LogoGame.height*0.5f),LogoGame);	
		
		
		switch(GameState){
		case 0:{
			
			if(GUI.Button(new Rect(Screen.width/2 - 80,Screen.height/2 +20,160,30),"Start Demo"))
				GameState = 1;
			}
			if(GUI.Button(new Rect(Screen.width/2 - 80,Screen.height/2 +60,160,30),"Purchase"))
			{
				Application.OpenURL ("https://www.assetstore.unity3d.com/#/content/10043");
			}
			break;
		case 1:
			if(GUI.Button(new Rect(50,50,160,30),"Back"))
			{
				GameState = 0;
			}
			break;
		}
		
		GUI.skin.label.fontSize = 12;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.Label(new Rect(0,Screen.height - 50,Screen.width,30),"Dungeon Breaker Starter Kit 2.0.  By Rachan Neamprasert | www.hardworkerstudio.com");
	}

	void Update ()
	{
		if(TargetLookat.Length>0){
			this.transform.position = Vector3.Lerp(this.transform.position,TargetLookat[GameState].transform.position,1.0f * Time.deltaTime);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,TargetLookat[GameState].transform.rotation,1.0f * Time.deltaTime);	
		}
	}
}
