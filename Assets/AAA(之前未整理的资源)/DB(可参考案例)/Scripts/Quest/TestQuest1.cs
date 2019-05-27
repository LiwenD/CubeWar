/// <summary>
/// Test quest1. Quest Kill Sample
/// </summary>

using UnityEngine;
using System.Collections;

public class TestQuest1 : QuestBase {

	int killzombienum = 0;
	
	public override void Info ()
	{
		QuestName = "Kill the Zombies";
		Description = "Kill 10 zombies and get $100 and 300 EXP";
		FinishText = "Well Done!! get your $100 and 300 EXP";
		base.Info ();
	}
	public override void Initialize (GameObject player)
	{
		Info();
		killzombienum = 0;
		IsSuccess = false;
		base.Initialize (player);
	}
	
	public override void Update () {
		Checking();
		if(IsSuccess){
			QuestDisplay = QuestName + " Success";
		}else{
			QuestDisplay = QuestName + " ("+killzombienum+"/10)";
		}
		base.Update();
	}
	
	public override void Checking ()
	{
		if(killzombienum >= 10){
			IsSuccess = true;
		}else{
			IsSuccess = false;
		}
		base.Checking ();
	} 
	
	public override void Rewarded ()
	{
		if(Player.GetComponent<CharacterInventory>()){
			Player.GetComponent<CharacterInventory>().Money += 100;
		}
		if(Player.GetComponent<CharacterStatus>()){
			Player.GetComponent<CharacterStatus>().ApplayEXP(100);	
		}
		base.Rewarded ();
	}
	
	public override void ActioneMessage (string message)
	{
		if(message=="zombie1killed"){
			killzombienum += 1;	
		}
		
		base.ActioneMessage (message);
	}

	
}
