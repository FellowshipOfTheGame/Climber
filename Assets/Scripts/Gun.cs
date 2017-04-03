using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public Camera camera;

	void Start ()
	{
	}
	
	void Update ()
	{
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(new Ray(transform.position, camera.transform.forward), out hit))
            {
                Crystal crystal = hit.collider.GetComponent<Crystal>();
                if(crystal)
                {
                    crystal.Effect();
                }
            }
        }
	}
}
