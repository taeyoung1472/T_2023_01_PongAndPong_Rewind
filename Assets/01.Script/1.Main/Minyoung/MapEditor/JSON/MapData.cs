using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapData
{
    public List<int> index = new();
    public List<Vector3> pos = new();
    public List<Quaternion> rot = new();
    public List<Vector3> scale = new();
}

