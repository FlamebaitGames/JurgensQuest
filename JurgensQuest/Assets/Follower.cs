using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
    
    public float maxSpeed = 60.0f;
    public Transform target;
    public AnimationCurve angleVelocityCurve;
    public float lowestVelocityOffset = -10.0f;
    public float highestVelocityOffset = 50.0f;
    Vector3 lastPos;
    void LateUpdate()
    {
        transform.position = target.position;
       
        float velocity = (target.position - lastPos).magnitude / Time.deltaTime;
        float f = Mathf.InverseLerp(0.0f, maxSpeed, Mathf.Clamp(velocity, 0.0f, maxSpeed));

        Debug.Log(velocity);
        transform.position -= Vector3.right * Mathf.Lerp(lowestVelocityOffset, highestVelocityOffset, f);
        lastPos = target.position;
        RaycastHit2D rc = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
        if(rc != null) {
            Vector3 t = transform.position;
            t.z = -(rc.distance + 1);
            transform.position = t;
        }
        transform.LookAt(target, Vector3.up);
    }
}
