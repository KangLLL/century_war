using UnityEngine;
using System.Collections;

public class EditorObjectBehavior : MonoBehaviour 
{
	protected IOperateFunction m_Function;
	
	public TilePosition Position { get; set; }
	
	void OnPress(bool isPressed)
	{
		if(UICamera.currentTouchID == -1)
		{
			if(isPressed)
			{
				this.m_Function.Move();
			}
			else
			{
				this.m_Function.DropDown();
			}
		}
	}
	
	void OnDrag(Vector2 delta)
	{
		if(UICamera.currentTouchID == -1)
		{
			this.m_Function.Drag();
		}
	}
	
	void OnClick()
	{
		if(UICamera.currentTouchID == -2)
		{
			this.m_Function.ChangeType();
		}
		else if(UICamera.currentTouchID == -3)
		{
			GameObject.Destroy(this.gameObject);
			this.m_Function.Delete();
		}
	}
}
