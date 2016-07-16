using UnityEngine;
using System.Collections;
using LitJson;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static ArrayList giftList = new ArrayList();
    private string jsonUrl = "https://s3-ap-northeast-1.amazonaws.com/paydaybucket/data_utf8bom.json";
    private string userUrl = "http://52.193.33.78:3000/";
    enum LoadDataNum {
        item = 1,
        user
    };
    LoadDataNum loadData;

    public Button nextBtn;
    public Button msgBtn;
    public Button upgradeBtn;
    public Button settingBtn;
    public Text msgText;
    public GameObject giftBox;

    public MsgBox msgBox;

	// Use this for initialization
	void Start () {
        Debug.Log("start");
        LoadData();
	}
	
	// Update is called once per frame
	void Update () {
	
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
        giftBox.gameObject.SetActive(true);
        //upgradeBtn.gameObject.SetActive(true);
        //settingBtn.gameObject.SetActive(true);
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
        string script = "월급상자에서 " + item.text + " 이(가) 나왔다!";
        msgBox.PrintScript(script);
        giftBox.gameObject.SetActive(false);

        // 뽑은 내용 json으로 만들어서 로그 전송

    }

    public void LoadData()
    {
        Debug.Log("LoadData");
        WWWHelper helper = WWWHelper.Instance;
        helper.OnHttpRequest += OnHttpRequest;
        // 아이템 데이터 로딩
        helper.get(1, jsonUrl);
        // 유저 데이터 로딩 REST api로 post 등으로 날려야함
        //helper.get(2, userUrl);
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
                JsonData item = items[i];
                string description = item["description"].ToString();
                int index = Convert.ToInt32(item["index"].ToString());
                int rangeStart = Convert.ToInt32(item["rangeStart"].ToString());
                int rangeEnd = Convert.ToInt32(item["rangeEnd"].ToString());
                string text = item["text"].ToString();
                int type = Convert.ToInt32(item["type"].ToString());

                GiftItem gi = new GiftItem();
                gi.description = description;
                gi.index = index;
                gi.rangeStart = rangeStart;
                gi.rangeEnd = rangeEnd;
                gi.text = text;
                gi.type = type;
                giftList.Add(gi);

                /*Debug.Log("des: " + description);
                 Debug.Log("index: " + index);
                 Debug.Log("rangeStart: " + rangeStart);
                 Debug.Log("rangeEnd: " + rangeEnd);
                 Debug.Log("text: " + text);*/
            }
            //GiftItem gi2 = (GiftItem)giftList[0];
            //Debug.Log("after: " + gi2.description);
        }
        else if(id == (int)LoadDataNum.user)
        {

        }
    }
}
