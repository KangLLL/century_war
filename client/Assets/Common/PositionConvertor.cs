using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Structs;

public class PositionConvertor  
{
	public static TilePosition GetBuildingTileIndexFromWorldPosition(Vector3 worldPosition)
	{
		TilePosition result = new TilePosition();
		result.Column = (int)((worldPosition.x - ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x + 0.01)
			/ ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width);
		if(worldPosition.x < 0)
		{
			result.Column --;
		}
		result.Row = (int)((worldPosition.y - ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y + 0.01)
			/ ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height);
		if(worldPosition.y < 0)
		{
			result.Row --;
		}
		return result;
	}
	
	public static TilePosition GetActorTileIndexFromWorldPosition(Vector3 worldPosition)
	{
		TilePosition result = new TilePosition();
		result.Column = (int)((worldPosition.x - ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x + 0.01)
			/ ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width);
		if(worldPosition.x < 0)
		{
			result.Column --;
		}
		result.Row = (int)((worldPosition.y - ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y + 0.01)
			/ ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height);
		if(worldPosition.y < 0)
		{
			result.Row --;
		}
		return result;
	}

	public static Vector3 ClampWorldPositionOfBuildingTile(Vector3 worldPosition)
	{
		Vector3 result = Vector3.zero;
		result.x = Mathf.Clamp(worldPosition.x, ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x, 
		                       ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x +
		                       ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width * ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width - 0.011f);
		result.y = Mathf.Clamp(worldPosition.y, ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y, 
		                       ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y +
		                       ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height * ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height - 0.011f);
		return result;
	}

	public static Vector3 ClampWorldPositionOfActorTile(Vector3 worldPosition)
	{
		Vector3 result = Vector3.zero;
		result.x = Mathf.Clamp(worldPosition.x, ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x, 
		                       ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x +
		                       ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width * ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width - 0.011f);
		result.y = Mathf.Clamp(worldPosition.y, ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y, 
		                       ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y +
		                       ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height * ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height - 0.011f);
		return result;
	}

	public static Vector2 ClampWorldPositionOfBuildingTile(Vector2 worldPosition)
	{
		Vector2 result = Vector3.zero;
		result.x = Mathf.Clamp(worldPosition.x, ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x, 
		                       ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x +
		                       ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width * ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width - 0.011f);
		result.y = Mathf.Clamp(worldPosition.y, ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y, 
		                       ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y +
		                       ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height * ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height - 0.011f);
		return result;
	}
	
	public static Vector2 ClampWorldPositionOfActorTile(Vector2 worldPosition)
	{
		Vector2 result = Vector2.zero;
		result.x = Mathf.Clamp(worldPosition.x, ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x, 
		                       ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x +
		                       ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width * ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width - 0.011f);
		result.y = Mathf.Clamp(worldPosition.y, ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y, 
		                       ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y +
		                       ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height * ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height - 0.011f);
		return result;
	}
	
	public static TilePosition GetBuildingTileIndexFromScreenPosition(Vector2 screenPosition)
	{
		Vector3 position = new Vector3(screenPosition.x, screenPosition.y, 0);
		return GetBuildingTileIndexFromScreenPosition(position);
	}
	
	public static TilePosition GetBuildingTileIndexFromScreenPosition(Vector3 screenPosition)
	{
		Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenPosition);
		return GetBuildingTileIndexFromWorldPosition(worldPosition);
	}   
	
	public static TilePosition GetActorTileIndexFromScreenPosition(Vector2 screenPosition)
	{
		Vector3 position = new Vector3(screenPosition.x, screenPosition.y, 0);
		return GetActorTileIndexFromScreenPosition(position);
	}
	
	public static TilePosition GetActorTileIndexFromScreenPosition(Vector3 screenPosition)
	{
		Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenPosition);
		return GetActorTileIndexFromWorldPosition(worldPosition);
	}
	
	
	public static Vector3 GetWorldPositionFromBuildingTileIndex(TilePosition tileIndex)
	{
		float x = ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x +
			tileIndex.Column * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width;
		float y = ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y +
			tileIndex.Row * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height;
		return new Vector3(x, y, 0);
	}
	
	public static Vector3 GetWorldPositionFromActorTileIndex(TilePosition tileIndex)
	{
		float x = ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x +
			tileIndex.Column * ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width;
		float y = ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y +
			tileIndex.Row * ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height;
		return new Vector3(x, y, 0);
	}
	
	public static Vector3 GetWorldPositionByBuildingTileIndex(TilePosition tileIndex)
    {
        float x = ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.x + 
			tileIndex.Column * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width;
        float y = ClientSystemConstants.BUILDING_TILE_MAP_OFFSET.y +
			tileIndex.Row * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height;
        float z = y;
        return new Vector3(x, y, z);
    }
	
	public static Vector3 GetWorldPositionByActorTileIndex(TilePosition tileIndex)
    {
        float x = ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.x + 
			tileIndex.Column * ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width;
        float y = ClientSystemConstants.ACTOR_TILE_MAP_OFFSET.y +
			tileIndex.Row * ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height;
        float z = y;
        return new Vector3(x, y, z);
    }
	
	public static TilePosition GetBuildingTilePositionFromActorTilePosition(TilePosition tileIndex)
	{
		return GetBuildingTileIndexFromWorldPosition(GetWorldPositionFromActorTileIndex(tileIndex));
	}
	
	public static TilePosition GetActorTilePositionFromBuildingTilePosition(TilePosition tileIndex)
	{
		return GetActorTileIndexFromWorldPosition(GetWorldPositionFromBuildingTileIndex(tileIndex));
	}

    public static List<TilePosition> TilePointListToTilePositionList(List<TilePoint> tilePointList)
    {
        List<TilePosition> tilePositionList = new List<TilePosition>();
        for (int i = 0, count = tilePointList.Count; i < count; i++)
        {
            TilePosition tlePosition = new TilePosition();
            tlePosition.Column = tilePointList[i].column;
            tlePosition.Row = tilePointList[i].row;
            tilePositionList.Add(tlePosition);
        }
        return tilePositionList;
    }
	/*
	public static TilePosition GetTileIndexFromWorldPosition(Vector3 worldPosition)
	{
		TilePosition result = new TilePosition();
		result.Column = Mathf.RoundToInt(worldPosition.x / ClientSystemConstants.TILE_PIXEL_SIZE);// TILE_PIXEL_SIZE;
		result.Row = Mathf.RoundToInt(worldPosition.y / ClientSystemConstants.TILE_PIXEL_SIZE); // TILE_PIXEL_SIZE;
		return result;
	}
	
	public static TilePosition GetTileIndexFromScreenPosition(Vector3 screenPosition)
	{
		Vector3 worldPosition = CameraManager.Instance.MainCamera.ScreenToWorldPoint(screenPosition);
		return GetTileIndexFromWorldPosition(worldPosition);
	}
	
	public static TilePosition GetTileIndexFromScreenPosition(Vector2 screenPosition)
	{
		Vector3 position = new Vector3(screenPosition.x, screenPosition.y, 0);
		return GetTileIndexFromScreenPosition(position);
	}
	
	public static Vector3 GetWorldPositionFromTileIndex(TilePosition tileIndex)
	{
		float x = tileIndex.Column * ClientSystemConstants.TILE_PIXEL_SIZE;// + TILE_PIXEL_SIZE / 2;
		float y = tileIndex.Row * ClientSystemConstants.TILE_PIXEL_SIZE;// + TILE_PIXEL_SIZE / 2;
		return new Vector3(x, y, 0);
		
	}
	
	public static Vector3 GetScreenPositionFromTileIndex(TilePosition tileIndex)
	{
		Vector3 worldPosition = GetWorldPositionFromTileIndex(tileIndex);
		return CameraManager.Instance.MainCamera.WorldToScreenPoint(worldPosition);
	}
    
	
	public static Vector3 GetWorldPositionByTileIndex(TilePosition tileIndex)
    {
        float x = tileIndex.Column * ClientSystemConstants.TILE_PIXEL_SIZE;
        float y = tileIndex.Row * ClientSystemConstants.TILE_PIXEL_SIZE;
        float z = y;
        return new Vector3(x, y, z);
    }
	*/
	
	
    /*
    public static Vector3 GetInputPosition(TouchPhase touchPhase = TouchPhase.Began)
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return Input.mousePosition;
        }
        else
        {
            if (SceneInput.Instance.TouchCount > 0)
            {
                switch (touchPhase)
                {
                    case TouchPhase.Began:
                        return SceneInput.Instance.Touches[0].position;
                    case TouchPhase.Moved:
                        return SceneInput.Instance.Touches[0].position;
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
    public static Vector3 GetWorldPositionFromInput(TouchPhase touchPhase = TouchPhase.Began)
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (SceneInput.Instance.TouchCount > 0)
            {
                switch (touchPhase)
                {
                    case TouchPhase.Began:
                        return CameraManager.Instance.MainCamera.ScreenToWorldPoint(SceneInput.Instance.Touches[0].position);
                    case TouchPhase.Moved:
                        return CameraManager.Instance.MainCamera.ScreenToWorldPoint(SceneInput.Instance.Touches[0].position);
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
    public static Vector3 GetDeltaPostion(Vector3 lastMousePostion,TouchPhase touchPhase = TouchPhase.Began)
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {  
            return Input.mousePosition - lastMousePostion;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                switch (touchPhase)
                {
                    case TouchPhase.Began:
                        return Input.GetTouch(0).deltaPosition;
                    case TouchPhase.Moved:
                        return Input.GetTouch(0).deltaPosition;
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
 */
}