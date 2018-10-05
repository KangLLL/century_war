using UnityEngine;
using System.Collections;
using System.IO;

public class MapReader : MonoBehaviour 
{
	private static MapReader s_Sigleton;
	
	public static MapReader Instance 
	{
		get { return s_Sigleton; }
	}
	
	[SerializeField]
	private string m_MapName;
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	public Hashtable LoadMap()
	{
		if(string.IsNullOrEmpty(this.m_MapName))
		{
			return null;
		}
		else
		{
			string filePath = Application.platform == RuntimePlatform.OSXEditor ? 
				this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix :
				EditorConfigInterface.Instance.MapStorePath + "/" +
				this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix;
			
			if(!System.IO.File.Exists(filePath))
			{
				return null;
			}
			else
			{
				FileStream fs =  new FileStream(filePath,FileMode.Open);
				StreamReader sr = new StreamReader(fs);
				
				Hashtable result = (Hashtable)JSONHelper.jsonDecode(sr.ReadToEnd());
				sr.Close();
				return result;
			}
		}
	}
}
