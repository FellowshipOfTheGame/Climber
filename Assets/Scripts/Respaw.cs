using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respaw : MonoBehaviour {
	public float minY = -10;
	private Vector3 _initialPosition;
	// Use this for initialization
	void Start () {
		_initialPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < minY) {
			gameObject.transform.position = _initialPosition;
		}
	}
}
