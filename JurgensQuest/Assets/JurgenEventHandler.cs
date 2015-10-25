using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class JurgenEventHandler : MonoBehaviour {
    public AudioClip[] footStepClips;
    public void OnFootStep()
    {
        if (footStepClips.Length == 0) return;
        if(GetComponent<Animator>().GetFloat("Grounded") < 0.3f)
            GetComponent<AudioSource>().PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length - 1)]);
    }
}
