/// <summary>
/// Character HU.
/// Using for Showing a Healt bar on the character object
/// </summary>


using UnityEngine;
using System.Collections;

public class CharacterHUD : MonoBehaviour {

	public GUISkin Skin;
	public bool AlwayShow;
	public Texture2D Bar_bg,Bar_hp,Bar_sp;
	CharacterStatus character;
	
	void Start () {
		if(this.gameObject.GetComponent<CharacterStatus>()){
			character = this.gameObject.GetComponent<CharacterStatus>();	
		}
	}

	void OnGUI(){

		if(Skin!=null)
		GUI.skin = Skin;
		
		if(Camera.main){
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);	
		var dir	= (Camera.main.transform.position - this.transform.position).normalized;
	    var direction = Vector3.Dot(dir,Camera.main.transform.forward);
	    
		if(direction < 0.6f){
			if(character){
				if(AlwayShow || character.HP < character.HPmax){
					GUI.BeginGroup(new Rect(screenPos.x - 42,Screen.height - screenPos.y + 20,84,28));
					GUI.DrawTexture(new Rect(0,0,84,9),Bar_bg);
					GUI.DrawTexture(new Rect(2,2,(80.0f / character.HPmax) * character.HP,5),Bar_hp);
					if(character.SPmax>0){
						GUI.DrawTexture(new Rect(0,9,84,7),Bar_bg);
						GUI.DrawTexture(new Rect(2,9,(80.0f / character.SPmax) * character.SP,5),Bar_sp);
					}
					GUI.EndGroup();
				}
			}	
		}
		}
		
	}
}
