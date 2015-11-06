using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class Craen120 : MonoBehaviour {


    private Vector3 startPosition { get { return Vector3.up * optimalHeight; } }
    public float mass = 1.0f;
    private float damping = 0.02f;
    private float invMass;
    private Vector3 prevPos;
    private Vector3 force = Vector3.zero;
    public float springK = 10.0f;
    public float optimalHeight = 10.0f;
    public float flySpeed = 200.0f;

    private bool ratatat = false;
    private bool ratatating = false;

    private Light light;

    public float xVelocity { get { return Vector3.Project((transform.position - prevPos) * (1.0f / Time.fixedDeltaTime), Vector3.right).magnitude; } }

    private Character target;
    
	// Use this for initialization
	void Start () {
        invMass = 1.0f / mass;
        light = GetComponentInChildren<Light>();
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -30.0f));
	}

    public void Freeze()
    {
        damping = 0.95f;
        ratatat = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log(xVelocity);
        
        AddSpringForce();

        Vector3 nPos = transform.position * (2.0f - damping) - prevPos * (1.0f - damping) + force * Time.fixedDeltaTime * Time.fixedDeltaTime * invMass;
        prevPos = transform.position;
        transform.position = nPos;
        force = Vector3.zero;

        
        AddForce(Vector2.right * (flySpeed - xVelocity));
        
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 30.0f)
        {
            light.transform.rotation = Quaternion.Slerp(light.transform.rotation, Quaternion.LookRotation((target.transform.position - light.transform.position).normalized, Vector3.up), Time.deltaTime * 4.0f);
        }
        else
        {
            light.transform.rotation = Quaternion.Slerp(light.transform.rotation, Quaternion.Euler(60.0f, 90.0f, 0.0f), Time.deltaTime);
        }
        
        if ((target.transform.position - transform.position).x < 3.0f)
        {
            GameManager.inst.PlayerCaught();
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 30.0f)
        {
            StartRatTatTat();
        }
        else
        {
            ratatat = false;
        }
    }

    private void AddSpringForce()
    {
        RaycastHit2D hit0 = Physics2D.Raycast(transform.position, (Vector2.down + Vector2.left).normalized, Mathf.Infinity, LayerMask.GetMask("Floor"));
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, (Vector2.down + Vector2.right).normalized, Mathf.Infinity, LayerMask.GetMask("Floor"));
        
        Vector3 dist = Vector3.up * 50.0f;
        if (hit0.collider != null)
        {
            Debug.DrawLine(transform.position, hit0.point, Color.green);
            dist = Vector3.down * Mathf.Min(dist.magnitude, hit0.distance);
        }
        if (hit1.collider != null)
        {
            Debug.DrawLine(transform.position, hit1.point, Color.blue);
            dist = Vector3.down * Mathf.Min(dist.magnitude, hit1.distance);
        }
        if (hit2.collider != null)
        {
            Debug.DrawLine(transform.position, hit2.point, Color.red);
            dist = Vector3.down * Mathf.Min(dist.magnitude, hit2.distance);
        }
        Debug.DrawRay(transform.position, dist, Color.yellow);
        //dist = dist.normalized * Mathf.Min(dist.magnitude, 15.0f);
        AddForce((dist - dist.normalized * (optimalHeight)) * springK);
        
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
        damping = 0.02f;
    }

    private void StartRatTatTat()
    {
        ratatat = true; ;
        if (!ratatating) StartCoroutine(Ratatat());
    }

    private IEnumerator Ratatat()
    {
        
        ratatating = true;
        while (ratatat)
        {
            Debug.Log("RAT");

			if(GetComponent<AudioSource>().isPlaying);
			{
			
				GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
			}

            yield return new WaitForSeconds(3.0f);
        }

        ratatating = false;
    }
}
