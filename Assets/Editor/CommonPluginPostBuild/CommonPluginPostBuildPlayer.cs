using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class CommonPluginPostBuildPlayer 
{
	private const string CLASS_FOLDER_NAME = "Classes";
	private const string APP_CONTROLLER_FILE_NAME = "UnityAppController.mm";
	
	private const string PLUG_IN_FOLDER_RELATIVE_PATH = "Plugins/iOSPlatform/NativeCode";
	private const string CLASS_GROUP_RELEATIVE_PATH = "Classes";
	
	[UnityEditor.Callbacks.PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
	{
		if(target == BuildTarget.iPhone)
		{
			string plugInFolderPath = System.IO.Path.Combine(Application.dataPath, PLUG_IN_FOLDER_RELATIVE_PATH);
			string destinationPath = System.IO.Path.Combine(pathToBuiltProject, PLUG_IN_FOLDER_RELATIVE_PATH);
			
			List<ProjectItemInformation> items = new List<ProjectItemInformation>();
			
			FileOperateHelper.CopyFolder(plugInFolderPath, destinationPath);
			
			items.AddRange(XcodeUtility.GetItemsFromDirectory(destinationPath, CLASS_FOLDER_NAME , true));
			items.Add (new FrameworkFileInformation() { FrameworkType = FrameworkType.StoreKit, FileType = ProjectFileType.Framework });
			XcodeModifyHelper.ModifyXcodeProject(items , pathToBuiltProject);
			XcodeModifyHelper.AddEntityToInfoPlist(pathToBuiltProject, "UIViewControllerBasedStatusBarAppearance", false);
			AddStoreHelperCode(pathToBuiltProject);

			XcodeModifyHelper.AddEntityToInfoPlist(pathToBuiltProject, "CFBundleDevelopmentRegion", "zh_CN");
			//AddPlugInFiles(xcodeProjectClassGroupPath, pathToBuiltProject);
			//AddStoreKitFramework(pathToBuiltProject);
			//AddStoreHelperCode(pathToBuiltProject);
		}
		
    }
	
	private static List<ProjectFileInformation> GetPlugInFilesInformation(string projectGroupPath, string projectFilePath)
	{
		string plugInFolderPath = System.IO.Path.Combine(Application.dataPath, PLUG_IN_FOLDER_RELATIVE_PATH);
		List<string> plugInFiles = FileOperateHelper.GetFiles(plugInFolderPath, new List<string>(){".h",".m"});
		FileOperateHelper.CopyFiles(plugInFiles, plugInFolderPath, projectGroupPath);
		
		List<ProjectFileInformation> files = new List<ProjectFileInformation>();
		foreach (string filePath in plugInFiles) 
		{
			ProjectFileInformation information = new ProjectFileInformation();
			information.FileName =  System.IO.Path.GetFileName(filePath);
			information.FilePath = filePath;
			information.FileType = ProjectFileType.Source;
		}
		return files;
		//XcodeModifyHelper.ModifyXcodeProject(plugInFiles, projectFilePath, ProjectFileType.Source);
	}
	
	private static ProjectFileInformation GetStoreKitFrameworkFileInformation(string projectFilePath)
	{
		FrameworkFileInformation result = new FrameworkFileInformation();
	    result.FrameworkType = FrameworkType.StoreKit;
		result.FileType = ProjectFileType.Framework;
		return result;
		
		//XcodeModifyHelper.ModifyXcodeProject(new List<string>(){"StoreKit.framework"},projectFilePath, ProjectFileType.Framework);
	}
	
	private static void AddStoreHelperCode(string pathToBuiltProject)
	{
		string classesFolderPath = System.IO.Path.Combine(pathToBuiltProject, CLASS_FOLDER_NAME);
		string appControllerFilePath = System.IO.Path.Combine(classesFolderPath, APP_CONTROLLER_FILE_NAME);
		
		FileStream fs = File.Open(appControllerFilePath, FileMode.Open,FileAccess.Read);
		StreamReader sr = new StreamReader(fs);
		
		string content = sr.ReadToEnd();
		sr.Close();
		string pattern = "- \\(BOOL\\)application\\:\\(UIApplication\\*\\)application didFinishLaunchingWithOptions\\:\\(NSDictionary\\*\\)launchOptions\n{((?:.|\n)+?)\n}";
		
		Match m = Regex.Match(content, pattern);
	
		if(m.Success)
		{
			pattern = "\\[StoreKitHelper sharedHelper";
			Match match = Regex.Match(content, pattern);
			if(!match.Success)
			{
				string codeString = "\n\t[StoreKitHelper sharedHelper];\n\n\tapplication.applicationIconBadgeNumber = 0;\n";
				string newContent = content.Substring(0,m.Groups[1].Index) + codeString + content.Substring(m.Groups[1].Index);
				
				string includeString = "#import \"StoreKitHelper.h\"";
				newContent = string.Format("{0}\n{1}",includeString, newContent);
				
				fs = File.Open(appControllerFilePath, FileMode.OpenOrCreate,FileAccess.Write);
				StreamWriter sw = new StreamWriter(fs);
				sw.Write(newContent);
				sw.Close();
			}
		}
		else
		{
			Debug.Log("there is no application did finish launching with options!");
		}
	}
}
