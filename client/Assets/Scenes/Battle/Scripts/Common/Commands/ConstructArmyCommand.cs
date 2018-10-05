using UnityEngine;
using System.Collections;

public class ConstructArmyCommand : UserCommand<ArmyIdentity> 
{
	public int Level { get;set; }
}
