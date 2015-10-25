using UnityEngine;
using System.Collections;

public class Cauldron : MonoBehaviour {
    public Rigidbody2D childPrefab;
    private DistanceJoint2D joint;
    private Rigidbody2D rbody;
    private Vector3 lastVel;
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
        lastVel = rbody.velocity;
        //joint.GetReactionTorque
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 vel = rbody.velocity;
        float acc = ((vel - lastVel) * (1.0f / Time.fixedDeltaTime)).magnitude;
        if (acc > accellerationThreshold)
        {
            //StartCoroutine(ChildrenOverboard((int)(acc * childrenPerNewton)));
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
			Debug.DrawRay(c.point, Vector3.Project(lastVel, c.normal), Color.yellow, 300.0f);
			Debug.DrawRay(c.point, lastVel, Color.red, 300.0f); //coll.relativeVelocity
            float impactVel = Vector3.Project(lastVel, c.normal).magnitude;

            if (Vector3.Angle(c.normal, rbody.velocity.normalized) > bumpAngleThreshold && impactVel > bumpTreshold)
            {
				Debug.Log(impactVel);
                StartCoroutine(ChildrenOverboard((int)((impactVel-bumpTreshold) * childrenPerMS)));
            }
        }
    }
}
