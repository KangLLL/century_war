using UnityEngine;
using System.Collections;

public class KodoAI : CharacterAI 
{
	public override void SetIdle (bool isResponseInstantly)
	{
		KodoIdleState idleState = new KodoIdleState(this, isResponseInstantly);
		this.ChangeState(idleState);
	}
}
