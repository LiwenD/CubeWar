/// <summary>
/// Floating text.
/// the GUI Text Floating system
/// </summary>

using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	public GUISkin CustomSkin;// GUISkin
	public string Text = "";// Text
	public float LifeTime = 1;// Life time
	public bool FadeEnd = false;// Fade out at last 1 second before destroyed
	public Color TextColor = Color.white; // Text color
	public bool Position3D = false; // enabled when you need the text along with world 3d position
	public Vector2 Position; // 2D Position
	public int FontSize = 22;
	private float alpha = 1;
	private float timeTemp = 0;

	void Start () {
		timeTemp = Time.time;
		GameObject.Destroy(this.gameObject,LifeTime);
		if(Position3D){
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			Position = new Vector2(screenPos.x,Screen.height - screenPos.y);
		}
	}

	void Update () {

		if(FadeEnd){
			if(Time.time >= ((timeTemp + LifeTime) - 1)){
				alpha = 1.0f - (Time.time - ((timeTemp + LifeTime) - 1));
			}
		}else{
			alpha = 1.0f - ((1.0f / LifeTime) * (Time.time - timeTemp));
		}
	
		if(Position3D){
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			Position = new Vector2(screenPos.x,Screen.height - screenPos.y);
		}
	
	}


	void OnGUI(){
		
		GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b,alpha);
		if(CustomSkin){
			GUI.skin = CustomSkin;
		}
		GUI.skin.label.fontSize = FontSize;
		Vector2 textsize = GUI.skin.label.CalcSize(new GUIContent(Text));
		Rect rect = new Rect(Position.x - (textsize.x/2), Position.y,textsize.x,textsize.y);

		GUI.skin.label.normal.textColor = TextColor;
		GUI.Label(rect,Text);

	}
}
