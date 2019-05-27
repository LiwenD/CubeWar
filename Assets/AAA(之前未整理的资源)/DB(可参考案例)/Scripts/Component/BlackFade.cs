using UnityEngine;
using System.Collections;

public class BlackFade : MonoBehaviour {

	public Texture2D BG;
	public float Alpha;
	private float alphaTarget;
	public float Delay = 0.5f;
	void Start () {
		alphaTarget = Alpha;
	}
	
	void OnGUI(){
		GUI.depth = 1;
		if(Alpha > 0.01f){
			GUI.color = new Color(1,1,1,Alpha);
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),BG);
		}
	}
	public void Fade(float start,float end,float delay){
		Alpha = start;
		Delay = delay;
		alphaTarget = end;
	}
	public void Fade(float start,float end){
		Alpha = start;
		Delay = 1;
		alphaTarget = end;
	}
	
	void Update () {
		Alpha = Mathf.Lerp(Alpha,alphaTarget,Delay*Time.deltaTime);
	}
}
