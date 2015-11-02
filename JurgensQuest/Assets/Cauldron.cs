using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityAnalyticsHeatmap;
[RequireComponent(typeof(AudioSource))]
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
    public AudioClip[] bounceSounds;
    private int nStartChildren;
    private bool grounded
    {
        get
        {
            return Physics2D.CircleCast(transform.position, GetComponent<CircleCollider2D>().radius * 1.2f, Vector2.down, 1.0f, LayerMask.GetMask("Floor")).collider != null;
        }
    }

	// Use this for initialization
	void Start () {
        joint = GetComponent<DistanceJoint2D>();
        rbody = GetComponent<Rigidbody2D>();
        lastVel = rbody.velocity;
        nStartChildren = nChildren;
        //joint.GetReactionTorque
	}
    void Update()
    {
         AudioSource s = GetComponent<AudioSource>();
         s.volume = rbody.velocity.magnitude / 1000.0f;
        if (grounded)
        {
           
            if (!s.isPlaying) s.Play();
        }
        else
        {
            s.Stop();
        }
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
        HeatmapEvent.Send("childrenOverboard", transform.position, GameManager.inst.raceTimer.elapsed, new Dictionary<string, object> { {"count", amount} });
        for (int i = 0; i < amount && nChildren > 0; i++)
        {
            Rigidbody2D o = Instantiate<Rigidbody2D>(childPrefab);
            o.position = transform.position;
            o.velocity = Vector2.up * 10.0f;
            nChildren--;
            int nRepresentatives = transform.childCount;
            int nVisible = Mathf.CeilToInt((float)nRepresentatives * ((float)nChildren / (float)nStartChildren));
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(j < nVisible);
            }
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

    private void PlayBounce()
    {
        if (bounceSounds.Length == 0) return;
        GetComponent<AudioSource>().PlayOneShot(bounceSounds[Random.Range(0, bounceSounds.Length - 1)]);
    }
}
