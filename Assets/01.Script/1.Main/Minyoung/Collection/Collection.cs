using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private int index;
    public Vector3 pos;
    private bool isEat;
    public bool IsEat => isEat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("SDdssd");
            Player player = other.GetComponent<Player>();
            isEat = true;
            gameObject.SetActive(false);
            StageManager.Instance.CurStageDataSO.stageCollection[index] = IsEat; //  .Add(IsEat);
        }
    }
}
