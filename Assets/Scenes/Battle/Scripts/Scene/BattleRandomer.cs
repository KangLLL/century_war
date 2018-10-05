using UnityEngine;
using System.Collections;

public class BattleRandomer : MonoBehaviour 
{
	private static BattleRandomer s_Sigleton;
	private System.Random m_Random;
	
	public static BattleRandomer Instance
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
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void SetSeed(int seed)
	{
		this.m_Random = new System.Random(seed);
	}
	
	public int GetRandomNumber(int min, int max)
	{
		return this.m_Random.Next(min, max);
	}
	
	public double GetRondomDouble()
	{
		return this.m_Random.NextDouble();
	}
	
	public float GetRondomValue(float min, float max)
	{
		float delta = max - min;
		float result = min + (float)(this.m_Random.NextDouble()) * delta;
		return result;
	}
}
