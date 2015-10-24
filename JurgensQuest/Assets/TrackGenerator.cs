﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TrackGenerator : MonoBehaviour {
    private List<Segment> list;
    private EdgeCollider2D collider;

	void Start () {
        collider = GetComponent<EdgeCollider2D>();
        InitializeNodes();
	}

	void Update () {
#if UNITY_EDITOR // So that we can see our nodes and path
        collider = GetComponent<EdgeCollider2D>();
        InitializeNodes();
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
