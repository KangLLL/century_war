using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class SummaryDropArmy : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_ArmyInformationPrefab;
	[SerializeField]
	private PrefabDictionary m_ArmyPrefabs;
	[SerializeField]
	private PrefabDictionary m_MercenaryPrefabs;
	[SerializeField]
	private PrefabDictionary m_PropsPrefabs;
	
	[SerializeField]
	private UIGrid m_Grid;
	[SerializeField]
	private UIDraggablePanel m_Panel;
	
	//private Dictionary<ArmyType, GameObject> m_ArmyPrefabsDict;
	//private Dictionary<MercenaryType, GameObject> m_MercenaryPrefabsDict;
	//private Dictionary<PropsType, GameObject> m_PropsPrefabsDict;
	
	void Start () 
	{
		Dictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>> armies = BattleRecorder.Instance.DropArmies;
		Dictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>> mercenaries = BattleRecorder.Instance.DropMercenaries;
		List<KeyValuePair<PropsType, int>> props = BattleRecorder.Instance.UseProps;
		
		/*
		this.m_ArmyPrefabsDict = new Dictionary<ArmyType, GameObject>(){
			{ArmyType.Berserker, this.m_BerserkerPrefab},
			{ArmyType.Ranger, this.m_RangerPrefab},
			{ArmyType.Marauder, this.m_MarauderPrefab}
		};
		this.m_MercenaryPrefabsDict = new Dictionary<MercenaryType, GameObject>()
		{
			{MercenaryType.Slinger, this.m_MercenarySlingerPrefab},
			{MercenaryType.Hercules, this.m_MercenaryHerculesPrefab},
			{MercenaryType.Kodo, this.m_MercenaryKodoPrefab},
			{MercenaryType.HerculesII, this.m_MercenaryHerculesIIPrefab},
			{MercenaryType.KodoII, this.m_MercenaryKodoIIPrefab},
			{MercenaryType.Arsonist, this.m_MercenaryArsonistPrefab},
			{MercenaryType.ArsonistII, this.m_MercenaryArsonistIIPrefab},
			{MercenaryType.PhalanxSoldier, this.m_MercenaryPhalanxSoldierPrefab},
			{MercenaryType.Catapults, this.m_MercenaryCatapultsPrefab},
			{MercenaryType.CrazyKodo, this.m_MercenaryCrazyKodoPrefab}
		};
		this.m_PropsPrefabsDict = new Dictionary<PropsType, GameObject>()
		{
			{PropsType.DamageBottle, this.m_PropsDamageBottlePrefab},
			{PropsType.ArmyBerserkerExcellent, this.m_PropsArmyBerserkerExcellentPrefab},
			{PropsType.ArmyBerserkerSophisticated, this.m_PropsArmyBerserkerSophisticatedPrefab},
			{PropsType.ArmyBerserkerEpic, this.m_PropsArmyBerserkerEpicPrefab},
			{PropsType.ArmyBerserkerLegend, this.m_PropsArmyBerserkerLegendPrefab},
			{PropsType.ArmyRangerExcellent, this.m_PropsArmyRangerExcellentPrefab},
			{PropsType.ArmyRangerSophisticated, this.m_PropsArmyRangerSophisticatedPrefab},
			{PropsType.MercenaryArsonistExcellent, this.m_PropsMercenaryArsonistExcellentPrefab},
			{PropsType.MercenaryCrazyKodoExcellent, this.m_PropsMercenaryCrazyKodoExcellentPrefab},
			{PropsType.MercenaryCrazyKodoSophisticated, this.m_PropsMercenaryCrazyKodoSophisticatedPrefab}
		};
		*/
		foreach(ArmyType type in armies.Keys)
		{
			GameObject armyInformation = GameObject.Instantiate(this.m_ArmyPrefabs[type.ToString()]) as GameObject;
			armyInformation.name = type.GetUIGridSortString() + "_" + armyInformation.name;
			armyInformation.transform.parent = this.m_Grid.transform;
			DropArmyInformation info = armyInformation.GetComponent<DropArmyInformation>();
			info.Level = ((ConstructArmyCommand)armies[type][0].ConstructCommand).Level;
			info.Quantity = armies[type].Count;
			UIDragPanelContents panelContent = armyInformation.GetComponentInChildren<UIDragPanelContents>();
			panelContent.draggablePanel = this.m_Panel;
		}
		foreach(MercenaryType type in mercenaries.Keys)
		{
			GameObject mercenaryInformation = GameObject.Instantiate(this.m_MercenaryPrefabs[type.ToString()]) as GameObject;
			mercenaryInformation.name = type.GetUIGridSortString() + "_" + mercenaryInformation.name;
			mercenaryInformation.transform.parent = this.m_Grid.transform;
			DropMercenaryInformation info = mercenaryInformation.GetComponent<DropMercenaryInformation>();
			info.Quantity = mercenaries[type].Count;
			UIDragPanelContents panelContent = mercenaryInformation.GetComponentInChildren<UIDragPanelContents>();
			panelContent.draggablePanel = this.m_Panel;
		}
		foreach (KeyValuePair<PropsType,int>  p in props) 
		{
			GameObject propsInformation = GameObject.Instantiate(this.m_PropsPrefabs[p.Key.ToString()]) as GameObject;
			propsInformation.name = p.Key.GetUIGridSortString() + "_" + propsInformation.name;
			propsInformation.transform.parent = this.m_Grid.transform;
			DropPropsInformation info = propsInformation.GetComponent<DropPropsInformation>();
			info.Quantity = p.Value;
			UIDragPanelContents panelContent = propsInformation.GetComponentInChildren<UIDragPanelContents>();
			panelContent.draggablePanel = this.m_Panel;
		}
		this.m_Grid.repositionNow = true;
	}
}
