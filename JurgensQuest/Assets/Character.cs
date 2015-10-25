using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Character : MonoBehaviour {
    private Rigidbody2D rBody;
    private FixedJoint joint;
    private Animator animator;
    
    private float flying = 0.0f;
	public float gravMultiplier = 2.0f;
    private bool grounded
    {
        get
        {
            return Physics2D.CircleCast(transform.position, GetComponent<CircleCollider2D>().radius * 1.2f, Vector2.down, 5.0f, LayerMask.GetMask("Floor")).collider != null;
        }
    }

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        Debug.Log(animator);
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat("Speed", rBody.velocity.magnitude);
        if (Input.GetButtonDown("Grav"))
        {
			rBody.gravityScale *= gravMultiplier;
            animator.SetTrigger("BeginHunker");
        }
        if (Input.GetButtonUp("Grav"))
        {
			rBody.gravityScale /= gravMultiplier;
            animator.SetTrigger("EndHunker");
        }

        flying = Mathf.MoveTowards(flying, (grounded ? 0.0f : 1.0f), Time.deltaTime);
        animator.SetFloat("Grounded", flying);
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            //rBody.AddTorque(300.0f);
            foreach(Rigidbody2D r in transform.parent.GetComponentsInChildren<Rigidbody2D>()) {
                r.AddForceAtPosition(Vector2.up * 900, rBody.position - (Vector2)(transform.right * 3.0f));
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            // Alter flag position
            GameObject g = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Private/Marker.prefab")) as GameObject;
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            GameObject o = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Private/Marker.prefab");
            PrefabUtility.ReplacePrefab(g, o, ReplacePrefabOptions.ConnectToPrefab);
        }
#endif
    }


    
    /*void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("GROUNDED");
        grounded = true;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        Debug.Log("NYA");
        grounded = false;
    }*/
}
