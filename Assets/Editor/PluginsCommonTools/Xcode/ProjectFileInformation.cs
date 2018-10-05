using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectFileInformation : ProjectItemInformation
{
	public string FileName { get;set; }
	public string FilePath { get;set; }
	public ProjectFileType FileType { get;set; }
	public bool IsNeedAddNameToReference { get;set; }
	
	public ProjectPhaseInformation Phase { get;set; }

	public string FileKnownType { get;set; }
	public string SourceTree { get;set; }
	
	public virtual void Initialize()
	{
	}
}
