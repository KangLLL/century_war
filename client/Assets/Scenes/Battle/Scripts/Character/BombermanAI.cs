using UnityEngine;
using System.Collections;

public class BombermanAI : CharacterAI 
{
	public override void SetIdle (bool isResponseInstantly)
	{
		BombermanIdleState idleState = new BombermanIdleState(this, isResponseInstantly);
		this.ChangeState(idleState);
	}
}
