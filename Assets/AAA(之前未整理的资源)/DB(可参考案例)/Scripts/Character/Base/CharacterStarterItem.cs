/// <summary>
/// Character starter item. Using for adding an item into your character
/// </summary>
using UnityEngine;
using System.Collections;

public class CharacterStarterItem : MonoBehaviour {

	public int[] StarterItem;
	
	void Start () {
		CharacterInventory character = this.gameObject.GetComponent<CharacterInventory>();
		if(character){
			for(int i=0;i<StarterItem.Length;i++){
				character.AddItem(StarterItem[i],1);
			}
		}
	}

}
