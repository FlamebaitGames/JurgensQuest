using UnityEngine;
using System.Collections;

public class Cauldron : MonoBehaviour {
    public Rigidbody2D childPrefab;
    private DistanceJoint2D joint;
    private Rigidbody2D rbody;
    private float lastVel;
    public float bumpTreshold = 15.0f;
    public float bumpAngleThreshold = 20.0f;
    public float accellerationThreshold = 1.0f;
    public float childrenPerMS = 1.0f;
    public float childrenPerNewton = 1.0f;
    public int nChildren = 20;

	// Use this for initialization
	void Start () {
        joint = GetComponent<DistanceJoint2D>();
        rbody = GetComponent<Rigidbody2D>();
        lastVel = rbody.velocity.magnitude;
        //joint.GetReactionTorque
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float vel = rbody.velocity.magnitude;
        float acc = Mathf.Abs((vel - lastVel) / Time.fixedDeltaTime);
        if (acc > accellerationThreshold)
        {
            StartCoroutine(ChildrenOverboard((int)(acc * childrenPerNewton)));
        }
        lastVel = vel;
	}

    private IEnumerator ChildrenOverboard(int amount)
    {
        for (int i = 0; i < amount && nChildren > 0; i++)
        {
            Rigidbody2D o = Instantiate<Rigidbody2D>(childPrefab);
            o.position = transform.position;
            o.velocity = Vector2.up * 10.0f;
            nChildren--;
            yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
        }
            yield return null;
    }
    

    void OnCollisionEnter2D(Collision2D coll)
    {
        foreach (ContactPoint2D c in coll.contacts) // Check if the impact velocity is beyond threshold
        {
            float impactVel = Vector3.Project(coll.relativeVelocity, c.normal).magnitude;
            if (Vector3.Angle(c.normal, rbody.velocity.normalized) > bumpAngleThreshold && impactVel > bumpTreshold)
            {
                StartCoroutine(ChildrenOverboard((int)(impactVel * childrenPerMS)));
            }
        }
    }
}
