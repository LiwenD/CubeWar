using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(PlayerCharacterUI))]
[RequireComponent(typeof(PlayerQuestManager))]
[RequireComponent(typeof(PlayerSave))]

public class PlayerManager : MonoBehaviour {
	[HideInInspector]
	public GameObject Player;
	private Teleporter doorEnter;

	void Awake () {
		Player = this.gameObject;
		DontDestroyOnLoad(this.gameObject);
	}

	public void enterDoor(Teleporter doorIn){
		Debug.Log("Enter..");
		if(doorIn){
			doorEnter = doorIn;	
		}
	}
	public void Teleport(Teleporter doorOut){
		Debug.Log("Teleported");
		if(doorOut && doorOut.DoorID == doorEnter.DoorID){
			Player.transform.position = doorOut.transform.position;
		}
	}
	
	public void Spawn(GameObject CharacterSlected){
		if(CharacterSlected){
			GameObject player = (GameObject)GameObject.Instantiate(CharacterSlected,this.transform.position,Quaternion.identity);
			Player = player;
		}
	}

}
