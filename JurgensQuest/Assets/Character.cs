using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Character : MonoBehaviour {
    private Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Grav"))
        {
            rBody.gravityScale *= 2;
        }
        if (Input.GetButtonUp("Grav"))
        {
            rBody.gravityScale /= 2;
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
