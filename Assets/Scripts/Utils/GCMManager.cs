using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GCMManager : MonoBehaviour {
    private string[] SENDER_IDS = { "639151196864" };

	// Use this for initialization
	void Start () {
        // Create receiver game object
        GCM.Initialize();
        Debug.Log("gcm start");
        // Set callbacks
        GCM.SetErrorCallback((string errorId) =>
        {
            Debug.Log("Error!!! " + errorId);
        });

        GCM.SetMessageCallback((Dictionary<string, object> table) =>
        {
            Debug.Log("Message!!! " + table.ToString());
        });

        GCM.SetRegisteredCallback((string registrationId) =>
        {
            Debug.Log("Registered!!! " + registrationId);
        });

        GCM.SetUnregisteredCallback((string registrationId) =>
        {
            Debug.Log("Unregistered!!! " + registrationId);
        });

        GCM.SetDeleteMessagesCallback((int total) =>
        {
            Debug.Log("DeleteMessages!!! " + total);
        });

        GCM.Register(SENDER_IDS);
	}
}
