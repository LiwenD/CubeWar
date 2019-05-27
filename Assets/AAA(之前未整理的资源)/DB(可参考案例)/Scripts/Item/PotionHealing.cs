/// <summary>
/// Potion healing.
/// this class using for adding into Potion item. 
/// will Recovery a HP of parent of this object 
/// **the parent must have <CharacterStatus>
/// </summary>

using UnityEngine;
using System.Collections;

public class PotionHealing : MonoBehaviour {

	public int HPheal = 10;
	
	
	void Start () {
		if(this.gameObject.transform.parent){
			// Update HP var in CharacterStatus.cs
			if(this.gameObject.transform.parent.gameObject.GetComponent<CharacterStatus>()){
				this.gameObject.transform.parent.gameObject.GetComponent<CharacterStatus>().HP += HPheal;
			}
		}
		GameObject.Destroy(this.gameObject,3);
	}
	

}
