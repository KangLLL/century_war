using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;

public class LogicTipsBehavior : MonoBehaviour 
{
	void Update () 
	{
		List<ArmyUpgradeNotification> armyNotifications = ArmyLevelObserver.Instance.PumpNotifications();
		List<TaskCompleteNotification> taskNotifications = TaskStatusObserver.Instance.PumpNotifications();

		foreach (ArmyUpgradeNotification army in armyNotifications) 
		{
			AudioController.Play("BuildingLevelUp");
			ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(army.ArmyType, army.NewLevel);
			UIErrorMessage.Instance.ErrorMessage(string.Format(ClientStringConstants.ARMY_UPGRADE_TIPS, armyConfigData.Name, army.NewLevel), Color.white);
		}
		foreach(TaskCompleteNotification task in taskNotifications)
		{
			AudioController.Play("CompleteTask");
			UIErrorMessage.Instance.ErrorMessage(string.Format(ClientStringConstants.TASK_COMPLETE_TIPS, task.Task.TaskConfigData.Name), Color.white);
		}
	}
}
