/// <summary>
/// Floating3d.
/// Text Object mover
/// </summary>

using UnityEngine;
using System.Collections;

public class Floating3D : MonoBehaviour {

	public Vector3 PositionMult;
	public Vector3 PositionDirection;
	private Vector3 positionTemp;
	private FloatingText floatingtext;
	
	void Start () {
		positionTemp = this.transform.position;	
	}

	void Update () {
		positionTemp += PositionDirection * Time.deltaTime;
		PositionDirection += PositionMult * Time.deltaTime;
		this.transform.position = Vector3.Lerp(this.transform.position,positionTemp,0.5f);
	}
}
