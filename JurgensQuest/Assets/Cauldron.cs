using UnityEngine;
using System.Collections;

public class Cauldron : MonoBehaviour {
    public Rigidbody2D childPrefab;
    private DistanceJoint2D joint;
    private Rigidbody2D rbody;
    private float lastVel;
    public float bumpTreshold = 15.0f;
    public float velocityThreshold = 1.0f;
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
        if (Mathf.Abs(vel / Time.fixedDeltaTime - lastVel / Time.fixedDeltaTime) > velocityThreshold)
        {
            StartCoroutine(ChildrenOverboard(4));
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
            if (Vector3.Project(coll.relativeVelocity, c.normal).magnitude > bumpTreshold)
            {
                StartCoroutine(ChildrenOverboard(7));
            }
        }
    }
}
