using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class RandomPlayer : MonoBehaviour {
    public bool playOnStart = true;
    public AudioClip[] clips;
    
	// Use this for initialization
	void Start () {
        if (playOnStart) Play();
	}
    public void Play()
    {
        if (clips.Length == 0) return;
        GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }
}
