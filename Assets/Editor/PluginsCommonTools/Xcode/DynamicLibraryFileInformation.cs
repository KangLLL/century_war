using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicLibraryFileInformation : ProjectFileInformation 
{
	public DynamicLibraryType DynamicLibraryType { get;set; }
	//public bool IsWeak { get;set; }
	
	private static Dictionary<DynamicLibraryType, string> dynamicLibraryKeyDict = new Dictionary<DynamicLibraryType, string>()
	{
		{DynamicLibraryType.Sqlite3 , "libsqlite3.dylib"},
		{DynamicLibraryType.Libz,"libz.dylib"}
	};
	
	public override void Initialize ()
	{
		if(this.DynamicLibraryType != DynamicLibraryType.Custom)
		{
			this.FileName = dynamicLibraryKeyDict[this.DynamicLibraryType];
		}
		if(string.IsNullOrEmpty(this.FilePath))
		{
			this.FilePath = "usr/lib/" + dynamicLibraryKeyDict[this.DynamicLibraryType];
		}
		this.FileKnownType = "compiled.mach-o.dylib";
		
		this.Phase = new ProjectPhaseInformation();
		this.Phase.PhaseType = ProjectPhaseType.Framework;
		this.SourceTree = "SDKROOT";
		this.ParentGroupName = "CustomTemplate";
		this.IsNeedAddNameToReference = true;
	}
}
