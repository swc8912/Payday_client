using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour {
    public string textFileName;
    public float sec;
    public Text textContents;
    private TextAsset textAsset;
    private bool standby;
    private string s;
    private int line;
    private int textCnt = 0;
    private int maxTextCnt;
    private string memoryScript;

    public Button nextBtn;
    public GameManager gm;

	// Use this for initialization
	void Start () {
        line = 0;
        standby = true;
        textAsset = Resources.Load<TextAsset>("text/" + textFileName);
        s = textAsset.text;
        maxTextCnt = s.Length;
        Debug.Log("text init ok s: " + s.ToString());

        PrintScript();
	}

    public void onClick()
    {
        Debug.Log("max: " + maxTextCnt + " text: " + textCnt);
        if(textCnt >= maxTextCnt)
        {
            if(standby)
                // 메인 화면 오브젝트들 세팅 게임매니저에서 처리
                gm.InitScene();
        }
        else
        {
            PrintScript();
        }
    }

    public void PrintScript()
    {
        if (standby)
            StartCoroutine("printScript");
    }

    public void PrintScript(string script)
    {
        if (standby)
        {
            memoryScript = script;
            StartCoroutine("printScript2");
        }
    }

    IEnumerator printScript()
    {
        Debug.Log("printScript");
        standby = false;

        int cnt = 0;
        char[] c = division(s);
        //Debug.Log("c len: " + c.Length);
        textCnt += c.Length;
        for (cnt = 0; cnt < c.Length; cnt++)
        {
            textContents.text += c[cnt].ToString();
            //Debug.Log("text: " + textContents.text);
            yield return new WaitForSeconds(sec);
        }
        standby = true;
    }

    char[] division(string script)
    {
        Debug.Log("script: " + script);
        char[] c = script.ToCharArray();
        script = string.Empty;
        char[] sct = string.Empty.ToCharArray();
        //Debug.Log("  c len: " + c.Length);
        for (int i = line; i < c.Length; i++)
        {
            script = string.Format(script + c[i].ToString());
            //Debug.Log("c[i]: " + c[i]);
            if (c[i] == '\n')
            {
                line = i + 1;
                sct = script.ToCharArray();
                break;
            }
        }
        //Debug.Log("division sct script: " + script);
        return sct;
    }

    IEnumerator printScript2()
    {
        Debug.Log("printScript2");
        standby = false;

        int cnt = 0;
        char[] c = memoryScript.ToCharArray();
        //Debug.Log("c len: " + c.Length);
        textCnt += c.Length;
        for (cnt = 0; cnt < c.Length; cnt++)
        {
            textContents.text += c[cnt].ToString();
            //Debug.Log("text: " + textContents.text);
            yield return new WaitForSeconds(sec);
            //yield return null;
        }
        standby = true;
    }
}
