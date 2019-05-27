using UnityEngine;
using System.Collections;

public class OrbitLookAt : Orbit
{
	public Vector3 LookAt	= Vector3.zero;
	void Start()
	{
		Data.Zenith	= -0.3f;
		Data.Length	= -6;
	}

	protected override void Update()
	{
		base.Update();
		gameObject.transform.position	+= LookAt;
		gameObject.transform.LookAt(LookAt);
	}
}
