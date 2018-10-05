using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SourceFileInformation : ProjectFileInformation
{
	private static Dictionary<string, string> fileKnownTypeDict = new Dictionary<string, string>()
	{
		{".h","sourcecode.c.h"},
		{".m","sourcecode.c.objc"},
		{".mm","sourcecode.cpp.objcpp"},
		{".a","archive.ar"}
	};
	
	private const string DEFAULT_FILE_TYPE = "file";
	
	public override void Initialize ()
	{
		string extension = System.IO.Path.GetExtension(this.FileName);
		if(this.FileName.Contains("+"))
		{
			this.FileName = "\"" + this.FileName + "\"";
		}
		this.FileKnownType = fileKnownTypeDict.ContainsKey(extension) ? 
			fileKnownTypeDict[extension] : DEFAULT_FILE_TYPE;
		this.SourceTree = "\"<group>\"";
		this.IsNeedAddNameToReference = true;
		if(!extension.Equals(".h"))
		{
			this.Phase = new ProjectPhaseInformation() { PhaseType = ProjectPhaseType.Source };
		}
	}
}
