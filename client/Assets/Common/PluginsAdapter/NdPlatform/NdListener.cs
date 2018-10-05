using UnityEngine;
using System.Collections;
using CommonUtilities;

public class NdListener : MonoBehaviour
{
    public void PlatformInitialSuccess(string result)
    {
        if (result != null)
        {
            Debug.Log(result);
        }
        GameObject.Find(ClientStringConstants.ND_START_OBJECT_NAME).GetComponent<NdStart>().Initialize();
    }

    public void NdPlayerLoginFinished(string result)
    {
        if (string.IsNullOrEmpty(result))
        {
            NdCenter.Instace.LoginSuccess();
        }
        else
        {
            NdCenter.Instace.LoginFail();
        }
    }

    public void NdBuyFinished(string result)
    {
        Debug.Log(result);
        Hashtable hash = (Hashtable)JsonUtility.jsonDecode(result);

        Debug.Log(hash.Count);
        if ((bool)hash["result"])
        {
            NdCenter.Instace.BuySuccess();
        }
        else
        {
            NdCenter.Instace.BuyFail(ClientStringConstants.PURCHASE_FAIL_TIPS);
            //hash["error"].ToString());
        }
    }

    public void NdPlatformLeaved(string result)
    {
        NdCenter.Instace.LeavePlatform();
    }
}
