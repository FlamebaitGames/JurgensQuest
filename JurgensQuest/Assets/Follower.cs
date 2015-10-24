using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
    
    public float maxSpeed = 60.0f;
    public Transform target;
    public bool useAngleVelocityOffset = true;
    public AnimationCurve angleVelocityCurve;
    public float lowestVelocityOffset = -10.0f;
    public float highestVelocityOffset = 50.0f;
    private float speedOffset = 0.0f;
    private float gravOffset = 0.0f;
    Vector3 lastPos;
    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position;
       
        float velocity = (target.position - lastPos).magnitude / Time.deltaTime;
        float f = Mathf.InverseLerp(0.0f, maxSpeed, Mathf.Clamp(velocity, 0.0f, maxSpeed));
        speedOffset = Mathf.MoveTowards(speedOffset, Mathf.Lerp(lowestVelocityOffset, highestVelocityOffset, f), Time.deltaTime);
        
        lastPos = target.position;
        RaycastHit2D rc = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
        if(rc != null) {
            Vector3 t = transform.position;
            t.z = -(rc.distance + 1) / 2;
            transform.position = t;
        }
        if (Input.GetButton("Grav")) gravOffset = Mathf.MoveTowards(gravOffset, 1.0f, Time.deltaTime);
        else gravOffset = Mathf.MoveTowards(gravOffset, 0.0f, Time.deltaTime);
        transform.position += Vector3.up * gravOffset;
        if(useAngleVelocityOffset) transform.position -= Vector3.right * speedOffset * -transform.position.z;
        Vector3 q = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up).eulerAngles;
        
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, q.y, 0.0f));
        //transform.LookAt(target, Vector3.up);
    }
}
