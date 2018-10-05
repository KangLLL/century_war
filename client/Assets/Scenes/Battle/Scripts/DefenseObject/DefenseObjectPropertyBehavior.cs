using UnityEngine;
using System.Collections;

using ConfigUtilities;

public class DefenseObjectPropertyBehavior : ObstaclePropertyBehavior, IObstacleInfo
{
	private Transform m_CachedAnchorTransform;
	
	public long DefenseObjectID { get;set; }
	
	void Start()
	{
	}
	
	public Transform AnchorTransform
	{
		get
		{
			if(this.m_CachedAnchorTransform == null)
			{
				this.m_CachedAnchorTransform = this.transform.FindChild(ClientStringConstants.ANCHOR_OBJECT_NAME);
			}
			return this.m_CachedAnchorTransform;
		}
	}
}
