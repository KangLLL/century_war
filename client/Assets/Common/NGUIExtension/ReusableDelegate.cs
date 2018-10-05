using UnityEngine;
using System.Collections;

public abstract class ReusableDelegate : MonoBehaviour 
{
	public abstract int TotalNumberOfCells { get; }
    public abstract void InitialCell(int index, GameObject cell);
}
