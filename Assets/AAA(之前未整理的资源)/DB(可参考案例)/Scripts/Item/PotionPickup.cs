/// <summary>
/// Potion pickup.
/// this is the active item when a player pick it up.
/// </summary>

using UnityEngine;
using System.Collections;


public class PotionPickup : MonoBehaviour {

	public GameObject ParticlePotion;
	public AudioClip SoundPickup;
	
	void OnTriggerEnter(Collider other) {
		
		if(other.tag == "Player"){
       		if(other.gameObject.GetComponent<CharacterStatus>()){
				other.gameObject.GetComponent<CharacterStatus>().HP+= 20;
				if(ParticlePotion){
					GameObject.Instantiate(ParticlePotion,this.transform.position,Quaternion.identity);
				}
				if(SoundPickup){
					AudioSource.PlayClipAtPoint(SoundPickup,this.transform.position);	
				}
				GameObject.Destroy(this.gameObject.transform.parent.gameObject);	
				
			}
		}
    }
}
