﻿using UnityEngine;
using System.Collections;

public class DropPropsInformation : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_QuantityLabel;
	
	private int m_Quantity;
	
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
	
	void Start () 
	{
		this.m_QuantityLabel.text = "X" + this.m_Quantity;
	}
}
