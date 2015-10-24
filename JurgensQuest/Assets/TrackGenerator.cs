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
    public Segment segmentPrefab;
    private LineRenderer lineRender;

	void Start () {
        collider = GetComponent<EdgeCollider2D>();
        lineRender = GetComponent<LineRenderer>();
#if UNITY_EDITOR
        
        lineRender.enabled = true;
#else 
        lineRender.enabled = false;
#endif
        InitializeNodes();
	}

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
            if (0.5f < angle && angle < 30.0f && middle != transform)
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
        lineRender.SetVertexCount(p.Length);
        for (int j = 0; j < p.Length; j++)
        {
            lineRender.SetPosition(j, new Vector3(p[j].x, p[j].y));
        }
                
    }
}
