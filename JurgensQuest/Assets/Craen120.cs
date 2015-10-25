using UnityEngine;
using System.Collections;

public class Craen120 : MonoBehaviour {


    private Vector3 startPosition = new Vector3(0, 10, 0);
    public float mass = 1.0f;
    private float damping = 0.0f;
    private float invMass;
    private Vector3 prevPos;
    private Vector3 force = Vector3.zero;
    public float springK = 10.0f;
    public float followFactor = 0.5f;
    public float maxFollowSpeed = 1.5f;

    public float xVelocity { get { return Vector3.Project((transform.position - prevPos) * (1.0f / Time.fixedDeltaTime), Vector3.right).magnitude; } }

    private Character target;
    
	// Use this for initialization
	void Start () {
        invMass = 1.0f / mass;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.Log(xVelocity);
        
        AddSpringForce();

        Vector3 nPos = transform.position * (2.0f - damping) - prevPos * (1.0f - damping) + force * Time.fixedDeltaTime * Time.fixedDeltaTime * invMass;
        prevPos = transform.position;
        transform.position = nPos;
        force = Vector3.zero;
        if ((target.transform.position - transform.position).x < 0.0f)
        {
            GameManager.inst.Restart();
        }
    }

    private void AddSpringForce()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
        if (hit.collider != null)
        {
            Vector3 diff = Vector3.down * hit.distance;
            diff = diff.normalized * Mathf.Min(diff.magnitude, 15.0f);
            AddForce((diff - diff.normalized * (10.0f)) * springK);
        }
        else
        {
            AddForce(Vector3.up * springK * 5.0f);
        }
    }

    private void AddFollowForce()
    {
        //AddForce(Vector2.right);
        Vector3 diff = target.transform.position - transform.position;
        diff.y = 0;
        diff = diff.normalized * Mathf.Min(diff.magnitude * followFactor, maxFollowSpeed);

        transform.position += (diff) * Time.deltaTime;
    }

    public void AddForce(Vector3 force)
    {
        this.force += force;
    }

    public void OnReset()
    {
        transform.position = startPosition;
        prevPos = transform.position;
        target = FindObjectOfType<Character>();
        AddForce(Vector2.right * 200.0f);
    }
}
