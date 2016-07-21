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
    //private string userUrl = "http://52.193.33.78:3000/payday";
    private string userUrl = "http://localhost:3000/payday";
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

        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
	}
	
    public void InsertNewUser()
    {
        Debug.Log("insertnewuser");
        WWWHelper helper = WWWHelper.Instance;
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
        // put
        Debug.Log("UpdateUser");
        WWWHelper helper = WWWHelper.Instance;
        string data = "";
        UserData ud = new UserData();
        ud.did = "asdfaaa";
        ud.email = "init@celes.kr";
        ud.money = 2554;
        ud.rank = "이사";
        ud.heart = 5;
        ud.charge = 10;
        ud.currentBoxId = "10";
        //string itemStr1 = "[{'description' : '4만원이다.','index' : 10001,'rangeStart' : 1,'rangeEnd' : 4000,'text' : '4만원','type' : 1,'value' : 4}]";
        //string itemStr2 = "{'description' : '4만원이다.','index' : 10001,'rangeStart' : 1,'rangeEnd' : 4000,'text' : '4만원','type' : 1,'value' : 4}";
        GiftItem item1 = new GiftItem();
        item1.description = "4만원이다.";
        item1.index = 10001;
        item1.rangeStart = 1;
        item1.rangeEnd = 4000;
        item1.text = "4만원";
        item1.type = 1;
        item1.value = 4;
        ud.pickItems = new ArrayList();
        ud.pickItems.Add(item1);
        GiftItem item2 = new GiftItem();
        item2.description = "10만원이다.";
        item2.index = 10002;
        item2.rangeStart = 4001;
        item2.rangeEnd = 8000;
        item2.text = "10만원";
        item2.type = 1;
        item2.value = 10;
        ud.pickItems.Add(item2);
        //JsonData jd = JsonMapper.ToObject(itemStr2);
        //for (int i = 0; i < jd.Count; i++)
          //  ud.pickItems.Add(JsonMapper.ToObject<GiftItem>(jd[i].ToString())); 
        //GiftItem gi = (GiftItem)ud.pickItems[0];
        //ud.pickItems = itemStr;
        ud.getPush = true;
        data = JsonMapper.ToJson(ud);
        Debug.Log("put data: " + data);
        /*IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("money", "22540");
        data.Add("rank", "이사");
        data.Add("heart", "5");
        data.Add("charge", "10");
        data.Add("currentBoxId", "6");
        //string itemStr = "[]";
        data.Add("pickItems", itemStr);
        data.Add("getPush", "true");*/
        helper.put(2, userUrl + "/?email=" + userData.email, data);
    }

    public void GetUser()
    {
        Debug.Log("GetUser");
        WWWHelper helper = WWWHelper.Instance;
        helper.get(2, userUrl + "/?email=" + userData.email);
    }

    public void GetBoxData()
    {
        Debug.Log("GetBoxData");
        WWWHelper helper = WWWHelper.Instance;
        helper.get(1, userUrl + "/?box=true");
    }

    enum LogCmd
    {
        firstIncome = 1,
        playGame,
        getItem,
        quitApp
    };
    LogCmd logCmd;

    public void InsertTimeLog()
    {
        Debug.Log("InsertTimeLog");
        WWWHelper helper = WWWHelper.Instance;
        LogData logData = new LogData();
        logData.email = userData.email;
        logData.logCmd = (int)LogCmd.firstIncome;
        logData.date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        Debug.Log("date: " + logData.date);
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("logCmd", "" + logData.logCmd);
        data.Add("email", logData.email);
        data.Add("date", logData.date);
        helper.post(3, userUrl, data);
    }

    public void InsertGetItemLog()
    {
        Debug.Log("InsertGetItemLog");
        GiftItem item = new GiftItem();
        item.description = "4만원이다.";
        item.index = 10001;
        item.rangeStart = 1;
        item.rangeEnd = 4000;
        item.text = "4만원";
        item.type = 1;
        item.value = 4;
        WWWHelper helper = WWWHelper.Instance;
        GetItemData itemData = new GetItemData();
        itemData.email = userData.email;
        itemData.itemIdx = item.index;
        itemData.date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        Debug.Log("date: " + itemData.date);
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("email", itemData.email);
        data.Add("itemIdx", "" + itemData.itemIdx);
        data.Add("date", itemData.date);
        //string itemStr = "[{'description' : '4만원이다.','index' : 10001,'rangeStart' : 1,'rangeEnd' : 4000,'text' : '4만원','type' : 1,'value' : 4}]";
        helper.post(3, userUrl, data);
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
            if (www.text.Length < 5000)
                Debug.Log("onhttprequest res: " + www.text);
            else
                Debug.Log("onhttprequest res too long");
        }

        JsonData json = JsonMapper.ToObject(www.text);
        if (id == (int)LoadDataNum.item) // 박스 데이터 정보 얻기
        {
            if (json["results"].Count == 0)
            {
                Debug.Log("box item data not found");
            }
            else
            {
                JsonData data = json["results"][0]["bdata"];
                //Debug.Log("data str: " + JsonMapper.ToJson(data).ToString());
                for (int i = 0; i < data.Count; i++)
                {
                    //Debug.Log("d: " + JsonMapper.ToJson(data[j]));
                    string jsonStr = JsonMapper.ToJson(data[i]);
                    if (GameManager.GiftList[i] == null)
                        GameManager.GiftList[i] = new ArrayList();
                    GameManager.GiftList[i].Add(JsonMapper.ToObject<BoxData>(jsonStr));
                }
                Debug.Log("box item data loaded");
            }
        }
        else if (id == (int)LoadDataNum.user)
        {
            Debug.Log("user aaa");
            try
            {
                if (json["results"].Count == 0) // 없는 경우 새로 추가
                {
                    Debug.Log("result count 0 insert new user");
                    InsertNewUser();
                }
                else // 유저 데이터가 있는 경우 적용
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
        else if (id == (int)LoadDataNum.log)
        {

        }
    }
}
