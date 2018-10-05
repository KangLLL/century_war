using UnityEngine;
using System.Collections;

public class CharacterHPBehavior : HPBehavior 
{
	protected override void OnDead ()
	{
		base.OnDead();
		this.ClearActor();
	}

	protected void ClearActor()
	{
		TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.transform.position);
		BattleSceneHelper.Instance.DestroyActor(gameObject, currentPosition);
		
		if(Application.loadedLevelName == ClientStringConstants.BATTLE_SCENE_LEVEL_NAME)
		{
			CharacterPropertyBehavior property = this.GetComponent<CharacterPropertyBehavior>();
			if(property.CharacterType == CharacterType.Invader)
			{
				if(BattleSceneHelper.Instance.TotalInvaderCount == 0 && 
				   BattleRecorder.Instance.DropArmyCount == ArmyMenuPopulator.Instance.TotalArmyCount &&
				   BattleRecorder.Instance.DropMercenaryCount == ArmyMenuPopulator.Instance.TotalMercenaryCount)
				{
					BattleDirector.Instance.EndMatch();
				}
			}
		}
	}
	
	public void SetDead()
	{
		if(this.m_CurrentHP > 0 || !this.m_IsInitialized)
		{
			GameObject.Destroy(gameObject);
			if(this.m_DeadEffectPrefab1 != null)
			{
				GameObject deadPrefab = GameObject.Instantiate(this.m_DeadEffectPrefab1) as GameObject;
				deadPrefab.transform.position = this.transform.position;
			}
		}
	}
}
