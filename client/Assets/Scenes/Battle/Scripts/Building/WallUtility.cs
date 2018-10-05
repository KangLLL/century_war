using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class WallUtility : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_UpWall;
	[SerializeField]
	private GameObject m_RightWall;
	
	void Start()
	{
		BuildingPropertyBehavior property = this.GetComponent<BuildingPropertyBehavior>();
		
		this.m_UpWall.SetActive(false);
		this.m_RightWall.SetActive(false);
		
		TilePosition upPosition = new TilePosition(property.BuildingPosition.Column, property.BuildingPosition.Row + 1);
		TilePosition rightPosition = new TilePosition(property.BuildingPosition.Column + 1, property.BuildingPosition.Row);
		
		if(upPosition.IsValidBuildingTilePosition())
		{
			GameObject upObject = BattleMapData.Instance.GetBuildingObjectFromBuildingObstacleMap
				(upPosition.Row, upPosition.Column);
			if(upObject != null && upObject.GetComponent<BuildingPropertyBehavior>() != null &&
				upObject.GetComponent<BuildingPropertyBehavior>().BuildingType == BuildingType.Wall)
			{
				this.m_UpWall.SetActive(true);
				BattleMapData.Instance.InflateUpActorObstacleOfWall(property);
			}
		}
		if(rightPosition.IsValidBuildingTilePosition())
		{
			GameObject rightObject = BattleMapData.Instance.GetBuildingObjectFromBuildingObstacleMap
				(rightPosition.Row, rightPosition.Column);
			if(rightObject != null && rightObject.GetComponent<BuildingPropertyBehavior>() != null && 
				rightObject.GetComponent<BuildingPropertyBehavior>().BuildingType == BuildingType.Wall)
			{
				this.m_RightWall.SetActive(true);
				BattleMapData.Instance.InflateRightObstacleOfWall(property);
			}
		}
	}
	
	public void HideUpObject()
	{
		this.m_UpWall.gameObject.SetActive(false);
	}
	
	public void HideRightObject()
	{
		this.m_RightWall.gameObject.SetActive(false);
	}
}
