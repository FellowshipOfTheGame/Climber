using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
    public float frequency;
    public float phase;
    public float amplitude;
    public Vector3 axis = Vector3.up;
    Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = pos + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.realtimeSinceStartup + phase) * Vector3.up;
    }
}
