using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
	
	public GUISkin skin;
	public string DoorName = "";
	public string SceneNameToGo = "";
	public string DoorID = "D1";
	private bool entering = false;
	private GameObject player;
	
	void Start(){
		PlayerManager playerManage = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));
		if(playerManage)
		playerManage.Teleport(this);	
	}
	
	
	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.GetComponent<PlayerManager>()){
			entering = true;
			player = collider.gameObject;
		}
	}
	
	void OnGUI(){
		if(skin)
			GUI.skin = skin;	
		
		if(!entering || !player)
			return;
		
		if(Vector3.Distance(player.transform.position,this.transform.position)>3){
			entering = false;	
		}
		
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + Vector3.up * 2);	
		
		if(GUI.Button(new Rect(screenPos.x - 75,Screen.height - screenPos.y,150,30),"Enter - "+DoorName)){
			if(SceneNameToGo!=""){
				PlayerManager playerManage = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));
				if(playerManage){
					playerManage.enterDoor(this);
					BlackFade fader = (BlackFade)GameObject.FindObjectOfType(typeof(BlackFade));
					if(fader)
					fader.Fade(1,0);
					Application.LoadLevel(SceneNameToGo);
				}
			}
		}
		
	}
}
