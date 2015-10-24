using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour {
    [HideInInspector]
    public int priority = -1;

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
