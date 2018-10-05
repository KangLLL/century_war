using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrameworkFileInformation : ProjectFileInformation 
{
	public FrameworkType FrameworkType { get;set; }
	public bool IsWeak { get;set; }
	
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
	
	public override void Initialize ()
	{
		if(this.FrameworkType != FrameworkType.Custom)
		{
			this.FileName = frameworkKeyDict[this.FrameworkType];
		}
		if(string.IsNullOrEmpty(this.FilePath))
		{
			this.FilePath = "System/Library/Frameworks/" + frameworkKeyDict[this.FrameworkType];
		}
		this.FileKnownType = "wrapper.framework";
		this.Phase = new ProjectPhaseInformation();
		this.Phase.PhaseType = ProjectPhaseType.Framework;
		if(this.FrameworkType != FrameworkType.Custom)
		{
			this.ParentGroupName = "CustomTemplate";
			this.SourceTree = "SDKROOT";
		}
		else
		{
			 this.SourceTree = "\"<group>\"";
		}
		this.IsNeedAddNameToReference = true;
	}
}
