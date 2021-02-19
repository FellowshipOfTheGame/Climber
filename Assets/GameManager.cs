using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public float WinTime;
	private float _time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_time += Time.deltaTime;
		if (_time > WinTime) {
			SceneManager.LoadScene("Menu");
		}
	}
}
