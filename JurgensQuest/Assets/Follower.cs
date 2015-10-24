using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public Transform target;

    void LateUpdate()
    {
        transform.position = target.position;
    }
}
