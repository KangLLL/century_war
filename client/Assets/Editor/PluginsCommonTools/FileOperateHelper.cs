using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileOperateHelper
{
	public static List<string> GetFiles(string folderPath, List<string> validExtensions)
	{
		string[] fileNames = Directory.GetFiles(folderPath);
		List<string> fileNameList = new List<string>();
		
		foreach (string fileName in fileNames) 
		{
			if(validExtensions.Contains(System.IO.Path.GetExtension(fileName)))
				fileNameList.Add(System.IO.Path.GetFileName(fileName));
		}
		return fileNameList;
	}
	
	public static void CopyFiles(List<string> files,string fileFolderPath,string projectGroupPath)
	{
		foreach (string file in files) 
		{
			string sourceFilePath = System.IO.Path.Combine(fileFolderPath, file);
			string destinationFilePath = System.IO.Path.Combine(projectGroupPath, file);
			File.Copy(sourceFilePath,destinationFilePath,true);
		}	
	}
	
	public static string GetRelativePathFromAbsolutePath(string basePath, string currentPath)
	{
		string[] baseParts = basePath.Split(new char[]{'/'});
		string[] currentParts = currentPath.Split(new char[]{'/'});
		
		int samePart = 1;
		for(int i = 1; i < baseParts.Length; i ++)
		{
			if(baseParts[i].Equals(currentParts[i]))
			{
				samePart ++;
			}
			else
			{
				break;
			}
		}
		
		string result = "";
		
		for(int i = samePart; i < baseParts.Length; i ++)
		{
			result += "../";
		}
		for(int i = samePart; i < currentParts.Length; i ++)
		{
			result += currentParts[i] + "/";
		}
		
		result = result.TrimEnd('/');
		if(result.Contains("+"))
		{
			result = "\"" + result + "\"";
		}
		return result;
	}
	
	public static void CopyFolder(string folderPath, string projectDestFolderPath)
	{
		
		if(Directory.Exists(folderPath))
		{
			if(!Directory.Exists(projectDestFolderPath))
			{
				Directory.CreateDirectory(projectDestFolderPath);
			}
			string[] files = Directory.GetFiles(folderPath);
			foreach(string filePath in files)
			{
				if(System.IO.Path.GetExtension(filePath).Equals(".h") || 
					System.IO.Path.GetExtension(filePath).Equals(".m") || 
					System.IO.Path.GetExtension(filePath).Equals(".a") ||
					System.IO.Path.GetExtension(filePath).Equals(".mm"))
				{
					string fileName = System.IO.Path.GetFileName(filePath);
					string sourceFilePath = System.IO.Path.Combine(folderPath, fileName);
					string destionationFilePath = System.IO.Path.Combine(projectDestFolderPath, fileName);
					File.Copy(sourceFilePath, destionationFilePath, true);
				}
			}
			
			string[] subFolders = Directory.GetDirectories(folderPath);
			foreach(string folderName in subFolders)
			{
				string sourceFolderPath = System.IO.Path.Combine(folderPath, folderName);
				string destFolderPath = System.IO.Path.Combine(projectDestFolderPath, folderName);
				CopyFolder(sourceFolderPath, destFolderPath);
			}
		}
	}
}
