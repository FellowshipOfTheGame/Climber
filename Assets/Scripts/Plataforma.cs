using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plataforma : MonoBehaviour {
	public enum State{ Moving, Freezing, Frozen };
	public State MyState = State.Moving;
	public float Speed;
	public Vector3 Direction;
	public float Range;
	public GameObject Explosion;

	private Rigidbody _Rigid;
	private Vector3 _initialPosition;
	private Vector3 _lastPosition;
	private Vector3 _destination;

	public MeshRenderer MyMesh;

	void Start(){
		_Rigid = GetComponent<Rigidbody> ();
		_initialPosition = transform.position;
		if(Mathf.Abs(Direction.x) <= 0)_Rigid.constraints |= RigidbodyConstraints.FreezePositionX;
		if(Mathf.Abs(Direction.y) <= 0)_Rigid.constraints |= RigidbodyConstraints.FreezePositionY;
		if(Mathf.Abs(Direction.z) <= 0)_Rigid.constraints |= RigidbodyConstraints.FreezePositionZ;
	}

	void Update(){
		if (MyState == State.Moving) {
			move ();
		} else if (MyState == State.Freezing) {
			freezingMove ();
		}
			
	}

	void move(){
		Vector3 aux = transform.position - _initialPosition;
		if (aux.magnitude > Range)
			changeDirection ();
		_Rigid.velocity = Direction * Speed;
		_lastPosition = transform.position;
	}

	void freezingMove (){
		Vector3 targetDir = _destination - transform.position;
		if (targetDir.sqrMagnitude < 0.01f) {
			MyState = State.Frozen;
			MyMesh.material.SetFloat("_Crystalization", 1f);
			_Rigid.velocity = Vector3.zero;
			transform.position = _destination;
			_Rigid.constraints |= RigidbodyConstraints.FreezePositionX;
			_Rigid.constraints |= RigidbodyConstraints.FreezePositionY;
			_Rigid.constraints |= RigidbodyConstraints.FreezePositionZ;

		} else {
			MyMesh.material.SetFloat("_Crystalization" , (1f - targetDir.magnitude));
			_Rigid.velocity = targetDir.normalized * Speed * (targetDir.magnitude + 0.3f);
		}

	}

	void changeDirection(){
		Direction *= -1f;
		transform.position = _lastPosition;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Projectile2") {
			if(Explosion != null) Instantiate (Explosion, transform.position, transform.rotation);
			Destroy(collision.gameObject);
			Destroy (gameObject);
		}
		if (MyState == State.Moving) {
			if (collision.gameObject.tag == "Cube") {
				changeDirection ();
				// Verificando se existe um cubo atraz dele, se tiver ira matar o cubo para ele não ficar "flicando" (ocilando em alta velociadade)
				if(!HaveSpaceIn(transform.position + Direction.normalized)){
					Destroy (gameObject);
				}
			}
			if (collision.gameObject.tag == "Projectile") {
				MyState = State.Freezing;
				Destroy(collision.gameObject);

				//calcular o destino em que o bloco sera congelado
				_destination = ChooseDestination ();
				if (_destination == Vector3.zero) {
					//se o destino estiver nulll é proque naquela direçaõ não ha espaço
					changeDirection ();
					_destination = ChooseDestination ();
					if (_destination == Vector3.zero)
						Destroy (gameObject);
				}
				_Rigid.constraints = RigidbodyConstraints.None;
				_Rigid.constraints = RigidbodyConstraints.FreezeRotation;
				GetComponent<TimeDestructor> ().enabled = false;
			}
		}

	}

	/* Calcula o destino em que o bloco sera congelado dado a direção atual, se não houver espaço para ser congelado, retornara NULL
	*/
	private Vector3 ChooseDestination(){
		float x;
		if (Mathf.Abs (Direction.x) > 0) {
			x = Mathf.Floor (transform.position.x);
			if (Direction.x > 0) x += Direction.x;
		} else {
			x = _initialPosition.x;
		}

		float y;
		if (Mathf.Abs (Direction.y) > 0) {
			y = Mathf.Floor (Mathf.Abs( transform.position.y));
			if (Direction.y > 0) y += Direction.y;
		} else {
			y = _initialPosition.y;
		}

		float z;
		if (Mathf.Abs (Direction.z) > 0) {
			z = Mathf.Floor (transform.position.z);
			if (Direction.z > 0) z += Direction.z;
		} else {
			z = _initialPosition.z;
		}
		Vector3 position = new Vector3 (x,y,z);
		if (HaveSpaceIn (position))
			return position;
		return Vector3.zero;
	}

	public bool HaveSpaceIn (Vector3 position){
		bool have = true;
		Collider[] hitColliders = Physics.OverlapSphere(position, 0.475f);
		int i = 0;
		//verificando se algum dos objetos no espaço é outro objeto alem de "mim"
		while (i < hitColliders.Length){
			if (hitColliders [i].gameObject.name != gameObject.name)
				have = false;
			i++;
		}
		return have;
	}
}
