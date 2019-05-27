/// <summary>
/// Item inventory.
/// this file is Strcture of Embedded item using for adding into the Embedded prefab
/// Such as Weapon , Sword , Shield
/// </summary>

using UnityEngine;
using System.Collections;


public class ItemInventory : MonoBehaviour
{	
	public int Damage = 0;
	public int Defend = 0;
	public float Distance = 2;
	public WeaponType Type;
	public GameObject Missle;// Missile object using to throw out in ranged weapon type.
	public int ItemEmbedSlotIndex = 0; // index of Inventorys embeded
	public AudioClip[] SoundHit; // Soung when hit
	public AudioClip[] SoundLaunch;// Soung when hit
	public float SpeedAttack = 1; // Attack Speed
	public bool PrimaryWeapon;
	
	void Start () {
	
	}
	
	
	void Update () {
	
	}
}
