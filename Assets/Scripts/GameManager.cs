using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static ArrayList giftList = new ArrayList();
    private string jsonUrl = "https://s3-ap-northeast-1.amazonaws.com/paydaybucket/data_utf8bom.json";
    private string userUrl = "http://52.193.33.78:3000/";
    private const int MAXCHARGETIME = 10;
    private const int MAXREGENHEART = 3;
    enum LoadDataNum {
        item = 1, // 아이템 리스트
        user, // 유저 데이터
        log // 로그 데이터 전송
    };
    LoadDataNum loadData;
    public static UserData userData = new UserData();

    public Button nextBtn;
    public Button msgBtn;
    public Button upgradeBtn;
    public Button settingBtn;
    public Text msgText;
    public Text cashText;
    public Text rankText;
    public Text heartText;
    public Text timerText;
    public GameObject giftBox;

    public MsgBox msgBox;

    private SpriteRenderer spriteRenderer;
    public Sprite[] giftSprites;

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
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
	}

    public void InitScene()
    {
        // 대화창 안보이게
        // 상자 이미지 보이게
        // 상자 열기 버튼 보이게
        // 직급 돈 보이게
        // 설정 강화 버튼 보이게
        // 하트 보이게
        Debug.Log("initscene");
        msgBtn.gameObject.SetActive(false);
        msgText.text = "";
        nextBtn.gameObject.SetActive(true);
        cashText.gameObject.SetActive(true);
        rankText.gameObject.SetActive(true);
        giftBox.gameObject.SetActive(true);
        heartText.gameObject.SetActive(true);
        spriteRenderer.sprite = giftSprites[0];
        //upgradeBtn.gameObject.SetActive(true);
        //settingBtn.gameObject.SetActive(true);
    }

    public void LoadData()
    {
        Debug.Log("LoadData");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        // 아이템 데이터 로딩
        helper.get(1, jsonUrl);
        // 유저 데이터 로딩 REST api로 post 등으로 날려야함
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("did", SystemInfo.deviceUniqueIdentifier);
        data.Add("email", ""); // 디바이스 id와 email로 본인 데이터 확인
        //helper.post(2, userUrl); //IDictionary<string, string> data

#if UNITY_IOS || UNITY_ANDROID
        // 기본 데이터
        userData.did = SystemInfo.deviceUniqueIdentifier;
        //Debug.Log("did: " + SystemInfo.deviceUniqueIdentifier);
#endif
        userData.email = "";
        userData.heart = MAXREGENHEART;
        userData.money = 0;
        userData.rank = "인턴";
        userData.uid = "init";
        userData.charge = MAXCHARGETIME;
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
            Debug.Log(www.text);
        }

        JsonData json = JsonMapper.ToObject(www.text);
        if (id == (int)LoadDataNum.item)
        {
            JsonData items = json["data"];
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                string str = items[i].ToJson();
                GiftItem item = JsonMapper.ToObject<GiftItem>(str);
                giftList.Add(item);
            }
            GiftItem gi2 = (GiftItem)giftList[0];
            Debug.Log("after: " + gi2.description);
        }
        else if(id == (int)LoadDataNum.user)
        {

        }
    }

    public void SetGiftResult(GiftItem item)
    {
        // 뽑기 버튼 숨기기
        // 선물 상자 이미지 숨기기
        // 대화상자 다시 나타내고 당첨 결과 출력
        nextBtn.gameObject.SetActive(false);
        msgBtn.gameObject.SetActive(true);
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

        // 뽑은 내용 json으로 만들어서 로그 전송

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
