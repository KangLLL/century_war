using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class NDPluginPostBuildPlayer  
{
	private const string CLASS_FOLDER_NAME = "Classes";
	private const string APP_CONTROLLER_FILE_NAME = "AppController.mm";
	
	private const string ND_FRAMEWORK_RELATIVE_PATH = "Plugins/iOSPlatform/NdPlatform/Framework";
	private const string ND_RESOURCE_BUNDLE_RELATIVE_PATH = "Plugins/iOSPlatform/NdPlatform/Resource";
	private const string ND_NATIVE_SOURCE_FOLDER_PATH = "Plugins/iOSPlatform/NdPlatform/NativeCode";
	
	[UnityEditor.Callbacks.PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
	{
		if(target == BuildTarget.iPhone)
		{
			Debug.Log("1");
			XcodeModifyHelper.ModifyBuildSetting(pathToBuiltProject, ProjectSettingType.OtherLinkFlag, "\"-ObjC\"",ModifyType.Replace);
			Debug.Log("2");
			List<ProjectItemInformation> infos = new List<ProjectItemInformation>();
			infos.AddRange(GetDependentFrameworks());
			infos.AddRange(GetNdFrameworks(pathToBuiltProject));
			infos.AddRange(GetNdResource(pathToBuiltProject));
			infos.AddRange(GetNativeCode(pathToBuiltProject));
			
			XcodeModifyHelper.ModifyXcodeProject(infos, pathToBuiltProject);
			Debug.Log("3");
		}
		
    }
	
	private static List<ProjectItemInformation> GetDependentFrameworks()
	{
		List<ProjectItemInformation> result = new List<ProjectItemInformation>()
		{
			new FrameworkFileInformation() { FrameworkType = FrameworkType.AddressBook, FileType = ProjectFileType.Framework },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.CoreGraphics, FileType = ProjectFileType.Framework },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.CoreTelephony, FileType = ProjectFileType.Framework, IsWeak = true },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.QuartzCore, FileType = ProjectFileType.Framework },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.SystemConfiguration, FileType = ProjectFileType.Framework },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.UIKit, FileType = ProjectFileType.Framework, IsWeak = true },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.Foundation, FileType = ProjectFileType.Framework },
			new FrameworkFileInformation() { FrameworkType = FrameworkType.MessageUI, FileType = ProjectFileType.Framework },
			new DynamicLibraryFileInformation() { DynamicLibraryType = DynamicLibraryType.Sqlite3, FileType = ProjectFileType.DynamicLibrary },
			new DynamicLibraryFileInformation() { DynamicLibraryType = DynamicLibraryType.Libz, FileType = ProjectFileType.DynamicLibrary}
		};
		return result;
	}
	
	private static List<ProjectItemInformation> GetNdFrameworks(string projectPath)
	{
		
		string frameworkPath = System.IO.Path.Combine(Application.dataPath, ND_FRAMEWORK_RELATIVE_PATH);
		
		string destinationPath = System.IO.Path.Combine(projectPath,ND_FRAMEWORK_RELATIVE_PATH);
		FileOperateHelper.CopyFolder(frameworkPath,destinationPath); 
		
		string parentGroup = "Frameworks";
		
		return XcodeUtility.GetItemsFromDirectory(destinationPath, parentGroup, true);
	}
	
	public static List<ProjectItemInformation> GetNdResource(string projectPath)
	{
		string resourcePath = System.IO.Path.Combine(Application.dataPath, ND_RESOURCE_BUNDLE_RELATIVE_PATH);
		
		string destinationPath = System.IO.Path.Combine(projectPath,ND_RESOURCE_BUNDLE_RELATIVE_PATH);
		FileOperateHelper.CopyFolder(resourcePath, destinationPath); 
		
		string parentGroup = "Resources";
		
		return XcodeUtility.GetItemsFromDirectory(destinationPath, parentGroup, true);
	}
	
	private static List<ProjectItemInformation> GetNativeCode(string projectPath)
	{
		string nativePath = System.IO.Path.Combine(Application.dataPath, ND_NATIVE_SOURCE_FOLDER_PATH);
		
		string destinationPath = System.IO.Path.Combine(projectPath,ND_NATIVE_SOURCE_FOLDER_PATH);
		FileOperateHelper.CopyFolder(nativePath,destinationPath); 
		
		string parentGroup = "Classes";
		
		return XcodeUtility.GetItemsFromDirectory(destinationPath, parentGroup, true);
	}
}
