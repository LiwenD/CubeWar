/// <summary>
/// Enemy spawner. auto Re-Spawning an Enemy by Random index of Objectman[]
/// </summary>


using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] Objectman; // Ememy object
	public float timeSpawn = 3;
	public int enemyCount = 10;
	public int radius;
	private float timetemp	= 0;
	private int indexSpawn;
	
	void Start () {
		indexSpawn = Random.Range(0,Objectman.Length);
		timetemp = Time.time;
	}
	

	void Update () {
		// just a basic function to spawing an enemys by random index of Objectman[]

		var gos	= GameObject.FindGameObjectsWithTag(Objectman[indexSpawn].tag);
   		if(gos.Length < enemyCount && Time.time > timetemp + timeSpawn)
		{
			
 			timetemp = Time.time;
			GameObject.Instantiate(Objectman[indexSpawn],transform.position + new Vector3(Random.Range(-radius,radius),transform.position.y,Random.Range(-radius,radius)),Quaternion.identity);
 			indexSpawn = Random.Range(0,Objectman.Length);
		}
	}
}
