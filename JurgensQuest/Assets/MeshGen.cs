using UnityEngine;
using System.Collections;

public class MeshGen : MonoBehaviour {
    Mesh mesh;
	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[] { new Vector3(-1, 0, -1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(1, 0, 1) }; //list.Count * 2

        Vector2[] newUV;

        /*for (int i = 0; i < list.Count - 1; i++)
        {
            vertices[i] = list[i].transform.localPosition + transform.forward;
            vertices[i + 1] = list[i + 1].transform.localPosition - transform.forward;
        }*/
        int[] triangles = new int[(vertices.Length * 2 - 2) * 3];
        for (int i = 0; i < vertices.Length - 2; i++)
        {
            triangles[i * 3] = i;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
