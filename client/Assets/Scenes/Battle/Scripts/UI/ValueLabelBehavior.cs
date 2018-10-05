using UnityEngine;
using System.Collections;

public class ValueLabelBehavior : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_ValueLabel;
	[SerializeField]
	private float m_FlashSecond;
	
	public void RefreshToValue(int newValue)
	{
		Hashtable args = new Hashtable();
		int current = int.Parse(this.m_ValueLabel.text);
		args.Add("from", current);
		args.Add("to", newValue);
		args.Add("time", this.m_FlashSecond);
		args.Add("onupdate","Flash");
		iTween.ValueTo(this.gameObject, args);
	}
	
	public void Flash(float v)
	{
		this.m_ValueLabel.text = ((int)v).ToString();
	}
}
