using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class GoNextScene : MonoBehaviour {

    public void onClick()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
        FacebookUnity.initFB();
        Debug.Log("initfb");
#endif
#if UNITY_EDITOR
        SceneManager.LoadScene("IntroScene");
#endif
    }
}
