using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {
	private Rigidbody _Rigid;
	public float MoveSpeed = 1;
	// Use this for initialization
	void Start () {
		_Rigid = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		_Rigid.velocity = new Vector2(MoveSpeed*Input.GetAxis("Horizontal"), MoveSpeed*Input.GetAxis("Vertical"));
		
	}
}
