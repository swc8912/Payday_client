using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using UnityEngine.UI;
using System.Net;
using Facebook.Unity;

public class GameManager : MonoBehaviour {
    public static bool debug = true;
    public static ArrayList[] GiftList = new ArrayList[MAXBOXNUMBER];
    private string jsonUrl = "https://s3-ap-northeast-1.amazonaws.com/paydaybucket/data_utf8bom.json";
    private static string userUrl = "http://52.193.33.78:3000/payday";
    //private string userUrl = "http://localhost:3000/payday";
    private const int MAXCHARGETIME = 10;
    private const int MAXREGENHEART = 3;
    private const int MAXBOXNUMBER = 10;
    enum LoadDataNum {
        item = 1, // 아이템 리스트
        user, // 유저 데이터
        log // 로그 데이터 전송
    };
    LoadDataNum loadData;
    enum LogCmd
    {
        firstIncome = 1,
        playGame,
        getItem,
        quitApp
    };
    LogCmd logCmd;
    public static UserData userData = new UserData();

    public Button nextBtn;
    public Button msgBtn;
    public Button upgradeBtn;
    public Button settingBtn;
    public Button shareBtn;
    public Text msgText;
    public Text cashText;
    public Text rankText;
    public Text heartText;
    public Text timerText;
    public GameObject giftBox;

    public MsgBox msgBox;

    private SpriteRenderer spriteRenderer;
    public Sprite[] giftSprites;

    private DateTime nowTime;
    private DateTime quitTime;

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        spriteRenderer = giftBox.gameObject.GetComponent<SpriteRenderer>();
        // 인터넷 연결 체크
        if (Application.internetReachability != NetworkReachability.NotReachable)
            LoadData();
        else // 원래는 다이얼로그 띄우고 재시도 해야함
        {
            Application.Quit();
            return;
        }

        nowTime = DateTime.UtcNow;
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                // 앱 종료 로그 전송
                quitTime = DateTime.UtcNow;
                long playTime = (long)((quitTime - nowTime).Milliseconds) / 1000L;
                Debug.Log("playTime: " + playTime);
                InsertTimeLog((int)LogCmd.quitApp, 0);
                InsertTimeLog((int)LogCmd.playGame, playTime);
                Application.Quit();
            }
        }
	}

    public void InitScene()
    {
        Debug.Log("initscene");
        // 대화창 안보이게
        // 상자 이미지 보이게
        // 상자 열기 버튼 보이게
        // 직급 돈 보이게
        // 설정 강화 버튼 보이게
        // 하트 보이게
        msgBtn.gameObject.SetActive(false);
        msgText.text = "";
        nextBtn.gameObject.SetActive(true);
        cashText.gameObject.SetActive(true);
        rankText.gameObject.SetActive(true);
        giftBox.gameObject.SetActive(true);
        heartText.gameObject.SetActive(true);
        shareBtn.gameObject.SetActive(false);
        spriteRenderer.sprite = giftSprites[0];
        //upgradeBtn.gameObject.SetActive(true);
        //settingBtn.gameObject.SetActive(true);
    }

    public void LoadData()
    {
        Debug.Log("LoadData");
        userData.did = "initdivice";
#if UNITY_IOS || UNITY_ANDROID
        // 기본 데이터 설정
        userData.did = SystemInfo.deviceUniqueIdentifier;
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
#if UNITY_IOS || UNITY_ANDROID
        FacebookUnity.GetUserDataFB();
#endif
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        helper.OnHttpRequest2 += OnHttpRequest2;
        // 아이템 데이터 로딩
        GetBoxData();
    }

    private void SetVisibleUserdata()
    {
        rankText.text = "직급: " + userData.rank;
        //userData.money = 20240;
        string moneyStr = "현금: " + userData.money + "만원";
        if (userData.money >= 10000)
            moneyStr = "현금: " + userData.money / 10000 + "억 " + userData.money % 10000 + "만원";
        cashText.text = moneyStr;
        timerText.text = userData.charge.ToString();
        heartText.text = "X " + userData.heart;
        StartCoroutine("HeartTimer");
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
        string itemStr = "[]";
        data.Add("pickItems", itemStr);
        data.Add("getPush", "true");
        helper.post(2, userUrl, data);
    }

    public void UpdateUser() // userData를 갱신해놓으면 그 데이터를 업데이트한다.
    {
        // put
        Debug.Log("UpdateUser");
        WWWHelper helper = WWWHelper.Instance;
        string data = JsonMapper.ToJson(userData);
        Debug.Log("put data: " + data);
        helper.put(2, userUrl + "/?email=" + userData.email + "&did=" + userData.did, data);
    }

    public void GetUser()
    {
        Debug.Log("GetUser");
        WWWHelper helper = WWWHelper.Instance;
        helper.get(2, userUrl + "/?email=" + userData.email + "&did=" + userData.did);
    }

    public void GetBoxData()
    {
        Debug.Log("GetBoxData");
        WWWHelper helper = WWWHelper.Instance;
        helper.get(1, userUrl + "/?box=true");
    }

    public static void InsertTimeLog(int logType, long time)
    {
        Debug.Log("InsertTimeLog");
        WWWHelper helper = WWWHelper.Instance;
        LogData logData = new LogData();
        logData.email = userData.email;
        logData.logCmd = logType; //(int)LogCmd.firstIncome;
        if(time > 0)
            logData.date = time.ToString();
        else
            logData.date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        Debug.Log("date: " + logData.date);
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("logCmd", "" + logData.logCmd);
        data.Add("email", logData.email);
        data.Add("date", logData.date);
        helper.post(3, userUrl, data);
    }

    public void InsertGetItemLog(GiftItem item)
    {
        Debug.Log("InsertGetItemLog");
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
        helper.post(3, userUrl, data);
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
                    if (data.IsArray) // 유저 데이터 불러오기 후
                    {
                        Debug.Log("data is array");
                        data = data[0];
                    }
                    else // 유저 추가 후
                        Debug.Log("data is not array");
                    Debug.Log("data count: " + data.Count);
                    userData.money = Int32.Parse(data["money"].ToString());
                    userData.rank = data["rank"].ToString();
                    userData.heart = Int32.Parse(data["heart"].ToString());
                    userData.charge = Int32.Parse(data["charge"].ToString());
                    userData.currentBoxId = data["currentBoxId"].ToString();
                    if (data["getPush"].ToString().ToLower().Equals("true"))
                        userData.getPush = true;
                    else if (data["getPush"].ToString().ToLower().Equals("false"))
                        userData.getPush = false;
                    JsonData items = JsonMapper.ToObject(data["pickItems"].ToJson());
                    int cnt = items.Count;
                    userData.pickItems.Clear();
                    for (int i = 0; i < cnt; i++)
                    {
                        userData.pickItems.Add(JsonMapper.ToObject<GiftItem>(items[i].ToJson()));
                    }
                    Debug.Log("get uesrdata end");
                    SetVisibleUserdata();
                }
            }
            catch (KeyNotFoundException e)
            {
                Debug.Log("keynotfoundexp: " + www.text);
            }
        }
        else if (id == (int)LoadDataNum.log)
        {

        }
    }

    void OnHttpRequest2(int id, string responseText)
    {
        Debug.Log("onhttpreq2: " + responseText);
    }

    public static void HandleResult(IResult result)
    {
        Debug.Log("handleresult");
        if (result == null)
        {
            //this.LastResponse = "Null Response\n";
            //LogView.AddLog(this.LastResponse);
            Debug.Log("null res");
            return;
        }

        //this.LastResponseTexture = null;

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            //this.Status = "Error - Check log for details";
            //this.LastResponse = "Error Response:\n" + result.Error;
            //LogView.AddLog(result.Error);
            Debug.Log("error: " + result.Error);
            // 테스트용 피시유니티는 페북로그인이 안됨
            WWWHelper helper = WWWHelper.Instance;
            helper.get(2, userUrl + "/?email=" + userData.email + "&did=" + userData.did);
        }
        else if (result.Cancelled)
        {
            //this.Status = "Cancelled - Check log for details";
            //this.LastResponse = "Cancelled Response:\n" + result.RawResult;
            //LogView.AddLog(result.RawResult);
            Debug.Log("Cancelled Response:\n" + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            //this.Status = "Success - Check log for details";
            //this.LastResponse = "Success Response:\n" + result.RawResult;
            //LogView.AddLog(result.RawResult);
            JsonData json = JsonMapper.ToObject(result.RawResult);
            try // 페이스북에서 받은 이메일 정보로 유저 데이터 요청
            {
                string email = json["email"].ToString();
                if (email != null && email.Length > 0)
                {
                    Debug.Log("fb result email: " + email);
                    GameManager.userData.email = email;
                    WWWHelper helper = WWWHelper.Instance;
                    helper.get(2, userUrl + "/?email=" + userData.email + "&did=" + userData.did);
                    // 처음 앱 킨 로그 전송
                    GameManager.InsertTimeLog((int)LogCmd.firstIncome, 0);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            //this.LastResponse = "Empty Response\n";
            //LogView.AddLog(this.LastResponse);
            Debug.Log("Empty Response");
        }
    }

    public void SetGiftResult(GiftItem item)
    {
        // 뽑기 버튼 숨기기
        // 선물 상자 이미지 숨기기
        // 대화상자 다시 나타내고 당첨 결과 출력
        nextBtn.gameObject.SetActive(false);
        msgBtn.gameObject.SetActive(true);
        shareBtn.gameObject.SetActive(true);
        //upgradeBtn.gameObject.SetActive(false);
        //settingBtn.gameObject.SetActive(false);
        spriteRenderer.sprite = giftSprites[item.type];
        string script = "월급상자에서 " + item.text + " 이(가) 나왔다!";
        msgBox.PrintScript(script);
        // 유저 데이터 갱신
        if (userData.heart > 0)
            userData.heart--;
        heartText.text = "X " + userData.heart;
        if (item.type == 1)
        {
            userData.money += item.value;
            string moneyStr = "현금: " + userData.money + "만원";
            if (userData.money >= 10000)
                moneyStr = "현금: " + userData.money / 10000 + "억 " + userData.money % 10000 + "만원";
            cashText.text = moneyStr;
        }
        userData.pickItems.Add(item);
        // 갱신 유저 데이터 서버로 업데이트
        UpdateUser();
        // 뽑은 내용 json으로 만들어서 로그 전송
        InsertGetItemLog(item);
    }

    public void NoHeart()
    {
        msgBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(false);
        giftBox.gameObject.SetActive(false);
        string script = "남아있는 월급 상자가 없다..";
        msgBox.PrintScript(script);
    }

    IEnumerator HeartTimer()
    {
        if (userData.heart >= MAXREGENHEART)
        {
            userData.charge = MAXCHARGETIME;
        }
        else if (userData.heart < MAXREGENHEART) // 하트가 2개 이하일때만 증가 
        {
            if (userData.charge > 0)
            {
                userData.charge--;
                
            }
            else if (userData.charge == 0)
            {
                userData.heart++;
                userData.charge = MAXCHARGETIME;
                heartText.text = "X " + userData.heart;
            }
            timerText.text = userData.charge.ToString();
        }
        yield return new WaitForSeconds(1);
        StartCoroutine("HeartTimer");
    }
}
