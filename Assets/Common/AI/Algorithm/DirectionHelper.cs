using UnityEngine;
using System.Collections;

public class DirectionHelper  
{
	private const float SCOPE_STEEP = Mathf.PI / 4;
	private const float MINIMUN_SCOPE = - Mathf.PI / 2;
	private const float SCOPE_1 = - Mathf.PI / 8;
	private const float SCOPE_2 = SCOPE_1 + SCOPE_STEEP;
	private const float SCOPE_3 = SCOPE_2 + SCOPE_STEEP;
	private const float SCOPE_4 = SCOPE_3 + SCOPE_STEEP;
	private const float SCOPE_5 = SCOPE_4 + SCOPE_STEEP;
	private const float SCOPE_6 = SCOPE_5 + SCOPE_STEEP;
	private const float SCOPE_7 = SCOPE_6 + SCOPE_STEEP;
	private const float SCOPE_8 = SCOPE_7 + SCOPE_STEEP;
	
	public static CharacterDirection GetDirectionFormVector(Vector2 deltaVector)
	{
		float k;
		if(deltaVector.x.IsZero())
		{
			k = deltaVector.y > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
		}
		else if(deltaVector.y.IsZero())
		{
			k = deltaVector.x > 0 ? 0 : Mathf.PI;
		}
		else
		{
			k = Mathf.Atan(deltaVector.y / deltaVector.x);
			if((deltaVector.y.IsNegative() && deltaVector.x.IsNegative()) || 
				(deltaVector.y.IsPositive() && deltaVector.x.IsNegative()))
			{
				k += Mathf.PI;
			}
		}
		
		CharacterDirection result = CharacterDirection.None;
		if(k >= MINIMUN_SCOPE  && k < MINIMUN_SCOPE - SCOPE_1)
		{
			result = CharacterDirection.Down;
		}
		else if(k >= SCOPE_1 && k < SCOPE_2)
		{
			result = CharacterDirection.Right;
		}
		else if(k >= SCOPE_2 && k < SCOPE_3)
		{
			result = CharacterDirection.RightUp;
		}
		else if(k >= SCOPE_3 && k < SCOPE_4)
		{
			result = CharacterDirection.Up;
		}
		else if(k >= SCOPE_4 && k < SCOPE_5)
		{
			result = CharacterDirection.LeftUp;
		}
		else if(k >= SCOPE_5 && k < SCOPE_6)
		{
			result = CharacterDirection.Left;
		}
		else if(k >= SCOPE_6 && k < SCOPE_7)
		{
			result = CharacterDirection.LeftDown;
		}
		else if(k >= SCOPE_7 && k < SCOPE_8)
		{
			result = CharacterDirection.Down;
		}
		else
		{
			result = CharacterDirection.RightDown;
		}
		
		return result;
	}
}
