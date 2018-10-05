using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class DropArmyInformation : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_QuantityLabel;
	[SerializeField]
	private UILabel m_LevelLabel;
	
	private int m_Quantity;
	private int m_Level;
	
	public int Quantity
	{
		get
		{
			return this.m_Quantity;
		}
		set
		{
			this.m_Quantity = value;
		}
	}
	
	public int Level	
	{
		get
		{
			return this.m_Level;
		}
		set
		{
			this.m_Level = value;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		this.m_QuantityLabel.text = "X" + this.m_Quantity;
		this.m_LevelLabel.text = StringConstants.PROMPT_LEVEL + this.m_Level.ToString();
	}
}
