using UnityEngine;
using System.Collections;

public class ArcherController : MonoBehaviour 
{
	/*
	[SerializeField]
	private int m_Velocity;
	[SerializeField]
	private tk2dAnimatedSprite m_Sprite;
	
	private int m_AttackUpId;
	private int m_AttackDownId;
	private int m_AttackLeftId;
	private int m_AttackRightId;
	
	private CharacterDirection m_Direction;
	private CharacterState m_State;
	
	private enum CharacterDirection
	{
		Up,
		Down,
		Left,
		Right
	}
	
	private enum CharacterState
	{
		Idle,
		Walk,
		Attack
	}
	
	// Use this for initialization
	void Start () 
	{
		this.m_Sprite.animationCompleteDelegate += Completed;
		this.m_AttackUpId = this.m_Sprite.GetClipIdByName(AnimationNameConstants.ATTACK_UP);
		this.m_AttackDownId = this.m_Sprite.GetClipIdByName(AnimationNameConstants.ATTACK_DOWN);
		this.m_AttackLeftId = this.m_Sprite.GetClipIdByName(AnimationNameConstants.ATTACK_LEFT);
		this.m_AttackRightId = this.m_Sprite.GetClipIdByName(AnimationNameConstants.ATTACK_RIGHT);
		
		this.SetDirection(CharacterDirection.Up);
		this.m_Sprite.Pause();
		this.m_State = CharacterState.Idle;
	}
	
	private void SetDirection(CharacterDirection direction)
	{
		this.m_Direction = direction;
		//this.m_State = CharacterState.Walk;
		
		switch (direction) 
		{
			case CharacterDirection.Up:
				this.m_Sprite.Play(AnimationNameConstants.MOVE_UP);
				break;
			case CharacterDirection.Down:
				this.m_Sprite.Play(AnimationNameConstants.MOVE_DOWN);
				break;
			case CharacterDirection.Left:
				this.m_Sprite.Play(AnimationNameConstants.MOVE_LEFT);
				break;
			case CharacterDirection.Right:
				this.m_Sprite.Play(AnimationNameConstants.MOVE_RIGHT);
				break;
		}
		this.m_Sprite.Resume();
	}
	
	
	private void Completed(tk2dAnimatedSprite sprite, int clipId)
	{
		if(clipId == this.m_AttackUpId || clipId == this.m_AttackRightId || clipId == this.m_AttackLeftId || clipId == this.m_AttackDownId)
		{
			this.SetDirection(this.m_Direction);
			this.m_Sprite.Pause();
			this.m_State = CharacterState.Idle;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		CharacterState destState = this.m_State;
		
		
		if(this.m_State == CharacterState.Idle)
		{
			/*
			print(Time.frameCount);
			print(Input.GetKeyDown(KeyCode.UpArrow));
			print(Input.GetKeyUp(KeyCode.UpArrow));
			print("*************************");
			
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.SetDirection(CharacterDirection.Down);
				destState = CharacterState.Walk;
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.SetDirection(CharacterDirection.Up);
				destState = CharacterState.Walk;
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.SetDirection(CharacterDirection.Left);
				destState = CharacterState.Walk;
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.SetDirection(CharacterDirection.Right);
				destState = CharacterState.Walk;
			}
			
			else if(Input.GetKeyDown(KeyCode.A))
			{
				if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_LEFT))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_LEFT);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_RIGHT))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_RIGHT);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_UP))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_UP);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_DOWN))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_DOWN);
				}
				this.m_Sprite.Resume();
			
				this.m_State = CharacterState.Attack;
			}
		}
		else if(this.m_State == CharacterState.Walk)
		{
			print(Time.frameCount);
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.SetDirection(CharacterDirection.Down);
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.SetDirection(CharacterDirection.Up);
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.SetDirection(CharacterDirection.Left);
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.SetDirection(CharacterDirection.Right);
			}
			
			else if(Input.GetKeyDown(KeyCode.A))
			{
				if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_LEFT))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_LEFT);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_RIGHT))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_RIGHT);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_UP))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_UP);
				}
				else if(this.m_Sprite.CurrentClip.name.Equals(AnimationNameConstants.MOVE_DOWN))
				{
					this.m_Sprite.Play(AnimationNameConstants.ATTACK_DOWN);
				}
				this.m_Sprite.Resume();
				
				this.m_State = CharacterState.Attack;
			}
			
			else if(Input.GetKeyUp(KeyCode.DownArrow) && this.m_Direction == CharacterDirection.Down)
			{
				this.m_Sprite.Pause();
				destState = CharacterState.Idle;
			}
			else if(Input.GetKeyUp(KeyCode.UpArrow) && this.m_Direction == CharacterDirection.Up)
			{
				this.m_Sprite.Pause();
				destState = CharacterState.Idle;
			}
			else if(Input.GetKeyUp(KeyCode.LeftArrow) && this.m_Direction == CharacterDirection.Left)
			{
				this.m_Sprite.Pause();
				destState = CharacterState.Idle;
			}
			else if(Input.GetKeyUp(KeyCode.RightArrow) && this.m_Direction == CharacterDirection.Right)
			{
				this.m_Sprite.Pause();
				destState = CharacterState.Idle;
			}
			
			Debug.Log(Input.GetKeyUp(KeyCode.UpArrow));
			
			Vector3 offset = new Vector3();
			switch (this.m_Direction) 
			{
				case CharacterDirection.Up:
					offset = new Vector3(0, this.m_Velocity, 0);
					break;
				case CharacterDirection.Down:
					offset = new Vector3(0, -this.m_Velocity, 0);
					break;
				case CharacterDirection.Left:
					offset = new Vector3(-this.m_Velocity, 0, 0);
					break;
				case CharacterDirection.Right:
					offset = new Vector3(this.m_Velocity, 0, 0);
					break;
			}
			this.transform.position = this.transform.position + offset;
		}
		this.m_State = destState;
	}
	
	*/
}
