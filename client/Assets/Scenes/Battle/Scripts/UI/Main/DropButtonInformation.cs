using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropButtonInformation<T,P> : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_QuantityLabel;
	[SerializeField]
	private ButtonStateBackground m_Icon;
	[SerializeField]
	private ButtonStateBackground m_Background;
	
	public T Type { get; set; }
	public List<P> Armies { get; set; }
	
	public bool Dropable { get { return this.Armies.Count > 0; } }
	
	// Use this for initialization
	public virtual void Start () 
	{
		this.m_QuantityLabel.text = "X" + this.Armies.Count.ToString();
	}
	
	void Update()
	{
		if(this.Armies.Count == 0)
		{
			this.m_Background.SetDisableSprite();
			this.m_Icon.SetDisableSprite();
		}
	}
	
	public P DropArmy()
	{
		P result = this.Armies[0];
		this.Armies.RemoveAt(0);
		this.m_QuantityLabel.text = "X" + this.Armies.Count.ToString();
		return result;
	}
}
