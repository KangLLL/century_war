using UnityEngine;
using System.Collections;

public class SimpleDelegate : ReusableDelegate 
{
	public override int TotalNumberOfCells 
	{
		get 
		{
			return 2000;
		}
	}
	
	public override void InitialCell (int index, GameObject cell)
	{
		cell.GetComponentInChildren<UILabel>().text = index.ToString();
	}
}
