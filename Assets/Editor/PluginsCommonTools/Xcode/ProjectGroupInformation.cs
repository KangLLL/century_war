using UnityEngine;
using System.Collections;

public class ProjectGroupInformation : ProjectItemInformation 
{
	public string GroupName { get;set; }
	public bool IsRoot { get;set; }
	public string GroupPath { get;set; }
}
