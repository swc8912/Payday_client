using UnityEngine;
using System.Collections;
 
using UnityEngine.UI;
using UnityEngine.Advertisements;
 
public class AdsManager : MonoBehaviour {
    public Button adsBtn;
    public GameManager gm;
    private ShowOptions _ShowOpt = new ShowOptions();
 
    void Awake()
    {
        Advertisement.Initialize("1094942", false);
        _ShowOpt.resultCallback = OnAdsShowResultCallBack;
        UpdateButton();
    }
 
    void OnAdsShowResultCallBack(ShowResult result)
    {
        Debug.Log("ads result callback");
        if (result == ShowResult.Finished)
        {
            Debug.Log("ads finished");
            GameManager.userData.heart++;
            GameManager.userData.charge = GameManager.MAXCHARGETIME;
            adsBtn.gameObject.SetActive(false);
            gm.SetVisibleHeart();
            gm.InitScene();
        }
    }
 
    void UpdateButton()
    {
        adsBtn.interactable = Advertisement.IsReady();
        //adsBtn.GetComponentInChildren<Text>().text 
        //    = "See ads and earn gold\r\nGold = " + _Gold.ToString();
    }
 
    public void OnBtnUnityAds()
    {
        Advertisement.Show(null, _ShowOpt);
    }    
 
    void Update() { UpdateButton(); }
}
 
