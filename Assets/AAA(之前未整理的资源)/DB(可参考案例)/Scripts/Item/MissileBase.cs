using UnityEngine;
using System.Collections;

public class MissileBase : MonoBehaviour {

	public int Damage;
	public GameObject Owner;
	public GameObject ExplosiveObject;
	public float Speed = 4;
	public float LifeTime = 3;
	
	void Start(){
		Destroy(this.gameObject,LifeTime);	
	}
	
	void Update(){
		if(this.gameObject.GetComponent<Rigidbody>()){
			this.gameObject.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * Speed;
		}else{
			this.gameObject.transform.position += this.gameObject.transform.forward * Speed * Time.deltaTime;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.tag+" VS "+Owner.gameObject.tag+"    "+other.gameObject.name+" _ "+this.Owner.gameObject.name);
		if(other){
		if(other.tag != Owner.gameObject.tag){
			if(ExplosiveObject){
				GameObject expspawned = (GameObject)GameObject.Instantiate(ExplosiveObject,this.transform.position,this.transform.rotation);
				GameObject.Destroy(expspawned,2);
			}
			if(other.gameObject.GetComponent<CharacterStatus>()){
				other.gameObject.GetComponent<CharacterStatus>().ApplayDamage(Damage,Vector3.zero,Owner);
			}
			GameObject.Destroy(this.gameObject);
		}
		}
	}

}
