using UnityEngine;
using System.Collections;

public class iOSStart : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Application.LoadLevel(ClientStringConstants.INITIAL_SCENE_LEVEL_NAME);
    }

}
 
