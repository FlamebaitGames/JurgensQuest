using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class Rope : MonoBehaviour {
    public bool selfIsEnd = false;
    public bool otherIsEnd = false;
    public Rigidbody2D other;
    private Rigidbody2D self;
    private float restLength;
    public float slack = 1.0f;
	// Use this for initialization
	void Start () {
        if (other == null) Debug.Log("Other is null!");
        self = GetComponent<Rigidbody2D>();
        restLength = (transform.position - other.transform.position).magnitude * slack;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (other == null) return;
        Vector3 dist = transform.position - other.transform.position;
        float diff = (dist.magnitude - restLength) / dist.magnitude;
        float totMass = self.mass + other.mass;
        if(!selfIsEnd)
            self.MovePosition(self.transform.position + dist * -0.5f * diff * other.mass / totMass);
        if(!otherIsEnd)
            other.MovePosition(other.transform.position + dist * 0.5f * diff * self.mass / totMass);
	}

    /*
     * 
     * 
    MyVector posA = a.getPosition();
    MyVector posB = b.getPosition();
    MyVector dist = posA - posB;
    float distLen = dist.length();
    if (distLen == 0.f) return;
    float diff = (distLen - restLength) / distLen;
    float totMass = a.getMass() + b.getMass();
    float aS = a.getMass() / totMass;
    float bS = b.getMass() / totMass;
    if ((a.isStatic || !a.partOfRope) && (b.isStatic || !b.partOfRope)) {

    }
    else if (a.isStatic || !a.partOfRope) {
        b.move(dist * 0.5f * diff);
    }
    else if (b.isStatic || !b.partOfRope) {
        a.move(dist * -0.5f * diff);
    }
    else {

        a.move(dist * -0.5f * diff);
        b.move(dist * 0.5f * diff);
    }
     * 
     */
}
