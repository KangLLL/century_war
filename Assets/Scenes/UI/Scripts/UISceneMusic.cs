using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class LoadSceneMusic : MonoBehaviour {
    private static LoadSceneMusic s_Sigleton;
    public static LoadSceneMusic Instance
    {
        get { return s_Sigleton; }
    }
    void Awake()
    {
        s_Sigleton = this;
    }
    public void SetSceneMusic(Age age)
    {
        AudioController.PlayMusic(ClientSystemConstants.AGE_SCENE_MUSIC[age]);
    }
}
