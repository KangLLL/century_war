using UnityEngine;
using System.Collections;

public class TweenColortk2dSprite : UITweener 
{
	public Color m_From = Color.white;
	public Color m_To = Color.white;
	
	public Color Color
	{
		get
		{
			return this.m_Sprite.color;
		}
		set
		{
			this.m_Sprite.color = value;
		}
	}
	
	private tk2dSprite m_Sprite;
	
	void Awake()
	{
		this.m_Sprite = gameObject.GetComponentInChildren<tk2dSprite>();
	}
	
	protected override void OnUpdate (float factor, bool isFinished)
	{
		this.Color = Color.Lerp(this.m_From, this.m_To, factor);
	}
	
	static public TweenColortk2dSprite Begin (GameObject go, float duration, Color color)
	{
		TweenColortk2dSprite comp = UITweener.Begin<TweenColortk2dSprite>(go, duration);
		comp.m_From = comp.Color;
		comp.m_To = color;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
