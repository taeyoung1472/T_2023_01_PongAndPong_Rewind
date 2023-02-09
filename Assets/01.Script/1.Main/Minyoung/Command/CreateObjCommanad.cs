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
            Debug.Log("이태영병신 ㅈ ㅜㄱ어라 신예린하고헤어져라");
            ObjManager.Destroy(trmInfoObj.gameObject);
        }
    }
}