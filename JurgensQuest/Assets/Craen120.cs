using UnityEngine;
using System.Collections;

public class Craen120 : MonoBehaviour {


    private Vector3 startPosition;
    public float mass = 1.0f;
    public float damping = 0.001f;
    private float invMass;
    private Vector3 prevPos;
    private Vector3 force = Vector3.zero;
    public float springK = 10.0f;
    
	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        invMass = 1.0f / mass;
        prevPos = transform.position;
        AddForce(Vector2.right * 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
        AddForce(Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
        if (hit.collider != null)
        {
            Vector3 diff = Vector3.down * hit.distance;

            AddForce((diff - diff.normalized * (10.0f)) * springK);
        }
        else
        {
            AddForce(Vector3.up * springK * 5.0f);
        }
        Vector3 nPos = transform.position * (2.0f - damping) - prevPos * (1.0f - damping) + force * Time.deltaTime * Time.deltaTime * invMass;
        prevPos = transform.position;
        transform.position = nPos;
        force = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        this.force += force;
    }

    public void OnReset()
    {
        transform.position = startPosition;

    }
}
