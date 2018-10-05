using UnityEngine;
using System.Collections;
using System.IO;

public class SaveBehavior : MonoBehaviour 
{
	void OnClick()
	{
		MapWriter.Instance.SaveMap();
		
		/*
		//bool isExists = System.IO.File.Exists("\\\\192.168.1.80\\Share\\123.txt");
		//Debug.Log(isExists);
		
		 FileStream fs = new FileStream("/192.168.1.80/Share/123.txt",FileMode.Open);
            StreamReader sr = new StreamReader(fs);
          string a = sr.ReadToEnd();
          Debug.Log(a);
		
		
       	//FileStream fs = new FileStream("\\\\192.168.1.80\\Share\\223.txt",FileMode.CreateNew); 
        //StreamWriter sw = new StreamWriter(fs);
        //sw.Write("write the map!");
        //sw.Close();
		
		//Debug.Log("Over!");
		*/
	}
}
