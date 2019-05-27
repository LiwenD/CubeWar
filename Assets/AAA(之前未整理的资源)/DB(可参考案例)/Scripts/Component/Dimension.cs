using UnityEngine;

using System;
using System.Runtime.Serialization;

[Serializable]
public struct SphericalVector
{
	public float Length;
	public float Zenith;
	public float Azimuth;
	public SphericalVector(float azimuth,float zenith,float length)
	{
		Length	= length;
		Zenith	= zenith;
		Azimuth	= azimuth;
	}
	
	public Vector3 Position
	{
		get { return Length * Direction; }
	}
	
	public Vector3 Direction
	{
		get
		{
			Vector3 ret;
			var vangle	= Zenith * Mathf.PI / 2;
			ret.y	= Mathf.Sin(vangle);
			var h	= Mathf.Cos(vangle);
			var hangle	= Azimuth * Mathf.PI;
			ret.x	= h * Mathf.Sin(hangle);
			ret.z	= h * Mathf.Cos(hangle);
			return ret;
		}
	}
	
	public override string ToString()
	{
		return string.Format("Azimuth {0:0.0000} : Zenith {1:0.0000} : Length {2:0.0000}]",Azimuth,Zenith,Length);
	}
}
