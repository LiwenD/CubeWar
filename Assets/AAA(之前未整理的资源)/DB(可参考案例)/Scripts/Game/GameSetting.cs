using UnityEngine;
using System.Collections;

public class GameSetting : MonoBehaviour {

	public string SceneNameStart;
	
	void Start () {
		Application.targetFrameRate = 60;
		Application.LoadLevel(SceneNameStart);
		BlackFade fader = (BlackFade)GameObject.FindObjectOfType(typeof(BlackFade));
		if(fader)
		fader.Fade(1,0);
	}

}
