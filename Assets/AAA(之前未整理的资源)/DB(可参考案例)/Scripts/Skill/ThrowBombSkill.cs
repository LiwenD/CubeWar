/// <summary>
/// Throw bomb skill. Thrower Object
/// </summary>

using UnityEngine;
using System.Collections;

public class ThrowBombSkill : MonoBehaviour {

	public float Dulation = 3;
	public GameObject DamageSkill;
	public float Force = 300;

	
	void Start () {
		if(GetComponent<Rigidbody>()){
			this.GetComponent<Rigidbody>().AddForce((this.transform.forward + Vector3.up) * Force);
			this.GetComponent<Rigidbody>().AddTorque((this.transform.forward + Vector3.up) * Force);	
		}
		StartCoroutine(countdown());
	}
	
	
	IEnumerator countdown()
    {
        while(true)
        {
			
            yield return new WaitForSeconds(Dulation);
			
			GameObject.Instantiate(DamageSkill,this.transform.position,Quaternion.identity);
			GameObject.Destroy(this.gameObject);
        }
    }
}
