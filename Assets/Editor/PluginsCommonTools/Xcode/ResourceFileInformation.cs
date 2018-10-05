using UnityEngine;
using System.Collections;

public class ResourceFileInformation : ProjectFileInformation 
{
	public override void Initialize ()
	{
		this.FileKnownType = "wrapper.plug-in";
		this.SourceTree =  "\"<group>\"";
		this.Phase = new ProjectPhaseInformation();
		this.Phase.PhaseType = ProjectPhaseType.Resource;
		
		this.IsNeedAddNameToReference = true;
	}
}
