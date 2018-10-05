using UnityEngine;
using System.Collections;

public class BattleUIManager : MonoBehaviour 
{
	private static BattleUIManager s_Sigleton;
	
	public static BattleUIManager Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
}
