/// <summary>
/// Damage hit active.
/// Spawing an Object when got hit by TriggerEnter
/// </summary>

using UnityEngine;
using System.Collections;

public class DamageHitActive : SkillBase {
	
	public GameObject explosiveObject;

	void OnTriggerEnter(Collider other) {
		if(other.tag == TagDamage){
			if(explosiveObject){
				var expspawned = (GameObject)GameObject.Instantiate(explosiveObject,this.transform.position,this.transform.rotation);
				if(expspawned.GetComponent<SkillBase>()){
					expspawned.GetComponent<SkillBase>().Owner = Owner;
				}
			}
			GameObject.Destroy(this.gameObject);
		}
	 	
	}
}
