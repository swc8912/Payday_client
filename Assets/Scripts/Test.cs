using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
    private UserData userData;
    private const int MAXREGENHEART = 3;
    private const int MAXCHARGETIME = 10;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Button btn5;
    public Button btn6;
    private string userUrl = "http://52.193.33.78:3000/payday";
    enum LoadDataNum
    {
        item = 1, // 아이템 리스트
        user, // 유저 데이터
        log // 로그 데이터 전송
    };
    LoadDataNum loadData;

	// Use this for initialization
	void Start () {
        Debug.Log("test start");
        userData = new UserData();
        userData.did = "asdfsdaf";
#if UNITY_IOS || UNITY_ANDROID
        // 기본 데이터 설정
        //userData.did = SystemInfo.deviceUniqueIdentifier;
        //Debug.Log("did: " + SystemInfo.deviceUniqueIdentifier);
#endif
        userData.email = "init@celes.kr";
        userData.heart = MAXREGENHEART;
        userData.money = 0;
        userData.rank = "인턴";
        userData.charge = MAXCHARGETIME;
        userData.currentBoxId = "1";
        userData.getPush = true;
        userData.pickItems = new ArrayList();
	}
	
	// Update is called once per frame
	public void Update () {
	
	}

    public void InsertNewUser()
    {
        Debug.Log("insertnewuser");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("did", userData.did);
        data.Add("email", userData.email);
        data.Add("money", "0");
        data.Add("rank", "인턴");
        data.Add("heart", "3");
        data.Add("charge", "10");
        data.Add("currentBoxId", "1");
        //string itemStr = "[{'description' : '4만원이다.','index' : 10001,'rangeStart' : 1,'rangeEnd' : 4000,'text' : '4만원','type' : 1,'value' : 4}]";
        string itemStr = "[]";
        data.Add("pickItems", itemStr);
        data.Add("getPush", "true");
        helper.post(2, userUrl, data);
    }

    public void UpdateUser()
    {
        // put으로 해야함
        Debug.Log("UpdateUser");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("money", "22540");
        data.Add("rank", "이사");
        data.Add("heart", "5");
        data.Add("charge", "10");
        data.Add("currentBoxId", "6");
        string itemStr = "[{'description' : '4만원이다.','index' : 10001,'rangeStart' : 1,'rangeEnd' : 4000,'text' : '4만원','type' : 1,'value' : 4}]";
        //string itemStr = "[]";
        data.Add("pickItems", itemStr);
        data.Add("getPush", "true");
        helper.post(2, userUrl + "/?email=" + userData.email, data);
    }

    public void GetUser()
    {
        Debug.Log("GetUser");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        helper.get(2, userUrl + "/?email=" + userData.email);
    }

    public void GetBoxData()
    {
        Debug.Log("GetBoxData");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        helper.get(1, userUrl + "/?box=true");
    }

    public void InsertTimeLog()
    {
        Debug.Log("InsertTimeLog");
    }

    public void InsertGetItemLog()
    {
        Debug.Log("InsertGetItemLog");
    }

    public void DeleteUser()
    {
        Debug.Log("DeleteUser");
    }

    void OnHttpRequest(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
            return;
        }
        else
        {
            Debug.Log("onhttprequest res: " + www.text);
        }

        JsonData json = JsonMapper.ToObject(www.text);
        if (id == (int)LoadDataNum.item)
        {
           
        }
        else if (id == (int)LoadDataNum.user)
        {
            Debug.Log("user aaa");
            try
            {
                if (json["results"].Count == 0)
                {
                    Debug.Log("email get go");
                    JsonData items = json["results"];
                }
                else
                {
                    JsonData data = json["results"];
                    userData.money = Int32.Parse(data[0]["money"].ToString());
                    userData.rank = data[0]["rank"].ToString();
                    userData.heart = Int32.Parse(data[0]["heart"].ToString());
                    userData.charge = Int32.Parse(data[0]["charge"].ToString());
                    userData.currentBoxId = data[0]["currentBoxId"].ToString();
                    if (data[0]["getPush"].ToString().Equals("true"))
                        userData.getPush = true;
                    else if (data[0]["getPush"].ToString().Equals("false"))
                        userData.getPush = false;
                    JsonData items = JsonMapper.ToObject(data[0]["pickItems"].ToString());
                    int cnt = items.Count;
                    userData.pickItems.Clear();
                    for (int i = 0; i < cnt; i++)
                    {
                        userData.pickItems.Add(JsonMapper.ToObject<GiftItem>(items[i].ToJson()));
                    }
                    Debug.Log("get uesrdata end");
                }
            }
            catch (KeyNotFoundException e)
            {
                Debug.Log("keynotfoundexp");
            }
        }
    }
}
