using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{
    public bool lockX, lockY, lockZ;
    public Vector2 floorSize;
    public Vector2 prefabSize = new Vector2(1, 1);
    public GameObject prefab;
}
