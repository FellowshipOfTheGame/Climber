using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

public class LightRotation : MonoBehaviour
{
    public float radius;
	void Update ()
	{
        transform.localPosition = new Vector3(radius * Mathf.Cos(Time.realtimeSinceStartup), 0, radius * Mathf.Sin(Time.realtimeSinceStartup));
	}
}
