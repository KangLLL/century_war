using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputDispatcher : MonoBehaviour 
{
	[SerializeField]
	private CharacterFactory m_CharacterFactory;
	[SerializeField]
	private CameraManager m_CamareManager;
	[SerializeField]
	private int m_GenerateArmyFrameThreshold;
	[SerializeField]
	private int m_GenerateArmyFrameInterval;
	[SerializeField]
	private float m_GenerateArmyVectorThreshold;
	
	private int m_CurrentFrame;
	
	private Dictionary<int, TouchInformation> m_Touches;
	
	void Start()
	{
		this.m_Touches = new Dictionary<int, TouchInformation>();
	}
	
	void OnPress(bool isPressed)
	{
		this.m_CamareManager.OnPress(isPressed);
		
		if(isPressed)
		{
			TouchInformation information = new TouchInformation(){
				IsMoveOutSide = false, PressedFrameCount = this.m_CurrentFrame};
			this.m_Touches.Add(UICamera.currentTouchID, information);
		}
		else
		{
			int fingerID = UICamera.currentTouchID;
			TouchInformation information = this.m_Touches[fingerID];
			
			if(!information.IsMoveOutSide && 
				this.m_CurrentFrame - information.PressedFrameCount < this.m_GenerateArmyVectorThreshold)
			{
				this.m_CharacterFactory.Construct(UICamera.currentTouch.pos);
			}
			else if(information.IsMoveOutSide)
			{
				this.m_CamareManager.OnClick();
			}
			this.m_Touches.Remove(fingerID);
		}
	}
	
	void OnDrag(Vector2 deltaVector)
	{
		TouchInformation touchInfo = this.m_Touches[UICamera.currentTouchID];
		
		if(this.m_CurrentFrame - touchInfo.PressedFrameCount < this.m_GenerateArmyFrameThreshold)
		{
			if(UICamera.currentTouch.totalDelta.magnitude > this.m_GenerateArmyVectorThreshold)
			{
				touchInfo.IsMoveOutSide = true;
			}
		}
		
		if(touchInfo.IsMoveOutSide || this.m_CurrentFrame - touchInfo.PressedFrameCount < this.m_GenerateArmyFrameThreshold)
		{
			this.m_CamareManager.OnDrag(deltaVector);
		}
	}

	void FixedUpdate()
	{
		this.m_CurrentFrame ++;
		
		if(BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished)
		{
			foreach (KeyValuePair<int, TouchInformation> touch in this.m_Touches) 
			{
				if(!touch.Value.IsMoveOutSide)
				{
					if(this.m_CurrentFrame - touch.Value.PressedFrameCount >= this.m_GenerateArmyFrameThreshold &&
						(this.m_CurrentFrame - touch.Value.PressedFrameCount - this.m_GenerateArmyFrameThreshold) % this.m_GenerateArmyFrameInterval == 0)
					{
						if(touch.Key < 0)
						{
							this.m_CharacterFactory.Construct(Input.mousePosition);
						}
						else
						{
							this.m_CharacterFactory.Construct(Input.GetTouch(touch.Key).position);
						}
					}
				}
			}
		}
	}
}
