using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandPatterns;

public class CreateObjCommanad : Command
{
    EditorObjectData data;
    Vector3 pos;
    Quaternion rot;
    GameObject inst = null;

    public CreateObjCommanad(EditorObjectData data, Vector3 pos, Quaternion rot)
    {
        this.data = data;
        this.pos = pos;
        this.rot = rot;
    }
    public override void Execute()
    {
        inst = ObjManager.ObjInstantiate(data.prefab, pos, rot);
    }

    public override void Undo()
    {
        ObjManager.Destroy(inst);
    }
}