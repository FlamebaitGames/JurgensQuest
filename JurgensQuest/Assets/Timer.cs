using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour {
    private Text text;
    public float elapsed { get; private set; }
    private float startAt;
	public bool paused = true;
    void Start()
    {
        text = GetComponent<Text>();
    }
	void Update () {
        if (!paused)
        {
            elapsed = Time.time - startAt;
            text.text = elapsed.ToString();
        }
	}

    public void StartTimer() {
        startAt = Time.time;
        paused = false;
    }
}
