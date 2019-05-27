/// <summary>
/// Adding this file into Enemy will do anything after dead
/// such as..
/// 1 . add some Score to GameManager.cs  
/// 2 . drop any Items after dead by random ItemDropAfterDead[]
/// </summary>


using UnityEngine;
using System.Collections;

public class EnemyDie : CharacterDie
{
	public GameObject[] ItemDropAfterDead;
	public int score = 1;

	
	public override void OnDead ()
	{
			
		if(ItemDropAfterDead.Length>0){
			// Drop item and adding some force
			int randomindex = Random.Range(0,ItemDropAfterDead.Length);
			if(ItemDropAfterDead[randomindex]!=null){
				GameObject item = (GameObject)Instantiate(ItemDropAfterDead[randomindex],this.gameObject.transform.position+ Vector3.up * 2,this.gameObject.transform.rotation);
				if(item.GetComponent<Rigidbody>()){
					item.GetComponent<Rigidbody>().AddForce((-this.transform.forward + Vector3.up) * 100);
					item.GetComponent<Rigidbody>().AddTorque((-this.transform.forward + Vector3.up) * 100);	
				}
				GameObject.Destroy(item,5);
			}
		}
		
		var gameManager	= (GameManager)FindObjectOfType(typeof(GameManager));
		if(gameManager){
			gameManager.Score += score;
		}
		var questmanager = (PlayerQuestManager)FindObjectOfType(typeof(PlayerQuestManager));
		if(questmanager){
			questmanager.ReadEventMessage("zombie1killed");
		}
		
		
		base.OnDead ();
	}
}
