using UnityEngine;
using System.Collections;

public class AchievementBuildingHPBehavior : BuildingBaseHPBehavior 
{
	private AchievementBuildingPropertyBehavior m_Property;
	
	public override void Start ()
	{
		base.Start ();
		this.m_Property = (AchievementBuildingPropertyBehavior)this.m_BaseProperty;
	}
	
	protected override void OnDead ()
	{
		BattleRecorder.Instance.RecordDestroyAchievementBuilding(this.m_Property.BuildingNO, this.m_Property.AchievementBuildingType, this.m_Property.IsDropProps);
		this.SceneHelper.DestroyAchievementBuilding(gameObject);
		base.OnDead ();
	}
}
