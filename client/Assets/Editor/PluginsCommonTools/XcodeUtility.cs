using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XcodeUtility  
{
	public static List<ProjectItemInformation> GetItemsFromDirectory(string directoryPath, string parentGroupName, bool isRoot)
	{
		string[] files = System.IO.Directory.GetFiles(directoryPath);
		
		List<ProjectItemInformation> fileInfos = new List<ProjectItemInformation>();
		foreach (string file in files) 
		{
			string extension = System.IO.Path.GetExtension(file);
			if(!extension.Equals(".meta") && !extension.Equals(".DS_Store"))
			{
				if(extension.Equals(".a") || extension.Equals(".h") || extension.Equals(".m") || extension.Equals(".mm"))
				{
					SourceFileInformation sourceInfo = new SourceFileInformation();
					sourceInfo.FileName = System.IO.Path.GetFileName(file);
					sourceInfo.FilePath = file;
					sourceInfo.ParentGroupName = parentGroupName;
					fileInfos.Add(sourceInfo);
				}
			}
		}
		
		string[] directories = System.IO.Directory.GetDirectories(directoryPath);
		
		List<ProjectItemInformation> directoryInfos = new List<ProjectItemInformation>();
		foreach (string directory in directories) 
		{
			string extension = System.IO.Path.GetExtension(directory);
			if(!extension.Equals(".meta") && !extension.Equals(".DS_Store"))
			{
				if(System.IO.Path.GetExtension(directory).Equals(".framework"))
				{	
					FrameworkFileInformation frameworkInfo = new FrameworkFileInformation();
					frameworkInfo.FileName = System.IO.Path.GetFileName(directory);
					
					Debug.Log(frameworkInfo.FileName);
					
					frameworkInfo.FilePath = directory;
					frameworkInfo.FrameworkType = FrameworkType.Custom;
					frameworkInfo.ParentGroupName = parentGroupName;
					directoryInfos.Add(frameworkInfo);
				}
				else if(System.IO.Path.GetExtension(directory).Equals(".bundle"))
				{
					ResourceFileInformation resourceInfo = new ResourceFileInformation();
					
					resourceInfo.FileName = System.IO.Path.GetFileName(directory);
					resourceInfo.FilePath = directory;
					resourceInfo.ParentGroupName = parentGroupName;
					directoryInfos.Add(resourceInfo);
				}
				else
				{
					ProjectGroupInformation groupInfo = new ProjectGroupInformation();
					groupInfo.GroupName = System.IO.Path.GetFileName(directory);
					if(isRoot)
					{
						groupInfo.GroupPath = directory;
						groupInfo.IsRoot  = true;
					}
					else
					{
						groupInfo.GroupPath = groupInfo.GroupName;
						groupInfo.IsRoot = false;
					}
					groupInfo.ParentGroupName = parentGroupName;
					directoryInfos.Add(groupInfo);
					directoryInfos.AddRange(GetItemsFromDirectory(directory, groupInfo.GroupName, false));
				}
			}
		}
		
		List<ProjectItemInformation> result = new List<ProjectItemInformation>();
		result.AddRange(fileInfos);
		result.AddRange(directoryInfos);
		return result;
	}
}
