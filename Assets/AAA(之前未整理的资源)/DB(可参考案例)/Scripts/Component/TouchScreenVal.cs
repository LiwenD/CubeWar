/// <summary>
/// Touch screen value. the touch screen controller System : Using one per Touch area
/// </summary>

using UnityEngine;
using System.Collections;

public class TouchScreenVal : MonoBehaviour {
	
	public Rect AreaTouch;
	private Vector2 controllerPositionTemp;
	private Vector2 controllerPositionNext;	
	
	
	public TouchScreenVal(Rect position){
		AreaTouch = position;
	}

	public bool OnTouchPress(){
		bool res = false;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch(i).position;
			if(touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && touchpos.y >= AreaTouch.yMin && touchpos.y <= AreaTouch.yMax){
				if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary){
                	res = true;
				}
			}
        }
		return res;
	}
	
	public Vector2 OnTouchDirection(){
		Vector2 direction = Vector2.zero;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch(i).position;
			if(touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && touchpos.y >= AreaTouch.yMin && touchpos.y <= AreaTouch.yMax){
				if(Input.GetTouch(i).phase == TouchPhase.Began){
					controllerPositionNext = new Vector2(Input.GetTouch(i).position.x,Screen.height - Input.GetTouch(i).position.y);
					controllerPositionTemp = controllerPositionNext;
				}else{
					controllerPositionNext = new Vector2(Input.GetTouch(i).position.x,Screen.height - Input.GetTouch(i).position.y);
					Vector2 deltagrag = (controllerPositionNext-controllerPositionTemp);
					direction.x	+= deltagrag.x;
					direction.y	-= deltagrag.y;
					//controllerPositionTemp = Vector2.Lerp(controllerPositionTemp,controllerPositionNext,0.5f);
				}	
			}
        }
		direction.Normalize();
		return direction;
	}
	
	
	
	
	public void Draw(Texture2D circle,bool bg){
		if(bg){
			GUI.DrawTexture(AreaTouch,circle);
		}
		GUI.DrawTexture(new Rect(controllerPositionNext.x-25,controllerPositionNext.y-25,50,50),circle);
		GUI.DrawTexture(new Rect(controllerPositionTemp.x-25,controllerPositionTemp.y-25,50,50),circle);
	}

}
