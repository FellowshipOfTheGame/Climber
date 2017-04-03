using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

[CustomEditor(typeof(Floor))]
public class FloorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Floor floor = (Floor)target;
        if (GUILayout.Button("Rotate"))
        {
            Rotate(floor);
        }
        if (GUILayout.Button("Create"))
        {
            Reset(floor);
            Rotate(floor);
        }
    }

    private void Rotate(Floor floor)
    {
        foreach (var child in floor.GetComponentsInChildren<Transform>())
        {
            if (child == floor.transform)
                continue;
            Vector3 euler = floor.prefab.transform.rotation.eulerAngles;
            child.localRotation = Quaternion.Euler( (floor.lockX ? 0 : 1) * Rand.Range(0, 3) * 90f + euler.x,
                                                    (floor.lockY ? 0 : 1) * Rand.Range(0, 3) * 90f + euler.y,
                                                    (floor.lockZ ? 0 : 1) * Rand.Range(0, 3) * 90f + euler.z);
        }
    }

    private void Reset(Floor floor)
    {
        foreach (var child in floor.GetComponentsInChildren<Transform>())
        {
            if (child == floor.transform)
                continue;
            DestroyImmediate(child.gameObject);
        }
        for (int x = 0; x < floor.floorSize.x; x++)
        {
            for (int y = 0; y < floor.floorSize.y; y++)
            {
                GameObject obj = Instantiate<GameObject>(floor.prefab);
                obj.transform.SetParent(floor.gameObject.transform);
                obj.transform.localPosition = new Vector3(floor.prefabSize.x * x, 0, floor.prefabSize.y * y);
            }
        }
    }
}