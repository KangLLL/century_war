using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ConfigUtilities.Enums;

public class ArmyMenuPopulator : MonoBehaviour 
{
	
	private static ArmyMenuPopulator s_Sigleton;
	
	[SerializeField]
	private PrefabDictionary m_ArmyPrefabs;
	[SerializeField]
	private PrefabDictionary m_MercenaryPrefabs;
	[SerializeField]
	private PrefabDictionary m_PropsPrefabs;
	
	[SerializeField]
	private GameObject m_PlaceHolderPrefab;
	[SerializeField]
	private UIDraggablePanel m_Panel;
	[SerializeField]
	private UIGrid m_Grid;
	
	private int m_TotalArmyCount;
	private int m_TotalMercenaryCount;
	private int m_TotalPropsCount;
	//private Dictionary<ArmyType, GameObject> m_ArmyPrefabDict;
	//private Dictionary<MercenaryType, GameObject> m_MercenaryPrefabDict;
	//private Dictionary<PropsType, GameObject> m_PropsPrefabDict;

	private int m_MinimunButtonCount;
	
	public int TotalArmyCount
	{
		get
		{
			return this.m_TotalArmyCount;
		}
	}
	
	public int TotalMercenaryCount
	{
		get
		{
			return this.m_TotalMercenaryCount;
		}
	}
	
	public int TotalPropsCount
	{
		get
		{
			return this.m_TotalPropsCount;
		}
	}
	
	public static ArmyMenuPopulator Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	public List<KeyValuePair<ArmyType, List<ArmyIdentity>>> AvailableArmies { get; set; } 
	public List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>> AvailableMercenaries { get; set; }
	public List<KeyValuePair<PropsType, List<int>>> AvailableProps { get;set; }
	public Dictionary<ArmyType, int> ArmyLevel { get; set; }

	void Awake()
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	// Use this for initialization
	void Start () 
	{
		UIPanel clipPanel = this.GetComponentInChildren<UIPanel>();
		
		this.m_MinimunButtonCount =(int)(clipPanel.clipRange.x / this.m_Grid.cellWidth);
		/*
		this.m_ArmyPrefabDict = new Dictionary<ArmyType, GameObject>(){
			{ArmyType.Berserker, this.m_BerserkerArmyButtonPrefab},
			{ArmyType.Ranger, this.m_RangerArmyButtonPrefab},
			{ArmyType.Marauder, this.m_MarauderArmyButtonPrefab}
		};
		this.m_MercenaryPrefabDict = new Dictionary<MercenaryType, GameObject>(){
			{MercenaryType.Slinger, this.m_MercenarySlingerButtonPrefab},
			{MercenaryType.Hercules, this.m_MercenaryHerculesButtonPrefab},
			{MercenaryType.Kodo, this.m_MercenaryKodoButtonPrefab},
			{MercenaryType.HerculesII, this.m_MercenaryHerculesIIButtonPrefab},
			{MercenaryType.KodoII, this.m_MercenaryKodoIIButtonPrefab},
			{MercenaryType.Arsonist, this.m_MercenaryArsonistButtonPrefab},
			{MercenaryType.ArsonistII, this.m_MercenaryArsonistIIButtonPrefab},
			{MercenaryType.PhalanxSoldier, this.m_MercenaryPhalanxSoldierButtonPrefab},
			{MercenaryType.Catapults, this.m_MercenaryCatapultsButtonPrefab},
			{MercenaryType.CrazyKodo, this.m_MercenaryCrazyKodoButtonPrefab}
		};
		this.m_PropsPrefabDict = new Dictionary<PropsType, GameObject>(){
			{PropsType.DamageBottle, this.m_PropsDamageBottleButtonPrefab},
			{PropsType.ArmyBerserkerExcellent, this.m_PropsArmyBerserkerExcellentButtonPrefab},
			{PropsType.ArmyBerserkerSophisticated, this.m_PropsArmyBerserkerSophisticatedButtonPrefab},
			{PropsType.ArmyBerserkerEpic, this.m_PropsArmyBerserkerEpicButtonPrefab},
			{PropsType.ArmyBerserkerLegend, this.m_PropsArmyBerserkerLegendButtonPrefab},
			{PropsType.ArmyRangerExcellent, this.m_PropsArmyRangerExcellentButtonPrefab},
			{PropsType.ArmyRangerSophisticated, this.m_PropsArmyRangerSophisticatedButtonPrefab},
			{PropsType.MercenaryArsonistExcellent, this.m_PropsMercenaryArsonistExcellentButtonPrefab},
			{PropsType.MercenaryCrazyKodoExcellent, this.m_PropsMercenaryCrazyKodoExcellentButtonPrefab},
			{PropsType.MercenaryCrazyKodoSophisticated, this.m_PropsMercenaryCrazyKodoSophisticatedButtonPrefab}
		};
		*/
		List<KeyValuePair<ArmyType, List<ArmyIdentity>>> availableArmies = this.AvailableArmies;
		if(availableArmies != null)
		{
			foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> armies in availableArmies)
			{
				GameObject armyButton = GameObject.Instantiate(this.m_ArmyPrefabs[armies.Key.ToString()]) as GameObject;
				armyButton.transform.parent = this.m_Grid.transform;
				
				armyButton.GetComponentInChildren<UICheckbox>().radioButtonRoot = this.m_Grid.transform;
				armyButton.name = armies.Key.GetUIGridSortString() + "_" + armyButton.name;
					
				ArmyButtonInformation info = armyButton.GetComponent<ArmyButtonInformation>();
				info.Type = armies.Key;
				info.ArmyLevel = this.ArmyLevel[armies.Key];
				info.Armies = armies.Value;
				this.m_TotalArmyCount += armies.Value.Count;
				
				UIDragPanelContents contents = armyButton.GetComponentInChildren<UIDragPanelContents>();
				contents.draggablePanel = this.m_Panel;
			}
		}
		
		SortedDictionary<int, KeyValuePair<MercenaryType, List<MercenaryIdentity>>> availableMercenaries = new SortedDictionary<int, KeyValuePair<MercenaryType, List<MercenaryIdentity>>>();
		if(this.AvailableMercenaries != null)
		{
			foreach (KeyValuePair<MercenaryType, List<MercenaryIdentity>> mercenary in this.AvailableMercenaries) 
			{
				availableMercenaries.Add(ClientConfigConstants.Instance.GetMercenaryOrder(mercenary.Key),
					mercenary);
			}
			
			SortedDictionary<int, KeyValuePair<MercenaryType, List<MercenaryIdentity>>>.Enumerator en = availableMercenaries.GetEnumerator();
		
			while (en.MoveNext()) 
			{
				KeyValuePair<MercenaryType, List<MercenaryIdentity>> mercenaries = en.Current.Value;
				GameObject mercenaryButton = GameObject.Instantiate(this.m_MercenaryPrefabs[mercenaries.Key.ToString()]) as GameObject;
				mercenaryButton.transform.parent = this.m_Grid.transform;
				
				mercenaryButton.GetComponentInChildren<UICheckbox>().radioButtonRoot = this.m_Grid.transform;
				mercenaryButton.name = mercenaries.Key.GetUIGridSortString() + "_" + mercenaryButton.name;
				
				MercenaryButtonInformation info = mercenaryButton.GetComponent<MercenaryButtonInformation>();
				info.Type = mercenaries.Key;
				info.Armies = mercenaries.Value;
				this.m_TotalMercenaryCount += mercenaries.Value.Count;
				
				UIDragPanelContents contents = mercenaryButton.GetComponentInChildren<UIDragPanelContents>();
				contents.draggablePanel = this.m_Panel;
			}
		}
		
		List<KeyValuePair<PropsType, List<int>>> availableProps = this.AvailableProps;
		if(availableProps != null)
		{
			foreach(KeyValuePair<PropsType, List<int>> props in availableProps)
			{
				GameObject propsButton = GameObject.Instantiate(this.m_PropsPrefabs[props.Key.ToString()]) as GameObject;
				propsButton.transform.parent = this.m_Grid.transform;
				
				propsButton.GetComponentInChildren<UICheckbox>().radioButtonRoot = this.m_Grid.transform;
				propsButton.name = props.Key.GetUIGridSortString() + "_" + propsButton.name;
					
				PropsButtonInformation info = propsButton.GetComponent<PropsButtonInformation>();
				info.Type = props.Key;
				info.Armies = props.Value;
				
				this.m_TotalPropsCount += props.Value.Count;
				UIDragPanelContents contents = propsButton.GetComponentInChildren<UIDragPanelContents>();
				contents.draggablePanel = this.m_Panel;
			}
		}
		
		/*
		foreach (int propsNo in LogicController.Instance.AvailableBattleProps) 
		{
			PropsLogicData logicData = LogicController.Instance.GetProps(propsNo);
			
			GameObject propsButton = GameObject.Instantiate(this.m_PropsPrefabs[logicData.PropsType.ToString()]) as GameObject;
			propsButton.transform.parent = this.m_Grid.transform;
			
			propsButton.GetComponentInChildren<UICheckbox>().radioButtonRoot = this.m_Grid.transform;
			propsButton.name = logicData.PropsType.GetUIGridSortString() + "_" + propsButton.name;
				
			PropsButtonInformation info = propsButton.GetComponent<PropsButtonInformation>();
			info.Type = logicData.PropsType;
			info.Armies = new List<int>();
			for(int i = 0; i < logicData.RemainingUseTime; i ++)
			{
				info.Armies.Add(propsNo);
			}
			
			this.m_TotalPropsCount += logicData.RemainingUseTime;
			
			UIDragPanelContents contents = propsButton.GetComponentInChildren<UIDragPanelContents>();
			contents.draggablePanel = this.m_Panel;
		}

		
		foreach(KeyValuePair<ItemType, List<ItemIdentity>> items in LogicController.Instance.AvailableItems)
		{
			GameObject itemButton = GameObject.Instantiate(this.m_ItemButtonPrefab) as GameObject;
			itemButton.transform.parent = this.m_Grid.transform;
			
			itemButton.GetComponentInChildren<UICheckbox>().radioButtonRoot = this.m_Grid.transform;
		}
		*/
		
		int validButtonCount = this.m_Grid.transform.childCount;
		if(validButtonCount > this.m_MinimunButtonCount)
		{
			//this.m_Panel.dragEffect = UIDraggablePanel.DragEffect.MomentumAndSpring;
		}
		else
		{
			for(int i = 0; i < this.m_MinimunButtonCount - validButtonCount; i ++)
			{
				GameObject placeHolderButton = GameObject.Instantiate(this.m_PlaceHolderPrefab) as GameObject;
				placeHolderButton.transform.parent = this.m_Grid.transform;
				placeHolderButton.transform.localPosition = Vector3.zero;
			}
			//this.m_Panel.dragEffect = UIDraggablePanel.DragEffect.Momentum;
		}
		
		this.m_Grid.repositionNow = true;
	}
}
