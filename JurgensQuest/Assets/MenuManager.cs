using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;


using System.Collections.Generic;
public class MenuManager : MonoBehaviour {

    public void LoadLevel(string name)
    {
        
        Application.LoadLevel(name);
    }

	public void Exit() {
        
		Application.Quit ();
	}

    void Start()
    {
        Analytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
    }

    

    void OnApplicationQuit()
    {

        Analytics.CustomEvent("quit", new Dictionary<string, object> {
            {"timePlayed", Time.realtimeSinceStartup}
        });
    }
}
