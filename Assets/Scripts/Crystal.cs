using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class Crystal : MonoBehaviour
{
    public AnimationCurve falloffCurve;
    public float falloffMultiplier;
    public float minFalloff;
    public AnimationCurve freezeCurve;
    public float freezeTime;
    
    public bool IsCrystalized
    {
        get { return _isCrystalized; }
    }


    private Material inside;
    private Material outside;
    private MeshRenderer render;
    private bool crystalizing;
    private bool _isCrystalized;
    
	void Start ()
    {
        render = GetComponent<MeshRenderer>();
        outside = render.materials[0];
        inside = render.materials[1];
        crystalizing = false;
        _isCrystalized = false;
        if (!inside || !outside)
            Debug.LogError("Not enough materials");
	}

    public void Effect()
    {
        if (!crystalizing && !_isCrystalized)
            StartCoroutine(Crystalize());
    }

    private IEnumerator Crystalize()
    {
        crystalizing = true;
        float time = 0f;
        while (time < freezeTime)
        {
            //inside.SetFloat("_Falloff", falloffCurve.Evaluate(time / freezeTime) * (falloffMultiplier - minFalloff) + minFalloff);
            //inside.SetFloat("_Minimum", freezeCurve.Evaluate(time / freezeTime));
            outside.SetFloat("_Falloff", falloffCurve.Evaluate(time / freezeTime) * (falloffMultiplier - minFalloff) + minFalloff);
            outside.SetFloat("_Minimum", freezeCurve.Evaluate(time / freezeTime));
            yield return null;
            time += Time.deltaTime;
        }
        crystalizing = false;
        _isCrystalized = true;
    }
}
