using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

using CommonUtilities;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class BattleInitialize : MonoBehaviour 
{
	private static BattleInitialize s_Sigleton;
	
	public static BattleInitialize Instance
	{
		get { return s_Sigleton; }
	}
	
	[SerializeField]
	private ObstacleFactory m_Factory;
	[SerializeField]
	private LoadAgeMap m_Map;
	[SerializeField]
	private GridFactory m_GridFactory;
	[SerializeField]
	private Age m_CurrentAge;
	
	private bool m_Start;
	
	public Age CurrentRivalAge { get { return this.m_CurrentAge; } }
	
	private const string CONFIG_PATH = "ConfigTable.zip";
	
	void Awake()
	{
		s_Sigleton = this;
		
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + CONFIG_PATH, FileMode.Open);
		MemoryStream uncompressedStream = new MemoryStream();
		CompressionUtility.DecompressStream(fileStream, uncompressedStream);
		
		BinaryFormatter bft = new BinaryFormatter();
		DataResource.Resource = (DataSet)bft.Deserialize(uncompressedStream);
		
		fileStream.Close();
		uncompressedStream.Close();
	}
	
	void Start()
	{
		Hashtable map = MapReader.Instance.LoadMap();
		BattleSceneHelper.Instance.ClearObject();
		int battleRandomSeed = 0;
		BattleRandomer.Instance.SetSeed(battleRandomSeed);

		if(map != null)
		{
			this.m_Factory.ConstructBuilding(DataConvertor.ConvertJSONToParameter(map));
			this.m_Map.SetMap(this.CurrentRivalAge);
			
			BattleMapData.Instance.ConstructGridArray();
			this.m_GridFactory.ConstructGird();
		}
	
		//AudioController.PlayMusic("BattleStart");
		BattleSceneHelper.Instance.EnableBuildingAI();
		BattleRecorder.Instance.BattleStartTime = TimeTickRecorder.Instance.CurrentTimeTick;
	}
	
	void Update()
	{
		if(!this.m_Start)
		{
			
			
			this.m_Start = true;
		}
	}
}
