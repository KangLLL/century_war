using UnityEngine;
using System.Collections;
using System.IO;

public class BattleWriter : MonoBehaviour 
{
	private static BattleWriter s_Sigleton;
	
	[SerializeField]
	private string m_BattleName;
	
	public static BattleWriter Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	public void SaveBattle()
	{
		Hashtable battle = new Hashtable();
		battle.Add(EditorConfigInterface.Instance.BattleTimeKey, TimeTickRecorder.Instance.CurrentTimeTick);
		if(BattleRecorder.Instance.DropArmies.Count > 0)
		{
			ArrayList armyList = new ArrayList();
			foreach(var drop in BattleRecorder.Instance.DropArmies)
			{
				foreach (var command in drop.Value) 
				{
					Hashtable c = new Hashtable();
					c.Add(EditorConfigInterface.Instance.DropTypeKey, (int)command.ConstructCommand.Identity.armyType);
					c.Add(EditorConfigInterface.Instance.ArmyLevelKey, ((ConstructArmyCommand)command.ConstructCommand).Level);
					c.Add(EditorConfigInterface.Instance.DropPositionXKey, command.ConstructCommand.Position.x);
					c.Add(EditorConfigInterface.Instance.DropPositionYKey, command.ConstructCommand.Position.y);
					
					c.Add(EditorConfigInterface.Instance.DropTimeKey, command.DroppedFrame);
					armyList.Add(c);
				}
			}
			armyList.Sort(new SortByTime());
			battle.Add(EditorConfigInterface.Instance.BattleArmyKey, armyList);
		}
		if(BattleRecorder.Instance.DropMercenaries.Count > 0)
		{
			ArrayList mercenaryList = new ArrayList();
			foreach(var drop in BattleRecorder.Instance.DropMercenaries)
			{
				foreach (var command in drop.Value) 
				{
					Hashtable c = new Hashtable();
					c.Add(EditorConfigInterface.Instance.DropTypeKey, (int)command.ConstructCommand.Identity.mercenaryType);
					c.Add(EditorConfigInterface.Instance.DropPositionXKey, command.ConstructCommand.Position.x);
					c.Add(EditorConfigInterface.Instance.DropPositionYKey, command.ConstructCommand.Position.y);
					c.Add(EditorConfigInterface.Instance.DropTimeKey, command.DroppedFrame);
					mercenaryList.Add(c);
				}
			}
			mercenaryList.Sort(new SortByTime());
			battle.Add(EditorConfigInterface.Instance.BattleMercenaryKey, mercenaryList);
		}
		
		if(BattleRecorder.Instance.UsePropsCommands.Count > 0)
		{
			ArrayList propsList = new ArrayList();
			foreach (var props in BattleRecorder.Instance.UsePropsCommands) 
			{
				foreach (var command in props.Value)
				{
					Hashtable c = new Hashtable();
					c.Add(EditorConfigInterface.Instance.DropTimeKey, (int)props.Key);
					c.Add(EditorConfigInterface.Instance.DropPositionXKey, command.ConstructCommand.Position.x);
					c.Add(EditorConfigInterface.Instance.DropPositionYKey, command.ConstructCommand.Position.y);
					propsList.Add(c);
				}
			}
			propsList.Sort(new SortByTime());
			battle.Add(EditorConfigInterface.Instance.BattlePropsKey, propsList);
			
		}
		
		FileStream fs =  Application.platform == RuntimePlatform.OSXEditor ? 
			new FileStream(this.m_BattleName + "."  + EditorConfigInterface.Instance.MapSuffix,FileMode.Create) :
			new FileStream(EditorConfigInterface.Instance.MapStorePath + "/" +
			this.m_BattleName + "."  + EditorConfigInterface.Instance.MapSuffix, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		
		sw.Write(JSONHelper.jsonEncode(battle));
		sw.Close();
		
		Debug.Log("write finish!");
	}
	
	private class SortByTime : IComparer
	{
		#region IComparer implementation
		public int Compare (object x, object y)
		{
			Hashtable a = (Hashtable)x;
			Hashtable b = (Hashtable)y;
			
			int aTime = (int)a[EditorConfigInterface.Instance.DropTimeKey];
			int bTime = (int)b[EditorConfigInterface.Instance.DropTimeKey];
			if(aTime > bTime)
			{
				return 1;
			}
			else if(aTime < bTime)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
		#endregion
	}
}
