using UnityEngine;
using System.Collections;

public class FinishZone : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>() != null)
        {
            GameManager.inst.Finish();
        }
    }
}
