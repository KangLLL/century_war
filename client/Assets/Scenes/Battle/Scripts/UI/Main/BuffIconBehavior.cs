using UnityEngine;
using System.Collections;

public class BuffIconBehavior : MonoBehaviour 
{
	[SerializeField]
	private UISprite m_Icon;
	
	public string IconName { get;set; }
	
	void Start () 
	{
		this.m_Icon.spriteName = this.IconName;
		this.m_Icon.MakePixelPerfect();
	}
}
