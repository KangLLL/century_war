using UnityEngine;
using System.Collections;

public class ResolutionAdaptive : MonoBehaviour
{

    [SerializeField]
    ScreenResolution m_ScreenResolution;
    void Awake()
    {
        ClientSystemConstants.SCREENRESOLUTION = this.m_ScreenResolution;


#if UNITY_IPHONE

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
           
            //if (iPhone.generation == iPhoneGeneration.iPad1Gen ||
            //    iPhone.generation == iPhoneGeneration.iPad2Gen ||
            //    iPhone.generation == iPhoneGeneration.iPad3Gen ||
            //    iPhone.generation == iPhoneGeneration.iPad4Gen ||
            //    iPhone.generation == iPhoneGeneration.iPadMini1Gen)
            //{
            //    ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size1024X768;
            //}
            //else
            //    if (iPhone.generation == iPhoneGeneration.iPhone5 || iPhone.generation == iPhoneGeneration.iPodTouch5Gen)
            //    {
            //        ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size1136X640;
            //    }
            //    else
            //    {
            //        ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size960X640;
            //    }

            switch (Screen.currentResolution.width)
            {
                case 960:
                    ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size960X640;
                    break;
                case 1024:
                    ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size1024X768;
                    break;
                case 1136:
                    ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size1136X640;
                    break;
                case 2048:
                    ClientSystemConstants.SCREENRESOLUTION = ScreenResolution.Size1024X768;
                    break;
            }	
    	}
#endif
	}
}


public enum ScreenResolution
{
    Size960X640,
    Size1024X768,
    Size1136X640
}