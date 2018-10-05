
public static class UnityXConfig
{
    static UnityXConfig ()
	{
#if UNITY_IPHONE
        if (UnityEngine.Screen.currentResolution.width == 480
            || UnityEngine.Screen.currentResolution.width == 320)
        {
            s_IsRetina = false;
        }
        else
        {
            s_IsRetina = true;
        }
#endif
    }


    public const float ScreenPixelRatio = 1.0f;
    public const int ScreenWidth = 640;
    public const int ScreenHeight = 960;
    private static bool s_IsRetina;

#if UNITY_IPHONE
    public static bool IsRetina
    {
        get
        {
            return s_IsRetina;
        }
    }
#endif

}
