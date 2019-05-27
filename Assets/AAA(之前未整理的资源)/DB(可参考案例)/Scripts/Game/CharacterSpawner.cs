using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour {

	public void Spawn(GameObject CharacterSlected){
		if(CharacterSlected){
			GameObject player = (GameObject)GameObject.Instantiate(CharacterSlected,this.transform.position,Quaternion.identity);
			if(!player.GetComponent<PlayerManager>()){
				player.AddComponent<PlayerManager>();
			}
		}
	}
	
}
