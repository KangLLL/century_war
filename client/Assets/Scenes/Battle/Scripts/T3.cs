using UnityEngine;
using System.Collections;

public class T3 : MonoBehaviour
{
	//private static Vector3 s_Destination = new Vector3(600, 600, 0);
	
	// Use this for initialization
	void Start () 
	{
		this.RePosition();
	}
	
	public void RePosition()
	{
		//float offsetX = 0;
		for(int i = 1; i < this.transform.childCount; i++)
		{
			Transform t = this.transform.GetChild(i);
			Transform pt = this.transform.GetChild(i-1);
			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(this.transform, pt); 
			t.localPosition = new Vector3(pt.localPosition.x + 2 * b.extents.x,t.localPosition.y,t.localPosition.z);
			//offsetX += 2 * b.extents.x;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		
		//Vector3 worldPosition = this.m_SceneCamera.ScreenToWorldPoint(Input.mousePosition);
		//Debug.Log("Current Tile is: row = " + PositionConvertor.GetActorTileIndexFromScreenPosition(Input.mousePosition).Row + 
		//	", column = " + PositionConvertor.GetActorTileIndexFromScreenPosition(Input.mousePosition).Column);
		
		/*
		if(Input.GetKeyDown(KeyCode.A))
		{
			IMapData mapData = SceneManager.Instance;
			BuildingIdentity buildingID = new BuildingIdentity(ConfigUtilities.Enums.BuildingType.CityHall, 0);
			BuildingLogicData logicData = LogicController.Instance.GetBuildingObject(buildingID);
			//GameObject go = mapData.GetBuildingObjectFromBuildingObstacleMap(logicData.BuildingPosition.Row, logicData.BuildingPosition.Column);
			//ActorDirector.Instance.SendBuilderToBuild(0, go);
		}
		
		//this.transform.localRotation = Quaternion.Euler(0,this.transform.localRotation.eulerAngles.y + this.m_Velocity,0);
		
		
		if(!this.transform.position.Equals(s_Destination))
		{
			Vector3 delta = s_Destination - this.transform.position;
			float percentage = this.m_Velocity / Vector2.Distance(s_Destination,this.transform.position);
			if(percentage >= 1)
			{
				this.transform.position = s_Destination;
			}
			else
			{
				this.transform.position += delta * percentage;
			}
		}
		*/
	}
}
