/// <summary>
/// Item pickup.
/// Adding to Item for Pickup
/// 
/// IndexItem is an index of any Item reference by Item[] in ItemManager
/// 0 = Axe
/// 1 = Sword
/// 2 = Shield
/// 3 = etc....
/// </summary>

using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ItemPickup : MonoBehaviour {
	
	public bool DestroyWhenPickup = true;
	public AudioClip SoundPickup;
	public int IndexItem = 0;
	public int Num = 1;
	
	
	void Start(){
		
	}
	

	void OnTriggerStay(Collider other)
	{
       	if(other.gameObject.GetComponent<CharacterInventory>())
		{
			other.gameObject.GetComponent<CharacterInventory>().AddItem(IndexItem,Num);
			// Play Sound when player Pickup this item
			if(SoundPickup){
				AudioSource.PlayClipAtPoint(SoundPickup,Camera.main.gameObject.transform.position);	
			}
			
			
			if(DestroyWhenPickup){
				if(this.gameObject.transform.parent){
					GameObject.Destroy(this.gameObject.transform.parent.gameObject);
				}else{
					GameObject.Destroy(this.gameObject);
				}
			}
		}
    }
	
}
