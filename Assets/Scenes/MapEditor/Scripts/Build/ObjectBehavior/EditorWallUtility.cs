using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorWallUtility : MonoBehaviour 
{
	private GameObject m_UpWall;
	private GameObject m_RightWall;
	
	private EditorBuildingBehavior m_BuildingBehavior;
	
	private const string UP_WALL_OBJECT_NAME = "BuildingBackgroundTop";
	private const string RIGHT_WALL_OBJECT_NAME = "BuildingBackgroundRight";
	
	void Start()
	{
		this.m_UpWall = this.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME).GetChild(0)
			.FindChild(UP_WALL_OBJECT_NAME).gameObject;
		this.m_RightWall = this.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME).GetChild(0)
			.FindChild(RIGHT_WALL_OBJECT_NAME).gameObject;
		
		this.m_UpWall.SetActive(false);
		this.m_RightWall.SetActive(false);
		this.m_BuildingBehavior = this.GetComponent<EditorBuildingBehavior>();
	}
	
	void Update()
	{
		TilePosition upPosition = new TilePosition(this.m_BuildingBehavior.Position.Column, this.m_BuildingBehavior.Position.Row + 1);
		TilePosition rightPosition = new TilePosition(this.m_BuildingBehavior.Position.Column + 1, this.m_BuildingBehavior.Position.Row);
		
		if(upPosition.IsValidBuildingTilePosition())
		{
			GameObject upObject = EditorFactory.Instance.MapData[upPosition.Row, upPosition.Column];
			if(upObject != null && upObject.GetComponent<EditorBuildingBehavior>() != null &&
				upObject.GetComponent<EditorBuildingBehavior>().BuildingType == BuildingType.Wall)
			{
				this.m_UpWall.SetActive(true);
			}
			else
			{
				this.m_UpWall.SetActive(false);
			}
		}
		else
		{
			this.m_UpWall.SetActive(false);
		}
		
		if(rightPosition.IsValidBuildingTilePosition())
		{
			GameObject rightObject = EditorFactory.Instance.MapData[rightPosition.Row, rightPosition.Column];
			if(rightObject != null && rightObject.GetComponent<EditorBuildingBehavior>() != null && 
				rightObject.GetComponent<EditorBuildingBehavior>().BuildingType == BuildingType.Wall)
			{
				this.m_RightWall.SetActive(true);
			}
			else
			{
				this.m_RightWall.SetActive(false);
			}
		}
		else
		{
			this.m_RightWall.SetActive(false);
		}
	}
}
