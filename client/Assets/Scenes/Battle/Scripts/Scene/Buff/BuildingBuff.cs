using UnityEngine;
using System.Collections;

public class BuildingBuff  
{
	public virtual int HPEffect { get { return 0; } }
	public virtual int AttackValueEffect { get { return 0; } }
	public virtual int AttackSpeedEffect { get { return 0; } }
}
