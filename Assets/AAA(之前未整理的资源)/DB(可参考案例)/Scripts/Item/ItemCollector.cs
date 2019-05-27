/// <summary>
/// Item type. is an Item Collector Structure 
/// using for show up in the Inventory GUI System
/// included
/// - Price
/// - ItemType
/// - Description
/// - Icon
/// - Etc..
/// </summary>


using UnityEngine;
using System.Collections;

public enum ItemType
{
	Weapon	= 0,
	Edible	= 1,
}
public enum WeaponType
{
	Melee	= 0,
	Ranged	= 1,
}

[System.Serializable]
public class ItemCollector
{
	public Texture2D Icon;
	public string Name = "Item Name";
	public string Description = "Description";
	public int Price;
	public ItemType ItemType;
	public GameObject ItemPrefab;
	public GameObject ItemPrefabDrop;
}

public enum ItemRenderMode{
	Player,
	Sell,
	Buy	
}