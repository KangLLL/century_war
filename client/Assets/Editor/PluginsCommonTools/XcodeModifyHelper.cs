using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

public enum ModifyType
{
	Append,
	Replace
}

public class XcodeModifyHelper  
{
	private const string DEPLOY_PROJECT_NAME = "Unity-iPhone";
	private const string DEPLOY_TARGET_NAME = "Unity-iPhone";
	private const string PROJECT_SETTING_FILE_NAME = "project.pbxproj";
	
	private const string ROOT_GROUP_NAME = "CustomTemplate";
	private const string CLASS_GROUP_NAME = "Classes";
	private const string LIBRARY__GROUP_NAME = "Libraries";
	
	private const string PROJECT_INFO_LIST_NAME = "Info.plist";
	
	
	private static Dictionary<ProjectSettingType, string> settingKeyDict = new Dictionary<ProjectSettingType, string>()
	{
		{ProjectSettingType.FrameworkSearchPath,"FRAMEWORK_SEARCH_PATHS"},
		{ProjectSettingType.OtherLinkFlag,"OTHER_LDFLAGS"},
		{ProjectSettingType.LibrarySearchPath, "LIBRARY_SEARCH_PATHS"}
	};
	/*
	private static Dictionary<ProjectFileType, string> fileKeyDict = new Dictionary<ProjectFileType, string>()
	{
		{ProjectFileType.Source, "Sources"},
		{ProjectFileType.Framework, "Framework"},
		{ProjectFileType.Resource, "Resources"}
	};
	
	private static Dictionary<ProjectPhaseType, string> phaseKeyDict = new Dictionary<ProjectPhaseType, string>()
	{
		{ProjectPhaseType.Source, "Sources"},
		{ProjectPhaseType.Framework, "Frameworks"},
		{ProjectPhaseType.Resource, "Resources"}
	};
	
	private static Dictionary<FrameworkType, string> frameworkKeyDict = new Dictionary<FrameworkType, string>()
	{
		{FrameworkType.AddressBook, "AddressBook.framework"},
		{FrameworkType.CoreTelephony,"CoreTelephony.framework"},
		{FrameworkType.CoreGraphics, "CoreGraphics.framework"},
		{FrameworkType.Foundation,"Foundation.framework"},
		{FrameworkType.MessageUI,"MessageUI.framework"},
		{FrameworkType.QuartzCore,"QuartzCore.framework"},
		{FrameworkType.StoreKit,"StoreKit.framework"},
		{FrameworkType.SystemConfiguration,"SystemConfiguration.framework"},
		{FrameworkType.UIKit,"UIKit.framework"}
	};
	
	private static Dictionary<string, string> fileKnownTypeDict = new Dictionary<string, string>()
	{
		{".h","sourcecode.c.h"},
		{".m","sourcecode.c.objc"},
		{".a","archive.ar"}
	};
	
	private static Dictionary<ProjectFileType ,FileTypeInformation> fileTypeDict = new Dictionary<ProjectFileType, FileTypeInformation>()
	{
		{ProjectFileType.Source , new FileTypeInformation(){GroupName = CLASS_GROUP_NAME, PhaseType = ProjectPhaseType.Source}},
		{ProjectFileType.Resource , new FileTypeInformation(){GroupName = LIBRARY__GROUP_NAME, PhaseType = ProjectPhaseType.Resource}},
		{ProjectFileType.Framework, new FileTypeInformation(){GroupName = ROOT_GROUP_NAME, PhaseType = ProjectPhaseType.Framework}}
	};
	*/
	/*
	private static Dictionary<string, ProjectFileType> extesionFileTypeDict = new Dictionary<string, ProjectFileType>()
	{
		{".h", ProjectFileType.Source},
		{".m", ProjectFileType.Source},
		{".a", ProjectFileType.Framework}
	};
	*/
	
	private static Dictionary<ProjectPhaseType, string> buildPhaseDict = new Dictionary<ProjectPhaseType, string>();
	private static List<string> buildConfigurationGuids = new List<string>();
	private static string projectContent;
    private static string targetName;
	private static string projectPath;
	
	private static void InitializeConfigurationGuid()
	{
		buildConfigurationGuids.Clear();
		
		string pattern = "([A-Z0-9]+) /\\* " + Regex.Escape(targetName) +
			" \\*/ = \\{\n[ \t]+isa = PBXNativeTarget;(?:.|\n)+?buildConfigurationList = ([A-Z0-9]+)(?:.|\n)+?buildPhases = \\(\n((?:.|\n)+?)\\);";
		Match match = Regex.Match(projectContent, pattern);
		if(!match.Success)
		{
			Debug.Log("Can't recover: Unable to find the build phases from your target: " + targetName);
			return;
		}
		else
		{
			string configurationGuid = match.Groups[2].Value;
			pattern = "/\\* Begin XCConfigurationList section \\*/(?:.|\n)+?" + configurationGuid + 
				"(?:.|\n)+?buildConfigurations = \\(\n\t+([A-Z0-9]+) /\\* Release \\*/,\n\t+([A-Z0-9]+) /\\* Debug \\*/";
			match = Regex.Match(projectContent, pattern);
			if(match.Success)
			{				
				buildConfigurationGuids.Add(match.Groups[1].Value);
				buildConfigurationGuids.Add(match.Groups[2].Value);
			}
		}
	}
	
	private static string GetPhaseGuid(ProjectPhaseInformation phaseInfo)	
	{
		if(!buildPhaseDict.ContainsKey(phaseInfo.PhaseType))
		{
			string pattern = "([A-Z0-9]+) /\\* " + Regex.Escape(targetName) +
				" \\*/ = \\{\n[ \t]+isa = PBXNativeTarget;(?:.|\n)+?buildConfigurationList = ([A-Z0-9]+)(?:.|\n)+?buildPhases = \\(\n((?:.|\n)+?)\\);";
			Match match = Regex.Match(projectContent, pattern);
		
			if(!match.Success)
			{
				Debug.Log("Can't recover: Unable to find the build phases from your target: " + targetName);
				return string.Empty;
			}
			else
			{
				string phasesContent = match.Groups[3].Value;
				string phaseGuid =  FindPhaseGuid(phasesContent,  phaseInfo.PhaseName);
				if(phaseGuid == null)
				{
					Debug.Log("Can't recover: Unable to find the " + phaseInfo.PhaseName + " phase from your target: " + targetName);
					return string.Empty;
				}
				else
				{
					buildPhaseDict.Add(phaseInfo.PhaseType, phaseGuid);
				}
			}	
		}
		return buildPhaseDict[phaseInfo.PhaseType];
	}
	
	private static string FindPhaseGuid(string phaseContent, string phaseName)
	{
		string phasePattern = "([A-Z0-9]+) /\\* " + phaseName + " \\*/";
		Match m = Regex.Match(phaseContent, phasePattern);
		if(!m.Success)
		{
			Debug.Log("Can't find " + phaseName +  " from your target");
			return null;
		}
		return  m.Groups[1].Value;
	}
	
	public static string GetXcodeProjectFilePath(string projectFilePath)
	{
		string projectName = DEPLOY_PROJECT_NAME + ".xcodeproj";
		string xcodeProjectFilePath = System.IO.Path.Combine(projectFilePath,projectName);
		string projectSettingFileName = System.IO.Path.Combine(xcodeProjectFilePath,PROJECT_SETTING_FILE_NAME);
		
		
		return projectSettingFileName;
	}
	
	public static void ModifyXcodeProject(List<ProjectItemInformation> items, string projectFilePath)
	{
		string projectSettingFileName = GetXcodeProjectFilePath(projectFilePath);
		FileStream stream = File.Open(projectSettingFileName,FileMode.Open,FileAccess.Read);
		StreamReader reader = new StreamReader(stream);
		string content = reader.ReadToEnd();
		reader.Close();
		
		projectPath = projectFilePath;
		projectContent = content;
		targetName = DEPLOY_TARGET_NAME;
		
		buildPhaseDict.Clear();
		InitializeConfigurationGuid();
		
		foreach(ProjectItemInformation item in items) //(string fileName in files) 
		{
			if(item is ProjectGroupInformation)
			{
				ProjectGroupInformation info = item as ProjectGroupInformation;
				string referenceGroupGuid = AddGroup(ref content, info);
				AddFileToGroup(ref content, info.ParentGroupName, referenceGroupGuid, info.GroupName);
			}
			else
			{
				ProjectFileInformation info = item as ProjectFileInformation;
				info.Initialize();
				
				if(info is FrameworkFileInformation)
				{
					FrameworkFileInformation frameworkInfo = info as FrameworkFileInformation;
					if(frameworkInfo.FrameworkType == FrameworkType.Custom)
					{
						AddFrameworkConfiguration(ref content, frameworkInfo.FilePath);
						frameworkInfo.FilePath = frameworkInfo.FileName;
					}
				}
				
				if(info is ResourceFileInformation)
				{
					info.FilePath = FileOperateHelper.GetRelativePathFromAbsolutePath(projectPath, info.FilePath);
				}
				else if(info is SourceFileInformation)
				{
					info.FilePath = FileOperateHelper.GetRelativePathFromAbsolutePath(System.IO.Path.Combine(projectPath,"Classes"), info.FilePath);
				}
				
				string referenceFileGuid = AddFileReference(ref content, info);
				AddFileToGroup(ref content, info.ParentGroupName , referenceFileGuid, info.FileName);
				if(info.Phase !=  null)
				{
					string buildFileGuid = AddBuildFile(ref content, info, referenceFileGuid);
					AddBuildFileToPhase(ref content, buildFileGuid, info.FileName, info.Phase);
				}
			}
		}
		
		stream = File.Open(projectSettingFileName,FileMode.Create,FileAccess.Write);
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(content);
		writer.Close();
		
		Debug.Log("Success!");
	}
	
	/*
	public static void ModifyXcodeProject(string fileFolderPath, string projectFilePath)
	{
		string projectName = DEPLOY_PROJECT_NAME + ".xcodeproj";
		string xcodeProjectFilePath = System.IO.Path.Combine(projectFilePath,projectName);
		string projectSettingFileName = System.IO.Path.Combine(xcodeProjectFilePath,PROJECT_SETTING_FILE_NAME);
		FileStream stream = File.Open(projectSettingFileName,FileMode.Open,FileAccess.Read);
		StreamReader reader = new StreamReader(stream);
		string content = reader.ReadToEnd();
		reader.Close();
		
		InitializeBuildPhaseAndConfigurationGuid(content, DEPLOY_TARGET_NAME);
		
		if(Directory.Exists(fileFolderPath))
		{
			string groupName = fileFolderPath.Substring(fileFolderPath.LastIndexOf("/") + 1);
			//string groupName = System.IO.Path.GetFileName(fileFolderPath);
			//AddGroup(ref content, groupName);
			string groupGuid = AddGroup(ref content, groupName);
			AddFileToGroup(ref content, ROOT_GROUP_NAME, groupGuid, groupName);
			string[] files = Directory.GetFiles(fileFolderPath);
			foreach(string filePath in files)
			{
				string fileExtension = System.IO.Path.GetExtension(filePath);
				string fileName = System.IO.Path.GetFileName(filePath);
				if(extesionFileTypeDict.ContainsKey(fileExtension))
				{
					ProjectFileType fileType = extesionFileTypeDict[System.IO.Path.GetExtension(fileName)];
					string referenceFileGuid = AddFileReference(ref content, fileName, fileType);
					AddFileToGroup(ref content, groupName, referenceFileGuid, fileName);
					string buildFileGuid = AddBuildFile(ref content, fileName, referenceFileGuid, fileType);
					AddBuildFileToPhase(ref content, buildFileGuid, fileName, fileTypeDict[fileType].PhaseType);
					if(fileExtension.Equals(".a"))
					{
						AddLibrarySearchConfiguration(ref content, groupName);
					}
				}
			}
			
			stream = File.Open(projectSettingFileName,FileMode.Create,FileAccess.Write);
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Close();
		}
		else
		{
			Debug.Log("There is no folder!");
		}
		
		Debug.Log("Success!");
	}
	*/
	public static void ModifyBuildSetting(string projectFilePath, ProjectSettingType settingType,
		string newValue, ModifyType modifyType)
	{
		string projectSettingFileName = GetXcodeProjectFilePath(projectFilePath);
		FileStream stream = File.Open(projectSettingFileName,FileMode.Open,FileAccess.Read);
		StreamReader reader = new StreamReader(stream);
		string content = reader.ReadToEnd();
		reader.Close();
		
		projectContent = content;
		targetName = DEPLOY_TARGET_NAME;
		
		InitializeConfigurationGuid();
		
		ModifyBuildSetting(ref content, settingType, newValue, modifyType);
		
		stream = File.Open(projectSettingFileName,FileMode.Create,FileAccess.Write);
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(content);
		writer.Close();
	}
	
	private static void ModifyBuildSetting(ref string projectContent, ProjectSettingType settingType, 
		string newValue, ModifyType modifyType)
	{	
		foreach(string configurationGuid in buildConfigurationGuids)
		{
			string pattern =  "/\\* Begin XCBuildConfiguration section \\*/(?:\n|.)+?" + configurationGuid +
				" /\\* (\\w+?) \\*/ = \\{(?:.|\n)+?buildSettings = \\{\n((?:.|\n)+?)name = \\1;";
			
			Match m = Regex.Match(projectContent, pattern);
			
			if(!m.Success)
			{
				Debug.Log("No build setting section!");
			}
			else
			{
				string settingContent = m.Groups[2].Value;
				string settingKey = settingKeyDict[settingType];
				pattern = Regex.Escape(settingKey) + " = \\(?\n?((?:.|\n)+?)\\)?;";
				
				Match contentMatch = Regex.Match(settingContent, pattern);
				if(!contentMatch.Success)
				{
					string addString = "\t\t\t\t" + settingKey + " = ";
					if(settingType == ProjectSettingType.LibrarySearchPath || settingType == ProjectSettingType.FrameworkSearchPath)
					{
						addString = addString + "(\n\t\t\t\t\t\"$(inherited)\",\n\t\t\t\t\t" + newValue + ",\n\t\t\t\t);\n";
					}
					else
					{
						addString = addString + newValue + ";\n";
					}
					projectContent = projectContent.Substring(0, m.Groups[2].Index) +
						addString + projectContent.Substring(m.Groups[2].Index);
				}
				else
				{
					string contentValue = contentMatch.Groups[1].Value;
					pattern = Regex.Escape(newValue);
					Match valueMatch = Regex.Match(contentValue, pattern);
					if(!valueMatch.Success)
					{
						string addString =  newValue;
						if(modifyType == ModifyType.Append)
						{
								addString = contentMatch.Groups[1].Value + addString + ",\n";
						}
						projectContent = projectContent.Substring(0,m.Groups[2].Index) + 
							settingContent.Substring(0, contentMatch.Groups[1].Index) + 
								addString + settingContent.Substring(contentMatch.Groups[1].Index + contentMatch.Groups[1].Length)
								+ projectContent.Substring(m.Groups[2].Index + m.Groups[2].Length);
					}
				}
			}
		}
	}
	
	
	private static void AddLibrarySearchConfiguration(ref string projectContent, string libraryPath)
	{
		ModifyBuildSetting(ref projectContent, ProjectSettingType.LibrarySearchPath, libraryPath, ModifyType.Append);
	}
	
	private static void AddFrameworkConfiguration(ref string projectContent, string frameworkPath) 
	{
		string directoryPath = System.IO.Path.GetDirectoryName(frameworkPath);
		string relativePath = FileOperateHelper.GetRelativePathFromAbsolutePath(projectPath, directoryPath);
		string newValue = "\"\\\"$(SRCROOT)/" + relativePath + "\\\"\"";
		
		//Debug.Log("the new value is: " + newValue);
		ModifyBuildSetting(ref projectContent, ProjectSettingType.FrameworkSearchPath, newValue, ModifyType.Append);
	}
	
	private static string AddFileReference(ref string projectContent, ProjectFileInformation file)
	{	
		string fileName = file.FileName;
		//Debug.Log(fileName);
		
		string pattern = "/\\* Begin PBXFileReference section \\*/\n((?:.|\n)+?)/\\* End PBXFileReference section \\*/";
		Match match = Regex.Match(projectContent, pattern);
		if(!match.Success)
		{
			Debug.Log("There is no file reference section");
			return null;
		}
		else
		{
			string referenceContent = match.Groups[1].Value;
			string filePath = file.FilePath;
			
			string referencePattern = "([A-Z0-9]+) /\\* " + Regex.Escape(fileName) + " \\*/ = {isa = PBXFileReference;";
			Match m = Regex.Match(referenceContent,referencePattern);
			
			if(m.Success)
			{
				Debug.Log("The file is already added");
				return m.Groups[1].Value;
			}
			else
			{
				string guid = GenerateGUID();
				string addLine = null;
				
				if(file.IsNeedAddNameToReference)
				{
					addLine =  "\t\t" + guid + " /* " + fileName + 
						" */ = {isa = PBXFileReference; lastKnownFileType = " + file.FileKnownType +"; name = " + 
							fileName + "; path = " + filePath + "; sourceTree = " + file.SourceTree + "; };\n";
				}
				else
				{
					addLine = "\t\t" + guid + " /* " + fileName + 
						" */ = {isa = PBXFileReference; lastKnownFileType = " + file.FileKnownType + 
						"; path = " + filePath + "; sourceTree = " + file.SourceTree + "; };\n";
				}
				Debug.Log(addLine);
				projectContent = projectContent.Substring(0,match.Groups[1].Index) + addLine + projectContent.Substring(match.Groups[1].Index);
				return guid;
			}
		}
	}
	
	private static string AddGroup(ref string projectContent, ProjectGroupInformation groupInfo)
	{
		string pattern = "([A-Z0-9]+) /\\* " + Regex.Escape(groupInfo.GroupName) + " \\*/ = \\{\n[ \t]+isa = PBXGroup;\n[ \t]+children = \\(\n((?:.|\n)+?)\\);";
		Match match = Regex.Match(projectContent, pattern);
		if(match.Success)
		{
			Debug.Log("The group is Already exists.");
			return match.Groups[1].Value;
		}
		else
		{
			pattern = "/\\* Begin PBXGroup section \\*/((?:.|\n)+?)/\\* End PBXGroup section \\*/";
			match = Regex.Match(projectContent, pattern);
			
			string groupGuid = GenerateGUID();
			
			string addString = string.Empty;
			if(string.IsNullOrEmpty(groupInfo.GroupPath))
			{
				addString = string.Format("{0}{1} /* {2} */ = {3}{0}\tisa = PBXGroup;{0}\tchildren = ({0}\t);{0}\tname = {2};{0}\tsourceTree =  \"<group>\";{0}{4};",
				"\n\t\t\t\t",groupGuid,groupInfo.GroupName,"{","}");
			}
			else
			{
				string path = groupInfo.IsRoot ? FileOperateHelper.GetRelativePathFromAbsolutePath(projectPath, groupInfo.GroupPath) : groupInfo.GroupPath;
				
				addString = string.Format("{0}{1} /* {2} */ = {4}{0}\tisa = PBXGroup;{0}\tchildren = ({0}\t);{0}\tname={2};{0}\tpath = {3};{0}\tsourceTree =  \"<group>\";{0}{5};",
				"\n\t\t\t\t",groupGuid,groupInfo.GroupName,path,"{","}");
			}
			projectContent = projectContent.Substring(0,match.Groups[1].Index) + addString + projectContent.Substring(match.Groups[1].Index);
			return groupGuid;
		}
	}
	
	private static void AddFileToGroup(ref string projectContent, string groupName, string fileGuid, string fileName)
	{
		string pattern = "/\\* " + Regex.Escape(groupName) + " \\*/ = \\{\n[ \t]+isa = PBXGroup;\n[ \t]+children = \\(\n((?:.|\n)+?)\\);";
		Match match = Regex.Match(projectContent, pattern);
		Debug.Log(groupName);
		if(!match.Success)
		{
			Debug.Log("1");
			ProjectGroupInformation groupInfo = new ProjectGroupInformation(){ GroupName = groupName };
			string groupGuid = AddGroup(ref projectContent, groupInfo);
			AddFileToGroup(ref projectContent, "CustomTemplate", groupGuid, groupInfo.GroupName);
			match = Regex.Match(projectContent, pattern);
			
			Debug.Log(match.Groups[1].Value);
		}
		
		string childrenContent = match.Groups[1].Value;
		Match m = Regex.Match(childrenContent, Regex.Escape(fileGuid));
		if(m.Success)
		{
			Debug.Log("This file is already a member of the " + groupName + " group.");
			return;
		}
		else
		{
			string addLine = "\t\t\t\t" + fileGuid + " /* " + fileName + " */,\n";
			projectContent = projectContent.Substring(0,match.Groups[1].Index) + addLine + projectContent.Substring(match.Groups[1].Index);
		}
	}
	
	private static string AddBuildFile(ref string projectContent, ProjectFileInformation fileInfo, string refFileGuid)
	{
		string pattern = "/\\* Begin PBXBuildFile section \\*/\n((?:.|\n)+?)/\\* End PBXBuildFile section \\*/"; 
		Match match = Regex.Match(projectContent, pattern);
		if(!match.Success)
		{
			Debug.Log("Couldn't find PBXBuildFile section.");
			return null;
		}
		else
		{
			string buildFileContent = match.Groups[1].Value;
			string buildFilePattern = "([A-Z0-9]+).+?fileRef = " + Regex.Escape(refFileGuid);
			Match m = Regex.Match(buildFileContent, buildFilePattern);
			if(m.Success)
			{
				Debug.Log("This build file already exists: " + fileInfo.FileName);
				return m.Groups[1].Value;
			}
			else
			{
				string guid = GenerateGUID();
				string addLine = "\t\t" + guid + " /* " + fileInfo.FileName + " in " + fileInfo.FileType + 
					" */ = {isa = PBXBuildFile; fileRef = " + refFileGuid + 
					" /* " + fileInfo.FileName + " */; ";
				FrameworkFileInformation frameworkInfo = fileInfo as FrameworkFileInformation;
				if(frameworkInfo != null && frameworkInfo.IsWeak)
				{
					addLine = addLine + "settings = {ATTRIBUTES = (Weak, ); }; ";
				}
				
				addLine = addLine + "};\n";
				projectContent = projectContent.Substring(0,match.Groups[1].Index) + addLine + projectContent.Substring(match.Groups[1].Index);
				return guid;
			}
		}
	}
	
	private static void AddBuildFileToPhase(ref string projectContent, string buildFileGuid, string fileName, ProjectPhaseInformation phaseInfo)
	{
		//Debug.Log(fileName);
		//Debug.Log(fileName + " : " + phaseInfo.PhaseType.ToString());
		//Debug.Log (phaseInfo.PhaseName);
		string phaseName = phaseInfo.PhaseName;
		
		if(System.IO.Path.GetExtension(fileName).Equals(".h"))
			return;
		string phaseGuid = GetPhaseGuid(phaseInfo);//buildPhaseDict[phaseInfo.PhaseType];
		string pattern = Regex.Escape(phaseGuid) + " /\\* " + Regex.Escape(phaseName) + " \\*/ = \\{(?:.|\n)+?files = \\(((?:.|\n)+?)\\)";
		Match match = Regex.Match(projectContent, pattern);
		if(!match.Success)
		{
			Debug.Log("Couldn't find the " + phaseName + " phase.");
			return;
		}
		else
		{
			string phaseContent = match.Groups[1].Value;
			Match m = Regex.Match(phaseContent, Regex.Escape(buildFileGuid));
			if(m.Success)
			{
				Debug.Log("The file has already been added.");
				return;
			}
			else
			{
				string addLine = "\t\t\t\t" + buildFileGuid + " /* "+fileName +" in " + phaseName +" */,\n";
				projectContent = projectContent.Substring(0, match.Groups[1].Index) + addLine + projectContent.Substring(match.Groups[1].Index);
			}
		}
	}
	
	private static string GenerateGUID()
	{
		string result = Guid.NewGuid().ToString();
		result = result.ToUpper();
		result = result.Replace("-",string.Empty);
		return result.Substring(0,24);
	}
	
	public static void AddEntityToInfoPlist(string projectPath, string key, object values)
	{
		string infoFilePath = System.IO.Path.Combine(projectPath,PROJECT_INFO_LIST_NAME);
		FileStream fs = File.Open(infoFilePath, FileMode.Open, FileAccess.Read);
		StreamReader sr = new StreamReader(fs);
		string content = sr.ReadToEnd();
		sr.Close();
		
		string pattern = "(<plist version=\"\\d+\\.+\\d+\">\n<dict>\n)((?:.|\n)+?)(\n</dict>\n</plist>)";
		Match match = Regex.Match(content, pattern);
		
		string entityContent = match.Groups[2].Value;
		string entityPattern = "(\t<key>" + key + "</key>\n(?:(?:.|\n)+?))\t<key>";
		Match entityMatch = Regex.Match(entityContent, entityPattern);
		
		if(entityMatch.Success)
		{
			entityContent = entityContent.Remove(entityMatch.Groups[1].Index, entityMatch.Groups[1].Length);
		}
		
		string addString = AddObjectToInfoPlist(key, values, 0);
		entityContent = addString + entityContent;
		string resultString = match.Groups[1].Value + entityContent + match.Groups[3].Value;
		
		fs = File.Open(infoFilePath, FileMode.Create, FileAccess.Write);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(resultString);
		sw.Close();
	}
	
	private static string AddObjectToInfoPlist(string key, object content, int layer)
	{
		if(content is ArrayList)
		{
			return AddArrayToInfoPlist(key, content as ArrayList, layer);
		}
		else if(content is Hashtable)
		{
			return AddDictionaryToInfoPlist(key, content as Hashtable, layer);
		}
		else if(content is ValueType)
		{
			return AddValueTypeToInfoPlist(key, content, layer);
		}
		else
		{
			return AddStringToInfoPlist(key, content.ToString(), layer);
		}
	}
	
	private static string GetIndentString(int layer)
	{
		string indentString = "\t";
		for(int i = 0; i < layer; i ++)
		{
			indentString = indentString + "\t";
		}
		return indentString;
	}
	
	private static string AddValueTypeToInfoPlist(string key, object valueValue, int layer)
	{
		string resultString = string.Empty;
		string indentString = GetIndentString(layer);
		if(key != null)
		{
			resultString = string.Format("{0}<key>{1}</key>\n", indentString, key);
		}
		
		if(valueValue is Boolean)
		{
			resultString = string.Format("{0}{1}<{2}/>\n",resultString, indentString, valueValue.ToString().ToLower());
		}
		else if(valueValue is Int32)
		{
			resultString = string.Format("{0}{1}<integer>{2}</integer>\n",resultString, indentString, valueValue);	
		}
		
		return resultString;
	}
	
	private static string AddStringToInfoPlist(string key, string stringValue, int layer)
	{
		string resultString = string.Empty;
		string indentString = GetIndentString(layer);
		if(key != null)
		{
			resultString = string.Format("{0}<key>{1}</key>\n", indentString, key);
		}
		resultString = string.Format("{0}{1}<string>{2}</string>\n",resultString, indentString, stringValue);
		return resultString;
	}
	
	private static string AddArrayToInfoPlist(string key, ArrayList arrayValue, int layer)
	{
		string indentString = GetIndentString(layer);
		string resultString = "";
		if(key != null)
		{
			resultString = string.Format("{0}<key>{1}</key>\n", indentString, key);
		}
		
		resultString = string.Format("{0}{1}<array>\n",resultString, indentString);
		string contentString = string.Empty;
		foreach(object o in arrayValue)
		{
			contentString = string.Format("{0}{1}",contentString, AddObjectToInfoPlist(null, o, layer + 1));
		}
		resultString = string.Format("{0}{1}\n{2}</array>\n",resultString,contentString,indentString); 
		return resultString;
	}
	
	private static string AddDictionaryToInfoPlist(string key, Hashtable dictValue, int layer)
	{
		string indentString = GetIndentString(layer);
		string resultString = "";
		if(key != null)
		{
			resultString = string.Format("{0}<key>{1}</key>\n", indentString, key);
		}
		
		resultString = string.Format("{0}{1}<dict>\n",resultString, indentString);
		string contentString = string.Empty;
		
		foreach(DictionaryEntry entry in dictValue)
		{
			contentString = string.Format("{0}{1}\n",contentString, AddObjectToInfoPlist(entry.Key.ToString(), entry.Value, layer + 1));
		}
		resultString = string.Format("{0}{1}\n{2}</dict>\n",resultString, contentString, indentString); 
		return resultString;
	}
}
