using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CommandPatterns
{ 
    public abstract class Command 
    {
        public abstract void Execute(); 
        public abstract void Undo();    
    }
}
