using UnityEngine;
using System.Collections;

public class EditorCellBehavior : MonoBehaviour 
{
	[SerializeField]
	private tk2dSlicedSprite m_Sprite;
	
	void Update () 
	{
		bool isValid = true;
		TilePosition currentPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(this.transform.position);
		
		if(!currentPosition.IsValidBuildingTilePosition())
		{
			isValid = false;
		}
		if(EditorFactory.Instance.MapData[currentPosition.Row, currentPosition.Column] != null)
		{
			isValid = false;
		}
		
		if(isValid)
		{
			this.m_Sprite.color = Color.green;
		}
		else
		{
			this.m_Sprite.color = Color.red;
		}
	}
}
