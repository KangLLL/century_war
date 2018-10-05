using UnityEngine;
using System.Collections;

public class UIReturnSceneBuild : MonoBehaviour {

    void OnClick()
    {
        LockScreen.Instance.DisableInput();
        UIManager.Instance.CloudFadeIn();
        StartCoroutine("DelayCloudFadeIn");
    }

    IEnumerator DelayCloudFadeIn()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
    }
}
