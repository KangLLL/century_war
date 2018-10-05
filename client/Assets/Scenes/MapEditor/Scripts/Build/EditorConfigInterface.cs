using UnityEngine;
using System.Collections;

public class EditorConfigInterface : MonoBehaviour 
{
	private static EditorConfigInterface s_Sigleton;
	
	public static EditorConfigInterface Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	[SerializeField]
	private GameObject m_CellPrefab;
	[SerializeField]
	private Camera m_SceneCamera;
	[SerializeField]
	private string m_MapStorePath;
	[SerializeField]
	private string m_MapSuffix;
	[SerializeField]
	private string m_MapBuildingKey;
	[SerializeField]
	private string m_MapRemovableObjectKey;
	[SerializeField]
	private string m_MapAchievementBuildingKey;
	[SerializeField]
	private string m_MapDefenseObjectKey;
	[SerializeField]
	private string m_MapBuildingTypeKey;
	[SerializeField]
	private string m_MapBuildingNoKey;
	[SerializeField]
	private string m_MapBuildingLevelKey;
	[SerializeField]
	private string m_MapBuildingRowKey;
	[SerializeField]
	private string m_MapBuildingColumnKey;
	[SerializeField]
	private string m_MapRemovableObjectTypeKey;
	[SerializeField]
	private string m_MapRemovableObjectRowKey;
	[SerializeField]
	private string m_MapRemovableObjectColumnKey;
	[SerializeField]
	private string m_MapAchievementBuildingTypeKey;
	[SerializeField]
	private string m_MapAchievementBuildingRowKey;
	[SerializeField]
	private string m_MapAchievementBuildingColumnKey;
	[SerializeField]
	private string m_MapDefenseObjectTypeKey;
	[SerializeField]
	private string m_MapDefenseObjectRowKey;
	[SerializeField]
	private string m_MapDefenseObjectColumnKey;
	[SerializeField]
	private string m_BattleTimeKey;
	[SerializeField]
	private string m_BattleArmyKey;
	[SerializeField]
	private string m_BattleMercenaryKey;
	[SerializeField]
	private string m_BattlePropsKey;
	[SerializeField]
	private string m_DropTypeKey;
	[SerializeField]
	private string m_ArmyLevelKey;
	[SerializeField]
	private string m_DropPositionXKey;
	[SerializeField]
	private string m_DropPositionYKey;
	[SerializeField]
	private string m_DropTimeKey;
	
	public GameObject CellPrefab { get { return this.m_CellPrefab; } }
	public Camera SceneCamera { get { return this.m_SceneCamera; } }
	public string MapStorePath { get { return this.m_MapStorePath; } }
	public string MapSuffix { get { return this.m_MapSuffix; } }
	public string MapBuildingKey { get { return this.m_MapBuildingKey; } }
	public string MapRemovableObjectKey { get { return this.m_MapRemovableObjectKey; } }
	public string MapAchievementBuildingKey { get { return this.m_MapAchievementBuildingKey; } }
	public string MapDefenseObjectKey { get { return this.m_MapDefenseObjectKey; } }
	public string MapBuildingTypeKey { get { return this.m_MapBuildingTypeKey; } }
	public string MapBuildingNoKey { get { return this.m_MapBuildingNoKey; } }
	public string MapBuildingLevelKey { get { return this.m_MapBuildingLevelKey; } }
	public string MapBuildingRowKey { get { return this.m_MapBuildingRowKey; } }
	public string MapBuildingColumnKey { get { return this.m_MapBuildingColumnKey; } }
	public string MapRemovableObjectTypeKey { get { return this.m_MapRemovableObjectTypeKey; } }
	public string MapRemovableObjectRowKey { get { return this.m_MapRemovableObjectRowKey; } }
	public string MapRemovableObjectColumnKey { get { return this.m_MapRemovableObjectColumnKey; } }
	public string MapAchievementBuildingTypeKey { get { return this.m_MapAchievementBuildingTypeKey; } }
	public string MapAchievementBuildingRowKey { get { return this.m_MapAchievementBuildingRowKey; } }
	public string MapAchievementBuildingColumnKey { get { return this.m_MapAchievementBuildingColumnKey; } }
	public string MapDefenseObjectTypeKey { get { return this.m_MapDefenseObjectKey; } }
	public string MapDefenseObjectRowKey { get { return this.m_MapDefenseObjectRowKey; } }
	public string MapDefenseObjectColumnKey { get { return this.m_MapDefenseObjectColumnKey; } }
	public string BattleTimeKey { get { return this.m_BattleTimeKey; } }
	public string BattleArmyKey { get { return this.m_BattleArmyKey; } }
	public string BattleMercenaryKey { get { return this.m_BattleMercenaryKey; } }
	public string BattlePropsKey { get { return this.m_BattlePropsKey; } }
	public string DropTypeKey { get { return this.m_DropTypeKey; } }
	public string ArmyLevelKey { get { return this.m_ArmyLevelKey; } }
	public string DropTimeKey { get { return this.m_DropTimeKey; } }
	public string DropPositionXKey { get { return this.m_DropPositionXKey; } }
	public string DropPositionYKey { get { return this.m_DropPositionYKey; } }
}
