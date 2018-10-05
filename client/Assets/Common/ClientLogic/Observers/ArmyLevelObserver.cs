using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ArmyLevelObserver : LogicObserver<ArmyUpgradeNotification> 
{
	private Dictionary<ArmyType, int> m_PreviousLevels;

	private static ArmyLevelObserver s_Sigleton;

	public static ArmyLevelObserver Instance
	{
		get { return s_Sigleton; }
	}

	void Awake()
	{
		s_Sigleton = this;
	}

	void OnDestroy()
	{
		s_Sigleton = null;
	}

	public override void Start () 
	{
		this.m_PreviousLevels = new Dictionary<ArmyType, int>();
		base.Start();
	}

	void Update()
	{
		if(LogicController.Instance.PlayerData != null)
		{
			foreach (ArmyType type in Enum.GetValues(typeof(ArmyType))) 
			{
				if(type != ArmyType.Length)
				{
					int level = LogicController.Instance.PlayerData.GetArmyLevel(type);
					if(this.m_PreviousLevels[type] != level)
					{
						this.m_PreviousLevels[type] = level;
						this.m_NotificationQueue.Enqueue(new ArmyUpgradeNotification() { ArmyType = type, NewLevel = level });
					}
				}
			}
		}
	}

	public override void StartObserve()
	{
		base.StartObserve();
		this.m_PreviousLevels.Clear();

		foreach (ArmyType type in Enum.GetValues(typeof(ArmyType))) 
		{
			if(type != ArmyType.Length)
			{
				this.m_PreviousLevels.Add(type, LogicController.Instance.PlayerData.GetArmyLevel(type));
			}
		}
	}
}
