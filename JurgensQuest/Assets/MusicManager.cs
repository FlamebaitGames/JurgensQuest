using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
	public AudioSource musicLoop;
	public AudioSource musicInit;

	// Use this for initialization
	void Start () {

        //Restart();
		//audio.PlayScheduled (musicInit.length);
	}

    public void Restart()
    {
        StopAllCoroutines();
        musicInit.Stop();
        musicLoop.Stop();
        musicInit.Play();
        StartCoroutine(PlayDelay(musicInit.clip.length));
        //musicLoop.PlayDelayed(musicInit.clip.length);//(ulong)musicInit.clip.samples);
    }
	
	// Update is called once per frame
	void Update () {

		/*if (!audio.isPlaying){
			audio.clip = MusicLoop;
			audio.Play();
			audio.loop = true;
			audio.
		}*/
	}
    private IEnumerator PlayDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        musicLoop.Play();
    }
	/*private IEnumerator PlayList() {
		audio.Play ();
		yield return new WaitForSeconds (audio.clip.length);

		while (true) {
			audio.clip = musicLoop;
			audio.Play();
			yield return new WaitForSeconds(audio.clip.length);

		}
	}*/
}
