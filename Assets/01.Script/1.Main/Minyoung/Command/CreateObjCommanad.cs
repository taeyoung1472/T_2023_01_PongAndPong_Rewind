using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandPatterns;

namespace CommandPatterns.RebindKey
{
    public class CreateObjCommanad : Command
    {
        public TransformInfo trmInfoObj;

        public CreateObjCommanad(TransformInfo trmInfoObj)
        {
            this.trmInfoObj = trmInfoObj;
        }
        public override void Execute()
        {
            ObjManager.ObjInstantiate(trmInfoObj.gameObject, trmInfoObj.transform.position, trmInfoObj.transform.rotation);
        }

        public override void Undo()
        {
            ObjManager.Destroy(trmInfoObj.gameObject);
        }
    }
}