/// <summary>
/// Player character UI.
/// Basic Player UI will show up any information of Player Character
/// Such as Inventory System , Skill System , etc...
/// </summary>

using UnityEngine;
using System.Collections;


public class PlayerCharacterUI : ItemUI {

	public GUISkin skin;
	[HideInInspector]
	public CharacterStatus characterStatus;
	[HideInInspector]
	public CharacterSkillManager characterSkill;
	[HideInInspector]
	public PlayerQuestManager playerQuestManage;
	
	private Vector2 scrollPosition;
	private bool showItem,showStatus,showMouse;
	
	[HideInInspector]
	public StatusRenderer statueRenderer;
	[HideInInspector]
	public MainUIRenderer mainUIRenderer;
	[HideInInspector]
	public ItemRenderer itemRenderer;
	[HideInInspector]
	public QuestRenderer questRenderer;
	[HideInInspector]
	public ShotcutRenderer shotcutRenderer;
	[HideInInspector]
	public SkillRenderer skillRenderer;
	
	
	
	
	void Awake(){
		statueRenderer = new StatusRenderer(new Vector2(20,20),this);
		mainUIRenderer = new MainUIRenderer(new Vector2(20,Screen.height - 150),this);
		itemRenderer = new ItemRenderer(new Vector2(Screen.width-320,20),this);
		questRenderer = new QuestRenderer(new Vector2(Screen.width-320,20),this);
		shotcutRenderer = new ShotcutRenderer(new Vector2(450,Screen.height - 120),this);
		skillRenderer = new SkillRenderer(new Vector2(Screen.width-320,20),this);
	}

	void Start ()
	{	
		base.SettingItemUI();
		if(this.gameObject.GetComponent<CharacterStatus>()){
			characterStatus	= this.gameObject.GetComponent<CharacterStatus>();
		}
		if(this.gameObject.GetComponent<CharacterInventory>()){
			characterInventory = this.gameObject.GetComponent<CharacterInventory>();	
		}
		if(this.gameObject.GetComponent<CharacterSkillManager>()){
			characterSkill = this.gameObject.GetComponent<CharacterSkillManager>();	
		}
		if(this.gameObject.GetComponent<PlayerQuestManager>()){
			playerQuestManage = this.gameObject.GetComponent<PlayerQuestManager>();	
		}
	}

	void Update()
	{
		if(Screen.lockCursor && Input.GetKeyDown(KeyCode.E)){
			Screen.lockCursor = false;	
		}
		statueRenderer.Update();
		mainUIRenderer.Update();
		mainUIRenderer.Position = new Vector2(20,Screen.height - 150);
		itemRenderer.Update();
		questRenderer.Update();
		shotcutRenderer.Update();
		skillRenderer.Update();
	}
	

	

	
	void OnGUI()
	{
		if(skin)
			GUI.skin = skin;
		
		GUI.depth = 0;
		if(characterStatus)
		{
			mainUIRenderer.Draw(characterStatus);
			statueRenderer.Draw(characterStatus);
			itemRenderer.Draw(characterInventory.ItemSlots,characterInventory,ItemRenderMode.Player,"Inventory   "+characterInventory.Money+"$");
			shotcutRenderer.Draw();
			skillRenderer.Draw(characterSkill);
			
			if(playerQuestManage){
				questRenderer.Draw(playerQuestManage.Quests,"Quest",QuestRenderMode.Active);
			}
				
			if(!Screen.lockCursor){
				if(GUI.Button(new Rect(Screen.width-130,30,100,30),"Hide Cursor")){
					Screen.lockCursor = true;
				}
			}else{
				GUI.skin.label.fontSize = 17;
				GUI.skin.label.normal.textColor = Color.white;
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUI.Label(new Rect(Screen.width-330,30,300,30),"Press 'E' Show Cursor");		
			}
		}
	}
}
