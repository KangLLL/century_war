using UnityEngine;
using System.Collections;

public class TweenColortk2dTextMesh : UITweener 
{
	public Color m_From = Color.white;
	public Color m_To = Color.white;
	
	public Color Color
	{
		get
		{
            return this.m_tk2dTextMesh.color;
		}
		set
		{
            this.m_tk2dTextMesh.color = value; 
            this.m_tk2dTextMesh.Commit();
		}
	}
	 
    private tk2dTextMesh m_tk2dTextMesh;
	void Awake()
	{
        this.m_tk2dTextMesh = gameObject.GetComponentInChildren<tk2dTextMesh>();
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
