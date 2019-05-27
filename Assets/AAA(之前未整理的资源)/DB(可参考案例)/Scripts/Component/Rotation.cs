using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	public Vector3 RotationAxis;
	
	void Start () {
	
	}

	void Update () {
		this.transform.Rotate(RotationAxis * Time.deltaTime);
	}
}
