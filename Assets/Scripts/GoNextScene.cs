using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class GoNextScene : MonoBehaviour {

    public void onClick()
    {
        Debug.Log("onclick");
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
        FacebookUnity fbu = new FacebookUnity();
        fbu.initFB();
#endif
#if UNITY_EDITOR
        SceneManager.LoadScene("IntroScene");
#endif
    }
}
