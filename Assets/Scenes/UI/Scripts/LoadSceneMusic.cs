using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class SceneMusic : MonoBehaviour {
    private static SceneMusic s_Sigleton;
    public static SceneMusic Instance
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
