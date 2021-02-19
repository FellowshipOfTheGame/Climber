using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class Shooter : MonoBehaviour {
	public string PlayerPrefixe = "Single";

	public float force = 100.0f;
	public AudioClip shotSFX;

	public Transform RightArm;
	public GameObject projectile;
	public float Fire1Frequency;
	private float _fire1TimeCounter = 0f;

	public Transform LeftArm;
	public GameObject projectile2;
	public float Fire2Frequency;
	private float _fire2TimeCounter = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		_fire1TimeCounter += Time.deltaTime; 
		_fire2TimeCounter += Time.deltaTime; 
		if (PlayerPrefixe == "Single") {
			if (Input.GetButtonDown ("Fire3") && _fire1TimeCounter >= (1f / Fire1Frequency)) {
				_fire1TimeCounter = 0f;
				shoot (projectile, RightArm);
			}
			if (Input.GetButtonDown ("Fire2") && _fire2TimeCounter >= (1f / Fire2Frequency)) {
				_fire2TimeCounter = 0f;
				shoot (projectile2, LeftArm);
			}
		} else {
			if (Input.GetButtonDown (PlayerPrefixe + "Fire3") && _fire1TimeCounter >= (1f / Fire1Frequency)) {
				_fire1TimeCounter = 0f;
				shoot (projectile, RightArm);
			}
			if (Input.GetButtonDown(PlayerPrefixe + "Fire2") && _fire2TimeCounter >= (1f / Fire2Frequency)) {
				_fire2TimeCounter = 0f;
				shoot (projectile2, LeftArm);
			}
		}
	}

	void shoot(GameObject bullet, Transform arm){
		GameObject rb = Instantiate (bullet, arm.position, arm.rotation);
		rb.GetComponent<Rigidbody>().AddForce (arm.forward * force, ForceMode.Impulse);
		if (shotSFX){
			AudioSource.PlayClipAtPoint (shotSFX, arm.position);
		}
	}
}
