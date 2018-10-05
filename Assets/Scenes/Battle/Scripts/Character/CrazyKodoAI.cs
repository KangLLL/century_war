using UnityEngine;
using System.Collections;

public class CrazyKodoAI : CharacterAI 
{
	public override void SetIdle (bool isResponseInstantly)
	{
		CrazyKodoIdleState idleState = new CrazyKodoIdleState(this, isResponseInstantly);
		this.ChangeState(idleState);
	}
}
