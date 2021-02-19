using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public Transform player;
	
	public Vector3 maxRange;
	public GameObject[] targets;
	public float timeToSpawn = 0.1f;

	private float nextTimeToSpawn;
	// Use this for initialization
	void Start () {
		nextTimeToSpawn = Time.time + timeToSpawn;	
	}

	// Update is called once per frame
	void Update () {
		if (nextTimeToSpawn < Time.time) {
			Spawn();
			nextTimeToSpawn = Time.time + timeToSpawn;
		}
		transform.position = new Vector3 (player.position.x, player.position.y, player.position.z);
	}

	private void Spawn(){
		float x;
		float y;
		float z;
		Vector3 position;
		do{
			x = Mathf.Floor(transform.position.x + Random.Range (-maxRange.x, maxRange.x));
			y = Mathf.Abs(Mathf.Floor(transform.position.y + Random.Range (-maxRange.y, maxRange.y)));
			z = Mathf.Floor(transform.position.z + Random.Range (-maxRange.z, maxRange.z));
			position = new Vector3(x, y, z);
		}while(HaveSpaceIn(position));

		int index = Random.Range (0, targets.Length);
		Instantiate (targets [index], position, new Quaternion(270f,0f,0f,1f));
	}

	public bool HaveSpaceIn (Vector3 position){
		return (Physics.OverlapSphere(position, 0.475f).Length > 0);
	}
}
