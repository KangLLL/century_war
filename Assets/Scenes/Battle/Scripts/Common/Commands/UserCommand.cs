using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UserCommand<T>  
{
	public T Identity { get;set; }
	public Vector3 Position { get;set; }
}
