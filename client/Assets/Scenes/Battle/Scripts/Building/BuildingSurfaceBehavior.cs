using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class BuildingSurfaceBehavior : MonoBehaviour 
{
	[SerializeField]
	private Transform m_SurfaceParent;
	private GameObject m_Surface;
	
	public void SetSurface(Age newAge, BuildingType buildingType)
	{
		if(this.m_Surface != null)
		{
			GameObject.Destroy(this.m_Surface);
		}
		string prefabName = ClientConfigConstants.Instance.GetSurfacePrefabName(buildingType, newAge);
		
		GameObject surfacePrefab = Resources.Load(prefabName,typeof(GameObject)) as GameObject;
		GameObject surface = GameObject.Instantiate(surfacePrefab) as GameObject;
        Vector3 localPosition = surface.transform.position;
		surface.gameObject.transform.parent = this.m_SurfaceParent;
        surface.gameObject.transform.localPosition = localPosition;
		this.m_Surface = surface;
	}

    public void SetSurface(Age newAge, AchievementBuildingType buildingType)
    {
        if (this.m_Surface != null)
        {
            GameObject.Destroy(this.m_Surface);
        }
        string prefabName = ClientConfigConstants.Instance.GetSurfacePrefabName(buildingType, newAge);

        GameObject surfacePrefab = Resources.Load(prefabName, typeof(GameObject)) as GameObject;
        GameObject surface = GameObject.Instantiate(surfacePrefab) as GameObject;
        Vector3 localPosition = surface.transform.position;
        surface.gameObject.transform.parent = this.m_SurfaceParent;
        surface.gameObject.transform.localPosition = localPosition;
        this.m_Surface = surface;
    }
}
