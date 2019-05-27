/// <summary>
/// Shock wave skill.
/// Move to direction and Spawing an object multiple time
/// </summary>

using UnityEngine;
using System.Collections;

public class ShockWaveSkill : SkillBase
{
	public GameObject Skill;
	public float Speed;
	public float Spawnrate;
	public float LifeTime = 1;
	private float timeTemp;
	void Start()
	{
		GameObject.Destroy(this.gameObject,LifeTime);
		
		// Shaking a camera
		ShakeCamera.Shake(0.2f,0.7f);
	}

	void Update()
	{
		this.transform.position += this.transform.forward * Speed * Time.deltaTime;
		if(Time.time > timeTemp + Spawnrate)
		{
			if(Skill){
				var skillSpawned = (GameObject)GameObject.Instantiate(Skill,this.transform.position,Quaternion.identity);
				if(skillSpawned.GetComponent<SkillBase>()){
					skillSpawned.GetComponent<SkillBase>().Owner = Owner;
				}
			}
			timeTemp = Time.time;
		}
	}
}
