/// <summary>
/// Damage skill.
/// will ApplayDamage() to <CharacterStatus>Object by around the radius
/// </summary>
using UnityEngine;
using System.Collections;

public class DamageSkill : SkillBase {

	public int Force;
	public float Radius;
	
	
	void Start () {
        var colliders = Physics.OverlapSphere(this.transform.position, Radius);
        foreach(var hit in colliders)
		{
            if(!hit)
            	continue;
				
			if(hit.tag == TagDamage){	
				if(hit.gameObject.GetComponent<CharacterStatus>()){
					hit.gameObject.GetComponent<CharacterStatus>().ApplayDamage(Damage,Vector3.zero,Owner);
				}
			}
			
			if (hit.GetComponent<Rigidbody>()){
                hit.GetComponent<Rigidbody>().AddExplosionForce(Force, transform.position, Radius, 3.0f);
			}
        }
	}

}
