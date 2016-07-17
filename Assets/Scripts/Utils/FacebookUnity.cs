using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class FacebookUnity {
    public void initFB()
    {
        FB.Init(this.OnInitComplete, this.OnHideUnity);
        //this.Status = "FB.Init() called with " + FB.AppId;
    }

    public void loginFB()
    {
        this.CallFBLogin();
        //this.Status = "Login called";
    }
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
    public void logoutFB()
    {
        CallFBLogout();
        //this.Status = "Logout called";
    }
#endif
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

    private void CallFBLogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
    }

    private void CallFBLoginForPublish()
    {
        // It is generally good behavior to split asking for read and publish
        // permissions rather than ask for them all at once.
        //
        // In your own game, consider postponing this call until the moment
        // you actually need it.
        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, this.HandleResult);
    }

    private void CallFBLogout()
    {
        FB.LogOut();
    }

    private void OnInitComplete()
    {
        //this.Status = "Success - Check logk for details";
        //this.LastResponse = "Success Response: OnInitComplete Called\n";
        //string logMessage = string.Format(
        //    "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
        //    FB.IsLoggedIn,
        //    FB.IsInitialized);
        //LogView.AddLog(logMessage);
        if (!FB.IsLoggedIn)
            this.loginFB();
        else
            SceneManager.LoadScene("IntroScene");
    }

    private void OnHideUnity(bool isGameShown)
    {
        //this.Status = "Success - Check logk for details";
        //this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
        //LogView.AddLog("Is game shown: " + isGameShown);
    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            //this.LastResponse = "Null Response\n";
            //LogView.AddLog(this.LastResponse);
            return;
        }

        //this.LastResponseTexture = null;

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            //this.Status = "Error - Check log for details";
            //this.LastResponse = "Error Response:\n" + result.Error;
            //LogView.AddLog(result.Error);
        }
        else if (result.Cancelled)
        {
            //this.Status = "Cancelled - Check log for details";
            //this.LastResponse = "Cancelled Response:\n" + result.RawResult;
            //LogView.AddLog(result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            //this.Status = "Success - Check log for details";
            //this.LastResponse = "Success Response:\n" + result.RawResult;
            //LogView.AddLog(result.RawResult);
        }
        else
        {
            //this.LastResponse = "Empty Response\n";
            //LogView.AddLog(this.LastResponse);
        }
    }
}
