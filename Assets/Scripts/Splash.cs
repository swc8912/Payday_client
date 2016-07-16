using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Splash : MonoBehaviour {
    public Text tapText;
    private bool flag;

	// Use this for initialization
	void Start () {
        flag = true;
        StartCoroutine("btnspotlight");
	}

    IEnumerator btnspotlight()
    {
        if (flag)
        {
            tapText.color = new Color(0, 0, 0, 255);
            flag = false;
        }
        else
        {
            tapText.color = new Color(0, 0, 0, 0);
            flag = true;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("btnspotlight");
    }
}
