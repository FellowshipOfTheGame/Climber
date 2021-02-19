using UnityEngine;
using System.Collections;

public class TimeDestructor : MonoBehaviour {
	public float time = 1.0f;
	private float _timeCounter;

	// Use this for initialization
	void Start () {
		_timeCounter = 0f;
	}

	void Update(){
		_timeCounter += Time.deltaTime;
		if (_timeCounter > time)
			DestroyNow ();
	}

	private void DestroyNow(){
		Destroy (gameObject);
	}
}
