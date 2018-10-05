using UnityEngine;
using System.Collections;

public class UISearchFight : MonoBehaviour {
    void OnClick()
    {
        LockScreen.Instance.DisableInput();
        UIManager.Instance.CloudBehaviour.FadeIn();
        StartCoroutine("LoadLevelCoroutine");
    }

    IEnumerator LoadLevelCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(ClientStringConstants.BATTLE_SCENE_LEVEL_NAME);
    }
}
