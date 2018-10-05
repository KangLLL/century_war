using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using CommonUtilities;
using ConfigUtilities;

public class EditorReplayReader : ReplayDataReader 
{
	[SerializeField]
	private string m_MapName;
	[SerializeField]
	private string m_BattleName;
	
	private const string CONFIG_PATH = "ConfigTable.zip";
	
	void Awake()
	{
		
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + CONFIG_PATH, FileMode.Open);
		MemoryStream uncompressedStream = new MemoryStream();
		CompressionUtility.DecompressStream(fileStream, uncompressedStream);
		
		BinaryFormatter bft = new BinaryFormatter();
		DataResource.Resource = (DataSet)bft.Deserialize(uncompressedStream);
		
		fileStream.Close();
		uncompressedStream.Close();
	}
	
	protected override string GetMapInformation ()
	{
		string filePath = Application.platform == RuntimePlatform.OSXEditor ? 
			this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix :
			EditorConfigInterface.Instance.MapStorePath + "/" +
			this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix;
		
		FileStream fs =  new FileStream(filePath,FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		string result = sr.ReadToEnd();
		sr.Close();
		return result;
	}
	
	protected override string GetBattleInformation ()
	{
		FileStream fs =  Application.platform == RuntimePlatform.OSXEditor ? 
			new FileStream(this.m_BattleName + "."  + EditorConfigInterface.Instance.MapSuffix,FileMode.Open) :
			new FileStream(EditorConfigInterface.Instance.MapStorePath + "/" +
			this.m_BattleName + "."  + EditorConfigInterface.Instance.MapSuffix, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		
		string result = sr.ReadToEnd();
		sr.Close();
		return result;
	}
}
