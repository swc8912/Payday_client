using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using LitJson;

public static class FacebookUnity {
    public static void InitFB()
    {
        FB.Init(OnInitComplete, OnHideUnity);
        //this.Status = "FB.Init() called with " + FB.AppId;
    }

    public static void LoginFB()
    {
        CallFBLogin();
        //this.Status = "Login called";
    }
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
    public static void logoutFB()
    {
        CallFBLogout();
        //this.Status = "Logout called";
    }
#endif

    public static void GetUserDataFB()
    {
        FB.API("/me?fields=id,email", HttpMethod.GET, GameManager.HandleResult);
    }

    public static void FeedShare(GiftItem item)
    {
        /*
         * FB.FeedShare(
          link: "https://example.com/myapp/?storyID=thelarch",
          linkName: "The Larch",
          linkCaption: "I thought up a witty tagline about larches",
          linkDescription: "There are a lot of larch trees around here, aren't there?",
          picture: "https://example.com/myapp/assets/1/larch.jpg",
          callback: FeedCallback
        );
         */
        FB.FeedShare(
            string.Empty, //toId
            new System.Uri("https://play.google.com/store/apps/details?id=kr.Celes.Payday"), //link
            "오늘은 월급날", //linkName
            item.description, //linkCaption
            "월급상자에서 " + item.text + " 이(가) 나왔다!", //linkDescription
            new System.Uri("https://s3-ap-northeast-1.amazonaws.com/paydaybucket/paydayico192.png"), //picture  null
            string.Empty, //mediaSource
            LogCallback //callback
        );
    }

    private static void LogCallback(IResult response)
    {
        Debug.Log("feedshare Worked");
    }

/*
    protected override void GetGui()
    {
        bool enabled = GUI.enabled;
        if (this.Button("FB.Init"))
        {
            
            
        }

        GUILayout.BeginHorizontal();

        GUI.enabled = enabled && FB.IsInitialized;
        if (this.Button("Login"))
        {
            
        }

        GUI.enabled = FB.IsLoggedIn;
        if (this.Button("Get publish_actions"))
        {
            this.CallFBLoginForPublish();
            this.Status = "Login (for publish_actions) called";
        }


        if (Button("Logout"))
        {
            
        }

        GUILayout.EndHorizontal();

        GUI.enabled = enabled && FB.IsInitialized;
        if (this.Button("Share Dialog"))
        {
            this.SwitchMenu(typeof(DialogShare));
        }

        bool savedEnabled = GUI.enabled;
        GUI.enabled = enabled &&
            AccessToken.CurrentAccessToken != null &&
            AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions");
        if (this.Button("Game Groups"))
        {
            this.SwitchMenu(typeof(GameGroups));
        }

        GUI.enabled = savedEnabled;

        if (this.Button("App Requests"))
        {
            this.SwitchMenu(typeof(AppRequests));
        }

        if (this.Button("Graph Request"))
        {
            this.SwitchMenu(typeof(GraphRequest));
        }

        if (Constants.IsWeb && this.Button("Pay"))
        {
            this.SwitchMenu(typeof(Pay));
        }

        if (this.Button("App Events"))
        {
            this.SwitchMenu(typeof(AppEvents));
        }

        if (this.Button("App Links"))
        {
            this.SwitchMenu(typeof(AppLinks));
        }

        if (Constants.IsMobile && this.Button("App Invites"))
        {
            this.SwitchMenu(typeof(AppInvites));
        }

        GUI.enabled = enabled;
    }*/

    private static void CallFBLogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, GameManager.HandleResult);
    }

    public static void CallFBLoginForPublish()
    {
        // It is generally good behavior to split asking for read and publish
        // permissions rather than ask for them all at once.
        //
        // In your own game, consider postponing this call until the moment
        // you actually need it.
        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, GameManager.HandleResult);
    }

    private static void CallFBLogout()
    {
        FB.LogOut();
    }

    private static void OnInitComplete()
    {
        //this.Status = "Success - Check logk for details";
        //this.LastResponse = "Success Response: OnInitComplete Called\n";
        //string logMessage = string.Format(
        //    "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
        //    FB.IsLoggedIn,
        //    FB.IsInitialized);
        //LogView.AddLog(logMessage);
        if (!FB.IsLoggedIn)
            LoginFB();
        else
            SceneManager.LoadScene("MainScene");
    }

    private static void OnHideUnity(bool isGameShown)
    {
        //this.Status = "Success - Check logk for details";
        //this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
        //LogView.AddLog("Is game shown: " + isGameShown);
    }

    
}
