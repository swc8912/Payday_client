using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoNextScene : MonoBehaviour {

    public void onClick()
    {
        Debug.Log("onclick");
        SceneManager.LoadScene("IntroScene");
    }
}
