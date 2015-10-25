using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TrackGenerator : MonoBehaviour {
    private List<Segment> list;
    private EdgeCollider2D collider;
    private Mesh mesh;

	void Start () {
        collider = GetComponent<EdgeCollider2D>();
#if UNITY_EDITOR
        InitializeNodes();
#endif
	}
#if UNITY_EDITOR
    [ContextMenu("Generate Mesh")]
    private void GenerateMesh()
    {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[list.Count * 2 + 2]; //
        Vector3[] normals = new Vector3[list.Count * 2 + 2];
        vertices[0] = transform.forward * 40.0f;
        vertices[1] = -transform.forward * 40.0f;
        normals[0] = transform.up;
        normals[1] = transform.up;
        Vector2[] newUV = new Vector2[vertices.Length];

        Vector3 min = Vector3.Min(vertices[0], vertices[1]);
        Vector3 max = Vector3.Max(vertices[0], vertices[1]);

        for (int i = 0; i < list.Count; i++)
        {
            vertices[i * 2 + 2] = list[i].transform.localPosition + transform.forward * 40.0f;
            vertices[i * 2 + 3] = list[i].transform.localPosition - transform.forward * 40.0f;
            min = Vector3.Min(min, vertices[i * 2 + 2]);
            max = Vector3.Max(max, vertices[i * 2 + 2]);
            min = Vector3.Min(min, vertices[i * 2 + 3]);
            max = Vector3.Max(max, vertices[i * 2 + 3]);
        }
        /*for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 normal = Vector3.zero;
            if (i > 1) normal += Vector3.Cross(vertices[i], vertices[i - 2]);
            if (i < vertices.Length - 2) normal += Vector3.Cross(vertices[i], vertices[i + 2]);
            Debug.Log(normal.normalized);
            normals[i] = normal.normalized;
        }*/
        int[] triangles = new int[(vertices.Length - 2) * 6];

        for (int i = 0; i < (vertices.Length - 2); i = i + 2)
        {
            triangles[i * 6] = i + 3;
            triangles[i * 6 + 1] = i + 1;
            triangles[i * 6 + 2] = i;
            triangles[i * 6 + 3] = i + 2;
            triangles[i * 6 + 4] = i + 3;
            triangles[i * 6 + 5] = i;
        }
        mesh.vertices = vertices;
        //mesh.normals = normals;
        Vector2 last0 = Vector2.zero;
        Vector2 last1 = Vector2.zero;
        for (int i = 0; i < newUV.Length-1; i+=2) // / (max.z - min.z) // /( max.x - min.x) // Mathf.Sqrt(vertices[i].x * vertices[i].x + vertices[i].y * vertices[i].y)
        {
            float x0 = vertices[i+1].x - last0.x;
            float y0 = vertices[i+1].y - last0.y;
            newUV[i] = new Vector2(last0.x + Mathf.Sqrt(x0 * x0 + y0 * y0), vertices[i].z);
            last0 = new Vector2(vertices[i+1].x, vertices[i+1].y);

            float x1 = vertices[i].x - last1.x;
            float y1 = vertices[i].y - last1.y;

            newUV[i + 1] = new Vector2(last1.x + Mathf.Sqrt(x0 * x0 + y0 * y0), vertices[i+1].z);
            last1 = new Vector2(vertices[i].x, vertices[i].y);
        }
        mesh.uv = newUV;
        
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        /*bool switsch = false;
            
        Vector3[] n = mesh.normals;
        for(int i = 0; i < n.Length; i++)
        {
            if (switsch) n[i] = -n[i];
            switsch = !switsch;
        }
        mesh.normals = n;
        mesh.RecalculateNormals();*/

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.UploadMeshData(true);
    }
#endif

	void Update () {
#if UNITY_EDITOR // So that we can see our nodes and path
        if (!Application.isPlaying)
        {
            collider = GetComponent<EdgeCollider2D>();
            InitializeNodes();
        }
        Transform last = transform;
        foreach (Segment s in list)
        {
            Debug.DrawLine(last.position, s.transform.position, Color.cyan);
            last = s.transform;
        }
#endif
        
	}


    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.4f);
    }

    [ContextMenu("Smoothen Track")]
    private void SmoothTrack()
    {
        List<Segment> newTrack = new List<Segment>();
        Vector3 back = transform.position + new Vector3(-1.0f, 0, 0);
        Transform middle = transform;
        int middleIndex = 0;
        int index = 0;
        foreach (Segment s in list)
        {
            Vector3 a = middle.position - back;
            Vector3 b = s.transform.position - middle.position;
            float angle = Vector3.Angle(a, b);
            if (5.0f < angle && angle < 30.0f && middle != transform)
            {
                GameObject g = new GameObject();
                g.AddComponent<Segment>();
                g.transform.parent = transform;
                Vector3 b1 = BezierLerp(back, middle.position, s.transform.position, 0.33f);
                Vector3 b2 = BezierLerp(back, middle.position, s.transform.position, 0.66f);
                g.transform.position = b1;
                middle.transform.position = b2;
                g.GetComponent<Segment>().priority = index - 1;
                middle.GetComponent<Segment>().priority = index;
                g.transform.SetSiblingIndex(index-1);
                newTrack.Add(g.GetComponent<Segment>());
                index++;
            }
            newTrack.Add(s);
            s.priority = index;
            s.transform.SetSiblingIndex(index);
            index++;
            back = middle.position;
            middle = s.transform;
        }
        
        list = newTrack;
        
        InitializeNodes();
    }

    private Vector3 BezierLerp(Vector3 back, Vector3 middle, Vector3 front, float x)
    {
        Vector3 first = Vector3.Lerp(back, middle, x);
        Vector3 second = Vector3.Lerp(middle, front, x);
        return Vector3.Lerp(first, second, x);
    }

    public void InitializeNodes()
    {
        Segment[] t = transform.GetComponentsInChildren<Segment>();
        list = new List<Segment>(t);
        int i = 0;
        Vector2[] p = new Vector2[list.Count + 1];
        p[i] = new Vector2(0, 0);
        foreach (Segment s in list)
        {
            s.priority = i;
            i++;
            p[i] = new Vector2(s.transform.localPosition.x, s.transform.localPosition.y);
        }
        collider.points = p;
                
    }
}
