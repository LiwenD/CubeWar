using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
	public SphericalVector Data	= new SphericalVector(0,0,1);
	protected virtual void Update()
	{
		gameObject.transform.position	= Data.Position;
	}
}
