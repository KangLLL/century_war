using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class CharacterFactory : MonoBehaviour 
{
	[SerializeField]
	private BattleSceneHelper m_SceneHelper;
	[SerializeField]
	private GridFactory m_GridFactory;
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	[SerializeField]
	private GameObject m_SelectionGroup;
	[SerializeField]
	private Transform m_ParentNode;
	
	private List<ConstructArmyCommand> m_ConstructArmyList;
	private List<UserCommand<MercenaryIdentity>> m_ConstructMercenaryList;
	private List<UsePropsCommand> m_UsePropsList;
	
	[SerializeField]
	private Transform m_BulletParent;
	
	void Start()
	{
		this.m_ConstructArmyList = new List<ConstructArmyCommand>();
		this.m_ConstructMercenaryList = new List<UserCommand<MercenaryIdentity>>();
		this.m_UsePropsList = new List<UsePropsCommand>();
	}
	
	private void ConstructCharacter(GameObject characterPrefab, Vector3 position, BuildingCategory favoriteCategory,
		int hp, int armorCategory, float moveVelocity, TargetType type, float attackCD, int attackValue, float attackScope, 
		int damageScope, float middleSpeed,int attackCategory, TargetType targetType, float pushFactor, float pushAttenuateFactor)
	{
		GameObject army = GameObject.Instantiate(characterPrefab) as GameObject;
		army.transform.position = position;
		army.transform.parent = this.m_ParentNode;
		
		CharacterAI ai = army.GetComponent<CharacterAI>();
		ai.BattleMapData = BattleMapData.Instance;
		ai.BattleSceneHelper = this.m_SceneHelper;
		ai.FavoriteCategory = favoriteCategory;
		ai.PushFactor = pushFactor;
		ai.PushAttenuateFactor = pushAttenuateFactor;
		
		ai.SetIdle(true);
		
		CharacterHPBehavior hpBehavior = army.GetComponent<CharacterHPBehavior>();
		hpBehavior.TotalHP = hp;
		hpBehavior.ArmorCategory = armorCategory;
		if(hpBehavior is KodoHPBehavior)
		{
			KodoHPBehavior kodoHP = hpBehavior as KodoHPBehavior;
			kodoHP.Factory = this;
		}
		
		CharacterPropertyBehavior property = army.GetComponent<CharacterPropertyBehavior>();
		property.CharacterType = CharacterType.Invader;
		property.MoveVelocity = moveVelocity;
		property.Type = type;
		
		AttackBehavior attackBehavior = army.GetComponent<AttackBehavior>();
		attackBehavior.BulletParent = this.m_BulletParent;
		attackBehavior.AttackCD = Mathf.FloorToInt(attackCD * ClientConfigConstants.Instance.TicksPerSecond);
		attackBehavior.AttackValue = attackValue;
		attackBehavior.AttackScope = attackScope;
		attackBehavior.DamageScope = damageScope;
		attackBehavior.BulletFlySpeed = middleSpeed;
		attackBehavior.AttackCategory = attackCategory;
		attackBehavior.TargetType = targetType;
		BattleSceneHelper.Instance.ConstructActor
			(army, PositionConvertor.GetActorTileIndexFromWorldPosition(army.transform.position));
	}
	
	public void UseProps(PropsType propsType, Vector3 position)
	{
		//AudioController.Play("PutdownSoldier");
		object functionData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsType).FunctionConfigData;
			
		GameObject propsPrefab = this.m_PreloadManager.GetPropsPrefab(propsType);
		
		GameObject props = GameObject.Instantiate(propsPrefab) as GameObject;
		props.transform.position = position;
		props.transform.parent = this.m_BulletParent;
		
		AttackPropsBehavior propsBehavior = props.GetComponent<AttackPropsBehavior>();
		propsBehavior.ParentNode = BattleObjectCache.Instance.RuinsObjectParent.transform;
		
		if(functionData is PropsScopeConfigData)
		{
			PropsScopeConfigData propsScopeConfigData = functionData as PropsScopeConfigData;
			AttackScopeDamageBehavior scopeBehavior = props.GetComponent<AttackScopeDamageBehavior>();
			scopeBehavior.AttackCategory = propsScopeConfigData.AttackCategory;
			scopeBehavior.Damage = propsScopeConfigData.Effect;
			scopeBehavior.Scope = propsScopeConfigData.Scope;
			scopeBehavior.TotalTimes = propsScopeConfigData.TotalTimes;
			scopeBehavior.IntervalTicks = propsScopeConfigData.IntervalTicks;
		}
		else if(functionData is PropsArmyConfigData)
		{
			PropsArmyConfigData propsArmyConfigData = functionData as PropsArmyConfigData;
			AttackArmyBehavior armyBehavior = props.GetComponent<AttackArmyBehavior>();
			armyBehavior.ArmyType = propsArmyConfigData.ArmyType;
			armyBehavior.ArmyLevel = propsArmyConfigData.Level;
			armyBehavior.IntervalTicks = propsArmyConfigData.IntervalTicks;
			armyBehavior.TotalTimes = propsArmyConfigData.TotalTimes;
			armyBehavior.Number = propsArmyConfigData.Number;
			armyBehavior.CharacterFactory = this;
		}
		else if(functionData is PropsMercenaryConfigData)
		{
			PropsMercenaryConfigData propsMercenaryConfigData = functionData as PropsMercenaryConfigData;
			AttackMercenaryBehavior mercenaryBehavior = props.GetComponent<AttackMercenaryBehavior>();
			mercenaryBehavior.MercenaryType = propsMercenaryConfigData.MercenaryType;
			mercenaryBehavior.IntervalTicks = propsMercenaryConfigData.IntervalTicks;
			mercenaryBehavior.TotalTimes = propsMercenaryConfigData.TotalTimes;
			mercenaryBehavior.Number = propsMercenaryConfigData.Number;
			mercenaryBehavior.CharacterFactory = this;
		}
		else if(functionData is PropsTargetConfigData)
		{
			PropsTargetConfigData propsTargetConfigData = functionData as PropsTargetConfigData;
			AttackTargetBehavior targetBehavior = props.GetComponent<AttackTargetBehavior>();
			targetBehavior.Scope = propsTargetConfigData.Scope;
			targetBehavior.LastingTicks = propsTargetConfigData.LastingTicks;
			
			AttackTargetHPBehavior hpBehavior = props.GetComponent<AttackTargetHPBehavior>();
			hpBehavior.TotalHP = propsTargetConfigData.HP;
			hpBehavior.ArmorCategory = propsTargetConfigData.ArmorCategory;
		}
	}
	
	public void ConstructArmy(ArmyType armyType, int level, Vector3 position)
	{
		this.ConstructArmy(armyType, level, position, true);
	}
	
	public void ConstructArmy(ArmyType armyType, int level, Vector3 position, bool isPlaySound)
	{
		if(isPlaySound)
		{
			AudioController.Play("PutdownSoldier");
		}
		ArmyConfigData configData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(armyType, level);
		GameObject armyPrefab = this.m_PreloadManager.GetArmyPrefab(armyType, level);
		
		this.ConstructCharacter(armyPrefab, position,(BuildingCategory)configData.LogicFavoriteType,configData.MaxHP,configData.ArmorCategory,
			configData.MoveVelocity,(TargetType)configData.Type,configData.AttackCD,configData.AttackValue,configData.AttackScope, configData.DamageScope,
			configData.AttackMiddleSpeed,configData.AttackCategory,(TargetType)configData.TargetType,
			configData.PushFactor, configData.PushAttenuateFactor);
	}
	
	public void ConstructMercenary(MercenaryType mercenaryType, Vector3 position)
	{
		this.ConstructMercenary(mercenaryType, position, true);
	}
	
	public void ConstructMercenary(MercenaryType mercenaryType, Vector3 position, bool isPlaySound)
	{
		if(isPlaySound)
		{
			AudioController.Play("PutdownSoldier");
		}
		MercenaryConfigData configData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenaryType);
		GameObject mercenaryPrefab = this.m_PreloadManager.GetMercenaryPrefab(mercenaryType);
		
		this.ConstructCharacter(mercenaryPrefab, position,(BuildingCategory)configData.FavoriteType,configData.MaxHP,configData.ArmorCategory,
			configData.MoveVelocity,(TargetType)configData.Type,configData.AttackCD,configData.AttackValue,configData.AttackScope, configData.DamageScope,
			configData.AttackMiddleSpeed,configData.AttackCategory,(TargetType)configData.TargetType,
			configData.PushFactor, configData.PushAttenuateFactor);
	}
	
	public void Construct(Vector2 position)
	{
		TilePosition mousePosition = PositionConvertor.GetBuildingTileIndexFromScreenPosition(position);
		mousePosition.Row = Mathf.Clamp(mousePosition.Row, 0, ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height - 1);
		mousePosition.Column = Mathf.Clamp(mousePosition.Column, 0 , ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width - 1);

		UICheckbox check = null;
		
		UICheckbox[] checks = this.m_SelectionGroup.GetComponentsInChildren<UICheckbox>();
		foreach(UICheckbox c in checks)
		{
			if(c.isChecked)
			{
				check = c;
				break;
			}
		}
		
		PropsButtonInformation propsInfo = check.transform.parent.GetComponent<PropsButtonInformation>();
		
		bool isUseProps = propsInfo != null && 
			!(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsInfo.Type).FunctionConfigData is PropsArmyConfigData) &&
				!(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsInfo.Type).FunctionConfigData is PropsMercenaryConfigData);
		
		
		
		bool isValidPosition = true;
		
		if(mousePosition.IsEdgeBuildingTilePosition())
		{
			isValidPosition = true;
		}
		else if(!mousePosition.IsValidBuildingTilePosition())
		{
			 this.m_GridFactory.DisplayGrid();
			isValidPosition = false;
		}
		else if(BattleMapData.Instance.CharacterForbiddenArray[mousePosition.Row, mousePosition.Column] && 
			!isUseProps)
		{
			this.m_GridFactory.DisplayGrid();
			isValidPosition = false;
		}
		
		if(isValidPosition && (BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished))
		{
			ArmyButtonInformation armyInfo = check.transform.parent.GetComponent<ArmyButtonInformation>();
			MercenaryButtonInformation mercenaryInfo = check.transform.parent.GetComponent<MercenaryButtonInformation>();

			if(armyInfo != null)
			{
				if(armyInfo.Dropable)
				{
					Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
					worldPosition = PositionConvertor.ClampWorldPositionOfBuildingTile(worldPosition);

					ArmyIdentity id = armyInfo.DropArmy();
					this.m_ConstructArmyList.Add(new ConstructArmyCommand()
						{ Position = worldPosition, 
							Identity = id, Level = armyInfo.ArmyLevel });
					
				}
				else
				{
					UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.NO_ARMY_TO_DROP_WARNING_MESSAGE);
				}
			}
			else if(mercenaryInfo != null)
			{
				if(mercenaryInfo.Dropable)
				{
					Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
					worldPosition = PositionConvertor.ClampWorldPositionOfBuildingTile(worldPosition);

					MercenaryIdentity id = mercenaryInfo.DropArmy();
					this.m_ConstructMercenaryList.Add(new UserCommand<MercenaryIdentity>()
						{ Position = worldPosition, 
							Identity = id });
				}
				else
				{
					UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.NO_ARMY_TO_DROP_WARNING_MESSAGE);
				}
			}
			else if(propsInfo != null)
			{
				if(propsInfo.Dropable)
				{
					Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
					worldPosition = PositionConvertor.ClampWorldPositionOfBuildingTile(worldPosition);

					int no = propsInfo.DropArmy();
					this.m_UsePropsList.Add(new UsePropsCommand()
						{ Position = worldPosition, 
							Identity = no, PropsType = propsInfo.Type });
				
				}
				else
				{
					UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.NO_PROPS_TO_DROP_WARNING_MESSAGE);
				}
			}
		}
		else
		{
			if(isUseProps)
			{
				UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.CAN_NOT_USE_PROPS_WARNING_MESSAGE);
			}
			else
			{
				UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.CAN_NOT_DROP_ARMY_WARNING_MESSAGE);
			}
		}
	}
	
	void FixedUpdate()
	{
		if(BattleDirector.Instance != null)
		{
			if(!BattleDirector.Instance.IsBattleStart && 
				(this.m_ConstructArmyList.Count > 0 || this.m_ConstructMercenaryList.Count > 0
				|| this.m_UsePropsList.Count > 0))
			{
				BattleDirector.Instance.StartBattle();
			}
		}
		foreach(ConstructArmyCommand constructCommand in this.m_ConstructArmyList)
		{
			this.ConstructArmy(constructCommand.Identity.armyType, constructCommand.Level, constructCommand.Position);
			BattleRecorder.Instance.RecordDropArmy(constructCommand);
		}
		foreach (UserCommand<MercenaryIdentity> constructCommand in this.m_ConstructMercenaryList) 
		{
			this.ConstructMercenary(constructCommand.Identity.mercenaryType, constructCommand.Position);
			BattleRecorder.Instance.RecordDropMercenary(constructCommand);
		}
		foreach (UsePropsCommand constructCommand in this.m_UsePropsList) 
		{
			this.UseProps(constructCommand.PropsType, constructCommand.Position);
			BattleRecorder.Instance.RecordUseProps(constructCommand);
		}
		
		this.m_ConstructArmyList.Clear();
		this.m_ConstructMercenaryList.Clear();
		this.m_UsePropsList.Clear();
	}
}
