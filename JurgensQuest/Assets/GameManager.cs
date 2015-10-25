using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    public static GameManager inst { get; private set; }
    public GameObject playerPrefab;
    public GameObject menuPanel;
    
    public Timer raceTimer;
    public float estFinishTime = 180.0f;
    public UnityEngine.UI.Text scoreText;

    public AudioClip beginSound;
    public AudioClip[] loseSounds;
    public AudioClip[] winSounds;

    public Transform playerSpawnPoint;
    public Follower cameraFollower;

    private GameObject player = null;
    private GameObject env;

    private bool gameEnded = false;
    

	// Use this for initialization
	void Start () {
        inst = this;
        if (playerPrefab == null) Debug.LogError("PlayerPrefab is null!");
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
        if (gameEnded) return;
        gameEnded = true;
        Freeze();
        
        menuPanel.SetActive(true);
        PlayRandom(loseSounds);
    }

    public void Finish()
    {
        if (gameEnded) return;
        gameEnded = true;
        PlayRandom(winSounds);
        menuPanel.SetActive(true);
        scoreText.gameObject.SetActive(true);
        int nChildrenAlive = 0;
        foreach (Cauldron c in player.GetComponentsInChildren<Cauldron>()) {
            nChildrenAlive += c.nChildren;
        }
        float score = (estFinishTime - raceTimer.elapsed);
        if (score < 0.0f)
        {
            score = (float)nChildrenAlive / Mathf.Abs(score);
        } else {
            score *= nChildrenAlive;
        }
        scoreText.text = "Score: " + score;
        Freeze();
        //Restart();
    }

    private void Freeze()
    {
        foreach (Rigidbody2D body in player.GetComponentsInChildren<Rigidbody2D>())
        {
            body.drag = 300.0f;
        }
        env.SendMessage("Freeze");
        raceTimer.paused = true;
    }

    public void Restart()
    {
        scoreText.gameObject.SetActive(false);
        gameEnded = false;
        if (player != null) Destroy(player);
        player = Instantiate<GameObject>(playerPrefab);
        player.transform.position = playerSpawnPoint.position;
        cameraFollower.target = player.GetComponentInChildren<Character>().transform;
        raceTimer.StartTimer();
        if(env != null) env.SendMessage("Reset");
        GetComponent<AudioSource>().PlayOneShot(beginSound);
    }

    public void Quit()
    {
		Debug.Log ("QUIT");
        Application.LoadLevel(0);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }


    public void PlayRandom(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }
}
