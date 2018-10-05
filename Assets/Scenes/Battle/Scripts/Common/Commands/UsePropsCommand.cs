using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UsePropsCommand : UserCommand<int> 
{
	public PropsType PropsType { get;set; }
}
