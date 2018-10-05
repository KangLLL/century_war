using UnityEngine;
using System.Collections;
using System.IO;

public class Test : ReusableDelegate
{
    [SerializeField]
    ReusableScrollView m_ReusableScrollView;
	
    void Start()
    {
        //this.m_ReusableScrollView.ReloadData();
		//string filePath = "////192.168.1.80/Share/EditorMaps/cccc";
		
		//Directory.CreateDirectory(filePath);
		
		
		//FileStream fs = new FileStream(filePath, FileMode.Open);
		//StreamReader sr = new StreamReader(fs);
		//Debug.Log(sr.ReadToEnd());
		//sr.Close();
		
    }
    public override void InitialCell(int index, GameObject cell)
    {
        //UIEmailItem uiEmailItem = cell.GetComponent<UIEmailItem>();
        //uiEmailItem.SetItemData(this.m_LogData[index], m_LogType); 
        cell.GetComponent<Test2>().SetItemData(index.ToString());
    }
    public override int TotalNumberOfCells
    {
        get { return 103;/*this.m_LogData.Length;*/ } 
    }
}
