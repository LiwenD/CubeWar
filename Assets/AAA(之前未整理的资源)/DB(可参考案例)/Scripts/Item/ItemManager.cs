/// <summary>
/// Item manager.
/// Is the Game Item Database using for reference to inventory system and item pickup
/// all of item that using in the game must be declared and setting in this class
/// </summary>

using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
	
	public ItemCollector[] Items;

	void Awake()
	{

		Debug.Log("Setting All Items");
	}
}





