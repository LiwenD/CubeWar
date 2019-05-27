/// <summary>
/// Test quest2. Quest Collected item Sample
/// </summary>

using UnityEngine;
using System.Collections;

public class TestQuest2 : QuestBase {
	
	int itemNum = 0;
	
	public override void Info ()
	{
		QuestName = "Collect Potion";
		Description = "Collected 5 potion and get $300";
		FinishText = "Well Done!! get your reward $300";
	}
	public override void Initialize (GameObject player)
	{
		Info();
		itemNum = 0;
		IsSuccess = false;
		base.Initialize (player);
	}
	
	public override void Update () {
		Checking();
		if(IsSuccess){
			QuestDisplay = QuestName + " Success";
		}else{
			QuestDisplay = QuestName + " ("+itemNum+"/5)";
		}
		base.Update();
	}
	
	public override void Checking ()
	{
		if(Player){
			if(Player.GetComponent<CharacterInventory>()){
				itemNum = Player.GetComponent<CharacterInventory>().GetItemNum(4);
			}
		}
		if(itemNum>=5){
			IsSuccess = true;	
		}else{
			IsSuccess = false;	
		}
		
		
		base.Checking ();
	}
	public override void Rewarded ()
	{
		if(Player.GetComponent<CharacterInventory>()){
			Player.GetComponent<CharacterInventory>().Money += 300;
		}
		base.Rewarded ();
	}
}
