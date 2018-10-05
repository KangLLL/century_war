using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectPhaseInformation  
{
	private static Dictionary<ProjectPhaseType, string> phaseKeyDict = new Dictionary<ProjectPhaseType, string>()
	{
		{ProjectPhaseType.Source, "Sources"},
		{ProjectPhaseType.Framework, "Frameworks"},
		{ProjectPhaseType.Resource, "Resources"}
	};
	
	private string m_PhaseName;
	private ProjectPhaseType m_PhaseType;
	
	public ProjectPhaseType PhaseType 
	{ 
		get
		{
			return this.m_PhaseType;
		}
		set
		{
			this.m_PhaseType = value;
			this.m_PhaseName = phaseKeyDict[this.m_PhaseType];
		}
	}
	public string PhaseName { get { return this.m_PhaseName; } }
}
