using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager inst { get; private set; }
    public GameObject playerPrefab;
    public GameObject menuPanel;
    public Timer raceTimer;

    public Transform playerSpawnPoint;
    public Follower cameraFollower;

    private GameObject player = null;
    private GameObject env;

	// Use this for initialization
	void Start () {
        inst = this;
        if (playerPrefab == null) Debug.LogError("PleyerPrefab is null!");
        env = GameObject.Find("Environment");
        Restart();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
	}

    public void PlayerCaught()
    {
        foreach( Rigidbody2D body in player.GetComponentsInChildren<Rigidbody2D>() ) {
            body.drag = 300.0f;
        }
        menuPanel.SetActive(true);
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

    public void Quit()
    {
        Application.LoadLevel(0);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
