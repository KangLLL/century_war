using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class DirectionOffsetConfig 
{
	public Vector2 UpOffset;
	public Vector2 RightUpOffset;
	public Vector2 RightOffset;
	public Vector2 RightDownOffset;
	public Vector2 DownOffset;
	public Vector2 LeftDownOffset;
	public Vector2 LeftOffset;
	public Vector2 LeftUpOffset;
	
	private Dictionary<CharacterDirection, Vector2> m_Offsets;
	
	public Vector2 GetOffset(CharacterDirection direction)
	{
		if(this.m_Offsets == null)
		{
			this.m_Offsets = new Dictionary<CharacterDirection, Vector2>();
			this.m_Offsets.Add(CharacterDirection.Up, this.UpOffset);
			this.m_Offsets.Add(CharacterDirection.Down, this.DownOffset);
			this.m_Offsets.Add(CharacterDirection.Left, this.LeftOffset);
			this.m_Offsets.Add(CharacterDirection.Right, this.RightOffset);
			this.m_Offsets.Add(CharacterDirection.LeftDown, this.LeftDownOffset);
			this.m_Offsets.Add(CharacterDirection.LeftUp, this.LeftUpOffset);
			this.m_Offsets.Add(CharacterDirection.RightDown, this.RightDownOffset);
			this.m_Offsets.Add(CharacterDirection.RightUp, this.RightUpOffset);
			this.m_Offsets.Add(CharacterDirection.None, this.UpOffset);
		}
		return this.m_Offsets[direction];
	}
}
