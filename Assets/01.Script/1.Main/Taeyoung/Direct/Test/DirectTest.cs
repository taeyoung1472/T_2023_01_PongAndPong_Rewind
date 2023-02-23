using UnityEngine;

public class DirectTest : MonoBehaviour
{
    void Start()
    {
        DirectManager.Instance.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DirectManager.Instance.ActiveDirect();
        }
    }
}
