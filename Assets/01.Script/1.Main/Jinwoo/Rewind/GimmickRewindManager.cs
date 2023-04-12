using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRewindManager : MonoBehaviour
{

    public void OnRewind()
    {
      //  RewindManager.Instance.InitRewind 
    }
    public IEnumerator ActiveObj(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
class RewindGimmick
{
   public RewindGimmick(float time, GameObject obj)
    {
        this.time = time;
        this.obj = obj;
    }

    public float time;
    public GameObject obj;
}

