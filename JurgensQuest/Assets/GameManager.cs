using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager inst { get; private set; }
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    private GameObject player = null;
    private GameObject env;
    public Follower cameraFollower;
    public Timer raceTimer;
	// Use this for initialization
	void Start () {
        inst = this;
        if (playerPrefab == null) Debug.LogError("PleyerPrefab is null!");
        env = GameObject.Find("Environment");
        Restart();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Finish()
    {
        Restart();
    }

    public void Restart()
    {
        if (player != null) Destroy(player);
        player = Instantiate<GameObject>(playerPrefab);
        player.transform.position = playerSpawnPoint.position;
        cameraFollower.target = player.GetComponentInChildren<Character>().transform;
        raceTimer.StartTimer();
        if(env != null) env.SendMessage("Reset");
    }
}
