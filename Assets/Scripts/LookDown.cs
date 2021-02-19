using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDown : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.localPosition;
		if(transform.rotation.x > 0)newPosition.z = (0.95f * (transform.localRotation.x/90));
		transform.localPosition = newPosition;
	}
}
