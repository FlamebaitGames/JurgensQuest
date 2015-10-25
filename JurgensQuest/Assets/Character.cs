using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Character : MonoBehaviour {
    private Rigidbody2D rBody;
    private FixedJoint joint;
	public float gravMultiplier = 2.0f;
	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Grav"))
        {
			rBody.gravityScale *= gravMultiplier;
        }
        if (Input.GetButtonUp("Grav"))
        {
			rBody.gravityScale /= gravMultiplier;
        }
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
}
