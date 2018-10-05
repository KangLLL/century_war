using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleObjectAnchor : MonoBehaviour 
{
	public enum AnchorPoint
	{
		MinMost,
		Center,
		MaxMost
	}
	
	[SerializeField]
	private UIGrid.Arrangement m_Arrangement;
	[SerializeField]
	private AnchorPoint m_AnchorPoint;
	
	[SerializeField]
	private Transform m_AnchorObject;
	[SerializeField]
	private Transform m_Parent;
	[SerializeField]
	private float m_WidgetsDistance;
	
	private Vector2 m_IntervalDistance = Vector2.zero;
	
	private bool m_IsNeedLayout;
	
	public void Relayout()
	{
		this.m_IsNeedLayout = true;
	}
	
	public void RelayoutImmediately()
	{
		Vector2 startPosition = this.m_Parent.transform.position;
		
		List<Transform> children = new List<Transform>();
		for(int i = 0; i < this.m_Parent.childCount; i ++)
		{
			children.Add(this.m_Parent.GetChild(i));
		}
		children.Sort((x, y) => {return string.Compare(x.name,y.name);});
		
		for(int i = 0; i < children.Count; i ++)
		{
			Transform needLayoutTransform = children[i];
			if(this.m_IntervalDistance.Equals(Vector2.zero))
			{
				this.m_IntervalDistance = this.m_Arrangement == UIGrid.Arrangement.Horizontal ? 
					new Vector2(NGUIMath.CalculateAbsoluteWidgetBounds(needLayoutTransform).size.x + this.m_WidgetsDistance, 0) :
						new Vector2(0, NGUIMath.CalculateAbsoluteWidgetBounds(needLayoutTransform).size.y + this.m_WidgetsDistance);
				
				Vector2 anchorPosition = new Vector2(this.m_AnchorObject.position.x, this.m_AnchorObject.position.y);
				startPosition = this.m_AnchorPoint == AnchorPoint.MinMost ? anchorPosition :
					this.m_AnchorPoint == AnchorPoint.MaxMost ? anchorPosition - this.m_IntervalDistance * (this.m_Parent.childCount - 1)
						:  anchorPosition - this.m_IntervalDistance * (this.m_Parent.childCount - 1) / 2;
			}
			
			Vector2 calculatedPosition = startPosition + i * this.m_IntervalDistance;
			
			needLayoutTransform.transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, needLayoutTransform.transform.position.z);
		}
	}
	
	void Start()
	{
		this.m_IsNeedLayout = true;
	}

	void Update () 
	{
		if(this.m_IsNeedLayout)
		{
			this.m_IsNeedLayout = false;
			this.RelayoutImmediately();
		}
	}
}
