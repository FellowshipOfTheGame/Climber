using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plataforma2 : MonoBehaviour {
	public enum State{ Moving, Freezing, Frozen };
	public State MyState = State.Moving;
	public float Speed;
	public Vector3 Direction;
	public float Range;

	private Vector3 _initialPosition;
	private Vector3 _lastPosition;
	private Vector3 _destination;
	private Renderer _rend;

	bool inBetween(float x, float x1, float x2){ return (x1 < x && x < x2); }

	void Start(){
		_rend = GetComponent<Renderer> ();
		_initialPosition = transform.position;
		_lastPosition = _initialPosition;
	}

	void Update(){

		if (MyState == State.Moving) {
			MovingMove ();
		} else if (MyState == State.Freezing) {
			freezingMove ();
		}
	}

	void move(){
		_lastPosition = transform.position;
		transform.Translate (Direction * Speed * Time.deltaTime);
	}

	void MovingMove(){
		Vector3 aux = transform.position - _initialPosition;
		if (aux.magnitude > Range)
			changeDirection ();
		move ();
	}
	void freezingMove (){
		Vector3 targetDir = _destination - transform.position;
		if (targetDir.sqrMagnitude < 0.01f) {
			MyState = State.Frozen;
			transform.position = _destination;
			GetComponent<BoxCollider> ().enabled = true;
		} else {
			Direction = targetDir.normalized;
			_rend.material.color = new Color(_rend.material.color.r - Time.deltaTime, _rend.material.color.g - Time.deltaTime, _rend.material.color.b - Time.deltaTime); 
			Speed = 0.1f + (Speed / 2f);
			move ();
		}
	}

	void changeDirection(){
		Direction *= -1f;
		transform.position = _lastPosition;
	}

	void OnCollisionEnter(Collision collision){
		print("Collision enter");
		if (MyState == State.Moving) {
			if(collision.gameObject.tag == "Cube") changeDirection ();
			if (collision.gameObject.tag == "Projectile") {
				MyState = State.Freezing;
				//calcular o destino em que o bloco sera congelado
				float x = Mathf.Floor(transform.position.x);
				float y = Mathf.Floor(transform.position.y);
				float z = Mathf.Floor(transform.position.z);
				if (Direction.x > 0) x += Direction.x;
				if (Direction.y > 0) x += Direction.y;
				if (Direction.z > 0) x += Direction.z;
				_destination = new Vector3 (x, y, z);
				print ("destination: " + _destination);
				GetComponent<BoxCollider> ().enabled = false;
				GetComponent<TimeDestructor> ().enabled = false;
			}
		}
	}
	void OnTriggerEnter(Collider col){
		print ("Trigger enter");

	}
}
