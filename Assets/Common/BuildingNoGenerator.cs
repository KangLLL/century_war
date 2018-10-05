using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuildingNoGenerator  
{
	private Dictionary<BuildingType, int> m_NoGenerator;
	
	public BuildingNoGenerator()
	{
		this.m_NoGenerator = new Dictionary<BuildingType, int>();
		for(int i = 0; i < (int)BuildingType.Length; i ++)
		{
			BuildingType type = (BuildingType)i;
			this.m_NoGenerator.Add(type, 0);
		}
	}
	
	public int GetBuildingNO(BuildingType type)
	{
		int result = this.m_NoGenerator[type];
		this.m_NoGenerator[type] ++;
		return result;
	}
}
