using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShatterObject : MonoBehaviour {
	public int NumberOfPieces = 16;
	public float xPower = 100, xRadius = 2, upForce = 80;
	public GameObject shatterEffect, piecePrefab;

	public void Shatter(GameObject go, float collisionSpeed){

		if(shatterEffect)
			Instantiate(shatterEffect, go.transform.position, go.transform.rotation);

		// These pieces are destroyed after 3 seconds
		if(piecePrefab){
			
			// Create pieces
			Vector3 selfPos = go.transform.position;
			Quaternion selfRot = go.transform.rotation;
			for (int i = 0; i < NumberOfPieces; i++){

				Vector3 position = new Vector3(
					selfPos.x + UnityEngine.Random.Range(-0.5f, 0.5f),
					selfPos.y + UnityEngine.Random.Range(-0.5f, 0.5f),
					selfPos.z + UnityEngine.Random.Range(-0.5f, 0.5f));

				Vector3 rotation = new Vector3(
					UnityEngine.Random.Range(-50f, 50f),
					UnityEngine.Random.Range(-50f, 50f),
					UnityEngine.Random.Range(-50f, 50f));

				GameObject p = Instantiate (piecePrefab, position, selfRot);
				p.transform.Rotate(rotation);
				Destroy (p, 2f);
			}

			Vector3 pos = go.transform.position;
			Collider[] colliders = Physics.OverlapSphere(pos, 2, 11);

			foreach(Collider col in colliders) {
				if(col.gameObject.CompareTag("Piece")){
					float force = xPower*collisionSpeed;
					col.GetComponent<Rigidbody>().AddExplosionForce(force, pos, xRadius, upForce);
				}
			}
		}
	}
}
