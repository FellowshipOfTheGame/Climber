using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		print (col.gameObject.name + " Win");
		if(col.gameObject.name == "Player1") SceneManager.LoadScene("Player1Win");
		if(col.gameObject.name == "Player2") SceneManager.LoadScene("Player2Win");
	}
}