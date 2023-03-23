using UnityEngine;

public abstract class GimmickObject : MonoBehaviour
{
    protected float recordPosY;
    public float RecordPosY { get { return recordPosY; } }
     
    public abstract void Init();
    public abstract void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse);
    public abstract void RecordTopPosition();
    public abstract bool IsGimmickable(GameObject gimmickObj);
}
