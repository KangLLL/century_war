using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class EditorButtonBehavior : MonoBehaviour 
{
	[SerializeField]
	private BuildingType m_Type;
	
	private BuildingConfigData m_ConfigData;
	
	// Use this for initialization
	void Start () 
	{
		this.m_ConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.m_Type, 0);
	}
	
	void OnClick()
	{
		EditorFactory.Instance.ConstructBuilding(this.m_Type, this.m_ConfigData.InitialLevel);
	}
}
